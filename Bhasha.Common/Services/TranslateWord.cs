using System;
using System.Threading.Tasks;
using Bhasha.Common.Database;

namespace Bhasha.Common.Services
{
    public class TranslateWord : ITranslate<Guid, TranslatedWord>
    {
        private readonly IStore<DbWord> _words;

        public TranslateWord(IStore<DbWord> words)
        {
            _words = words;
        }

        public async Task<TranslatedWord?> Translate(Guid wordId, Language language)
        {
            var dbWord = await _words.Get(wordId);
            if (dbWord == null)
            {
                return default;
            }

            var translations = dbWord.Translations;
            if (translations == null || !translations.ContainsKey(language))
            {
                return default;
            }

            var translation = translations[language];
            var word = new Word(dbWord.Id, dbWord.PartOfSpeech, dbWord.Cefr, dbWord.PictureId);

            return new TranslatedWord(word, translation.Native!, translation.Spoken!, translation.AudioId);
        }
    }
}
