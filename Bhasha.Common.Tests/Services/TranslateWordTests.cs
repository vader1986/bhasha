using System;
using System.Threading.Tasks;
using Bhasha.Common.Database;
using Bhasha.Common.Services;
using Bhasha.Common.Tests.Support;
using Moq;
using NUnit.Framework;

namespace Bhasha.Common.Tests.Services
{
    [TestFixture]
    public class TranslateWordTests
    {
        private Mock<IStore<DbWord>> _words;
        private TranslateWord _word;

        [SetUp]
        public void Before()
        {
            _words = new Mock<IStore<DbWord>>();
            _word = new TranslateWord(_words.Object);
        }

        [Test]
        public async Task Translate_WordIdForMissingWord_ReturnsNull()
        {
            // setup
            var wordId = Guid.NewGuid();

            _words
                .Setup(x => x.Get(wordId))
                .ReturnsAsync(default(DbWord));

            // act
            var result = await _word.Translate(wordId, Language.Bengali);

            // assert
            Assert.That(result, Is.Null);
        }

        [Test]
        public async Task Translate_WordIdForMissingTranslation_ReturnsNull()
        {
            // setup
            var wordId = Guid.NewGuid();
            var word = DbWordBuilder.Default.Build();

            _words
                .Setup(x => x.Get(wordId))
                .ReturnsAsync(word);

            if (word.Translations.ContainsKey(Language.Bengali))
            {
                word.Translations.Remove(Language.Bengali);
            }

            // act
            var result = await _word.Translate(wordId, Language.Bengali);

            // assert
            Assert.That(result, Is.Null);
        }

        [Test]
        public async Task Translate_WordIdIntoLanguage_ReturnsTranslatedWord()
        {
            // setup
            var wordId = Guid.NewGuid();
            var word = DbWordBuilder.Default.Build();
            var translation = word.Translations[Language.Bengali];

            _words
                .Setup(x => x.Get(wordId))
                .ReturnsAsync(word);

            // act
            var result = await _word.Translate(wordId, Language.Bengali);

            // assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Native, Is.EqualTo(translation.Native));
            Assert.That(result.Spoken, Is.EqualTo(translation.Spoken));
        }
    }
}
