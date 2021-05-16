using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bhasha.Common.Database;
using Bhasha.Common.Services;

namespace Bhasha.Common.Importers
{
    public class ChapterImporter
    {
        private readonly IStore<DbTranslatedChapter> _translatedChapters;
        private readonly IStore<DbChapter> _chapters;
        private readonly IStore<DbExpression> _expressions;
        private readonly IStore<DbWord> _words;
        private readonly ITranslate<Guid, TranslatedExpression> _exprTranslator;
        private readonly ITranslate<Guid, TranslatedWord> _wordTranslator;
        
        public ChapterImporter(IStore<DbTranslatedChapter> translatedChapters, IStore<DbChapter> chapters, IStore<DbExpression> expressions, IStore<DbWord> words, ITranslate<Guid, TranslatedExpression> exprTranslator, ITranslate<Guid, TranslatedWord> wordTranslator)
        {
            _translatedChapters = translatedChapters;
            _chapters = chapters;
            _expressions = expressions;
            _words = words;
            _exprTranslator = exprTranslator;
            _wordTranslator = wordTranslator;
        }

        private async Task<bool> TryUseExisting(DbTranslatedWord word, Language language)
        {
            var existing = await _wordTranslator.Translate(word.Id, language);
            if (existing != null)
            {
                word.Cefr = existing.Word.Cefr;
                word.PartOfSpeech = existing.Word.PartOfSpeech;
                word.PictureId = existing.Word.PictureId;
                word.Translation = new DbTranslation {
                    Native = existing.Native,
                    Spoken = existing.Spoken,
                    AudioId = existing.AudioId
                };

                return true;
            }

            return false;
        }

        private async Task<bool> TryAddTranslation(DbTranslatedWord word, Language language)
        {
            var existing = await _words.Get(word.Id);
            if (existing != null && !existing.Translations!.ContainsKey(language))
            {
                existing.Translations.Add(language, word.Translation!);
                await _words.Replace(existing);

                return true;
            }

            return false;
        }

        private async Task ImportWord(DbTranslatedWord word, Language language)
        {
            if (await TryUseExisting(word, language))
            {
                return;
            }

            if (await TryAddTranslation(word, language))
            {
                return;
            }

            word.Validate();

            var newWord = new DbWord {
                Cefr = word.Cefr,
                PartOfSpeech = word.PartOfSpeech,
                PictureId = word.PictureId,
                Translations = new Dictionary<string, DbTranslation>
                {
                    { language, word.Translation! }
                }
            };

            var importedWord = await _words.Add(newWord);
            word.Id = importedWord.Id;
        }

        private async Task<bool> TryAddTranslation(DbTranslatedExpression expression, Language language)
        {
            var existing = await _expressions.Get(expression.ExpressionId);

            if (existing != null && !existing.Translations!.ContainsKey(language))
            {
                var wordsIds = expression.Words.Select(x => x.Id).ToArray();
                existing.Translations.Add(language, new DbWords { WordIds = wordsIds });
                await _expressions.Replace(existing);

                return true;
            }

            return false;
        }

        private async Task<bool> TryUseExisting(DbTranslatedExpression expression, Language language)
        {
            var existing = await _exprTranslator.Translate(expression.ExpressionId, language);
            if (existing != null)
            {
                expression.Cefr = existing.Expression.Cefr;
                expression.ExprType = existing.Expression.ExprType;
                expression.Words = existing.Words.Select(
                    word => new DbTranslatedWord
                    {
                        Id = word.Word.Id,
                        Cefr = word.Word.Cefr,
                        PartOfSpeech = word.Word.PartOfSpeech,
                        PictureId = word.Word.PictureId,
                        Translation = new DbTranslation
                        {
                            Native = word.Native,
                            Spoken = word.Spoken,
                            AudioId = word.AudioId
                        }
                    }).ToArray();
                return true;
            }
            else
            {
                return false;
            }
        }

        private async Task ImportExpression(DbTranslatedExpression expression, Language language)
        {
            if (await TryUseExisting(expression, language))
            {
                return;
            }

            if (await TryAddTranslation(expression, language))
            {
                return;
            }

            await Task.WhenAll(expression.Words.Select(word => ImportWord(word, language)));

            expression.Validate();

            var wordIds = expression.Words.Select(word => word.Id).ToArray();
            var newExpression = new DbExpression {
                Cefr = expression.Cefr,
                ExprType = expression.ExprType,
                Translations = new Dictionary<string, DbWords> {
                    { language, new DbWords { WordIds = wordIds } }
                }
            };

            var importedExpression = await _expressions.Add(newExpression);
            expression.ExpressionId = importedExpression.Id;
        }

        public Task<DbExpression> Import(DbExpression expression)
        {
            expression.Validate();
            return _expressions.Add(expression);
        }

        public Task<DbWord> Import(DbWord word)
        {
            word.Validate();
            return _words.Add(word);
        }

        public async Task<DbTranslatedChapter> Import(DbTranslatedChapter chapter)
        {
            var expressions = chapter.Pages
                .Select(page => page.Native)
                .Append(chapter.Name)
                .Append(chapter.Description);

            await Task.WhenAll(expressions.Select(expr => ImportExpression(expr!, chapter.Languages!.Native!)));
            await Task.WhenAll(chapter.Pages.Select(page => ImportExpression(page.Target!, chapter.Languages!.Target!)));

            var dbChapter = new DbChapter {
                Level = chapter.Level,
                NameId = chapter.Name!.ExpressionId,
                DescriptionId = chapter.Description!.ExpressionId,
                PictureId = chapter.PictureId,
                Pages = chapter.Pages.Select(page => new DbPage {
                    ExpressionId = page.Native!.ExpressionId,
                    PageType = page.PageType,
                }).ToArray()
            };

            dbChapter = await _chapters.Add(dbChapter);

            chapter.Id = dbChapter.Id;
            chapter.Validate();

            return await _translatedChapters.Add(chapter);
        }
    }
}
