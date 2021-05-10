using System.Linq;
using Bhasha.Common.Arguments;
using Bhasha.Common.Services;

namespace Bhasha.Common.Database
{
    public class DatabaseTypeConverter
        : IConvert<DbTranslatedWord, TranslatedWord>,
          IConvert<DbTranslatedExpression, TranslatedExpression, Language>,
          IConvert<DbTranslatedChapter, Chapter>,
          IConvert<DbUserProfile, Profile>,
          IConvert<DbStats, Stats>
    {
        private readonly IWordsPhraseConverter _wordsToPhrase;
        private readonly IArgumentAssemblyProvider _argumentAssemblies;

        public DatabaseTypeConverter(IWordsPhraseConverter wordsToPhrase, IArgumentAssemblyProvider argumentAssemblies)
        {
            _wordsToPhrase = wordsToPhrase;
            _argumentAssemblies = argumentAssemblies;
        }

        public TranslatedExpression Convert(DbTranslatedExpression input, Language language)
        {
            input.Validate();

            var expression = new Expression(input.ExpressionId, input.ExprType, input.Cefr);
            var words = input.Words.Select(Convert).ToArray();
            var native = _wordsToPhrase.Convert(words.Select(word => word.Native), language);
            var spoken = _wordsToPhrase.Convert(words.Select(word => word.Spoken), language);

            return new TranslatedExpression(expression, words, native, spoken);
        }

        public TranslatedWord Convert(DbTranslatedWord input)
        {
            input.Validate();

            var word = new Word(input.Id, input.PartOfSpeech, input.Cefr, input.PictureId);
            var native = input.Translation!.Native!;
            var spoken = input.Translation!.Spoken!;
            var audioId = input.Translation!.AudioId;

            return new TranslatedWord(word, native, spoken, audioId);
        }

        public Chapter Convert(DbTranslatedChapter input)
        {
            input.Validate();

            var native = input.Languages!.Native!;
            var target = input.Languages!.Target!;

            var targets = input.Pages.Select(page => Convert(page.Target!, target));

            object Assembly(DbTranslatedPage page)
            {
                var assembly = _argumentAssemblies.GetAssembly(page.PageType);
                return assembly.Assemble(targets!, page.Native!.ExpressionId);
            }

            var name = Convert(input.Name!, native);
            var description = Convert(input.Description!, native);
            var pictureId = input.PictureId;
            var pages = input.Pages.Select(page => new Page(page.PageType, Convert(page.Native!, native), Assembly(page))).ToArray();

            return new Chapter(input.ChapterId, input.Level, name, description, pages, pictureId);
        }

        public Profile Convert(DbUserProfile input)
        {
            input.Validate();

            var native = input.Languages!.Native!;
            var target = input.Languages!.Target!;

            return new Profile(input.Id, input.UserId!, native, target, input.Level, input.CompletedChapters);
        }

        public Stats Convert(DbStats input)
        {
            input.Validate();

            return new Stats(input.ProfileId, input.ChapterId, input.Completed, input.Tips!, input.Submits!, input.Failures!);
        }
    }
}
