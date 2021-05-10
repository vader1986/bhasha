using System;
using System.Collections.Generic;
using Bhasha.Common.Arguments;
using Bhasha.Common.Database;
using Bhasha.Common.Services;
using Bhasha.Common.Tests.Support;
using Moq;
using NUnit.Framework;

namespace Bhasha.Common.Tests.Database
{
    [TestFixture]
    public class DatabaseTypeConverterTests
    {
        private Mock<IWordsPhraseConverter> _wordsToPhrase;
        private Mock<IAssembleArguments> _assembly;
        private Mock<IArgumentAssemblyProvider> _argumentAssemblies;
        private DatabaseTypeConverter _converter;

        [SetUp]
        public void Before()
        {
            _wordsToPhrase = new Mock<IWordsPhraseConverter>();
            _assembly = new Mock<IAssembleArguments>();
            _argumentAssemblies = new Mock<IArgumentAssemblyProvider>();
            _argumentAssemblies
                .Setup(x => x.GetAssembly(It.IsAny<PageType>()))
                .Returns(_assembly.Object);

            _converter = new DatabaseTypeConverter(
                _wordsToPhrase.Object,
                _argumentAssemblies.Object);
        }

        [Test]
        public void Convert_DbTranslatedExpression_ReturnsTranslatedExpression()
        {
            // setup
            var expression = DbTranslatedExpressionBuilder.Default.Build();

            _wordsToPhrase
                .Setup(x => x.Convert(It.IsAny<IEnumerable<string>>(), It.IsAny<Language>()))
                .Returns("good");

            // act
            var result = _converter.Convert(expression, Language.Bengali);

            // assert
            Assert.NotNull(result);
            Assert.That(result.Words.Length, Is.EqualTo(expression.Words.Length));
            Assert.That(result.Native, Is.EqualTo("good"));
            Assert.That(result.Spoken, Is.EqualTo("good"));
        }

        [Test]
        public void Convert_DbTranslatedWord_ReturnsTranslatedWord()
        {
            // setup
            var translatedWord = DbTranslatedWordBuilder.Default.Build();

            // act
            var result = _converter.Convert(translatedWord);

            // assert
            Assert.NotNull(result);
            Assert.That(result.Native, Is.EqualTo(translatedWord.Translation.Native));
            Assert.That(result.Spoken, Is.EqualTo(translatedWord.Translation.Spoken));
        }

        [Test]
        public void Convert_DbTranslatedChapter_ReturnsChapter()
        {
            // setup
            var translatedChapter = DbTranslatedChapterBuilder.Default.Build();

            _assembly
                .Setup(x => x.Assemble(It.IsAny<IEnumerable<TranslatedExpression>>(), It.IsAny<Guid>()))
                .Returns(new object());

            // act
            var result = _converter.Convert(translatedChapter);

            // assert
            Assert.NotNull(result);
            Assert.That(result.Pages.Length, Is.EqualTo(translatedChapter.Pages.Length));
        }

        [Test]
        public void Convert_DbUserProfile_ReturnsProfile()
        {
            // setup
            var userProfile = DbUserProfileBuilder.Default.Build();

            // act
            var result = _converter.Convert(userProfile);

            // assert
            Assert.NotNull(result);
            Assert.That(result.UserId, Is.EqualTo(userProfile.UserId));
            Assert.That(result.Id, Is.EqualTo(userProfile.Id));
            Assert.That(result.Level, Is.EqualTo(userProfile.Level));
            Assert.That(result.Native.ToString(), Is.EqualTo(userProfile.Languages.Native));
            Assert.That(result.Target.ToString(), Is.EqualTo(userProfile.Languages.Target));
        }

        [Test]
        public void Convert_DbStats_ReturnsStats()
        {
            // setup
            var stats = DbStatsBuilder.Default.Build();

            // act
            var result = _converter.Convert(stats);

            // assert
            Assert.NotNull(result);
            Assert.That(result.ChapterId, Is.EqualTo(stats.ChapterId));
            Assert.That(result.ProfileId, Is.EqualTo(stats.ProfileId));
            Assert.That(result.Completed, Is.EqualTo(stats.Completed));
            Assert.That(result.Tips, Is.EqualTo(stats.Tips));
            Assert.That(result.Submits, Is.EqualTo(stats.Submits));
            Assert.That(result.Failures, Is.EqualTo(stats.Failures));
        }
    }
}
