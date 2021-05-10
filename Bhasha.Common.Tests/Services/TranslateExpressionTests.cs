using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Bhasha.Common.Database;
using Bhasha.Common.Services;
using Bhasha.Common.Tests.Support;
using Moq;
using NUnit.Framework;

namespace Bhasha.Common.Tests.Services
{
    [TestFixture]
    public class TranslateExpressionTests
    {
        private Mock<IStore<DbExpression>> _expressions;
        private Mock<ITranslate<Guid, TranslatedWord>> _words;
        private Mock<IWordsPhraseConverter> _wordsToPhrase;
        private TranslateExpression _expression;

        [SetUp]
        public void Before()
        {
            _expressions = new Mock<IStore<DbExpression>>();
            _words = new Mock<ITranslate<Guid, TranslatedWord>>();
            _wordsToPhrase = new Mock<IWordsPhraseConverter>();
            _expression = new TranslateExpression(
                _expressions.Object,
                _words.Object,
                _wordsToPhrase.Object);
        }

        [Test]
        public async Task Translate_ExpressionIdForNonExistingExpression_ReturnsNull()
        {
            // setup
            var expressionId = Guid.NewGuid();

            _expressions
                .Setup(x => x.Get(expressionId))
                .ReturnsAsync(default(DbExpression));

            // act
            var result = await _expression.Translate(expressionId, Language.Bengali);

            // assert
            Assert.That(result, Is.Null);
        }

        [Test]
        public async Task Translate_ExpressionIdForNonExistingTranslation_ReturnsNull()
        {
            // setup
            var language = Language.English;
            var expressionId = Guid.NewGuid();
            var expression = DbExpressionBuilder.Default.Build();

            if (expression.Translations.ContainsKey(language))
            {
                expression.Translations.Remove(language);
            }

            _expressions
                .Setup(x => x.Get(expressionId))
                .ReturnsAsync(expression);

            // act
            var result = await _expression.Translate(expressionId, language);

            // assert
            Assert.That(result, Is.Null);
        }

        [Test]
        public async Task Translate_ExpressionIdForMissingWordTranslation_ReturnsNull()
        {
            // setup
            var language = Language.English;
            var expressionId = Guid.NewGuid();
            var expression = DbExpressionBuilder.Default.Build();

            _expressions
                .Setup(x => x.Get(expressionId))
                .ReturnsAsync(expression);

            _words
                .Setup(x => x.Translate(It.IsAny<Guid>(), language))
                .ReturnsAsync(default(TranslatedWord));

            // act
            var result = await _expression.Translate(expressionId, language);

            // assert
            Assert.That(result, Is.Null);
        }

        [Test]
        public async Task Translate_ExpressionIdToLanguage_ReturnsTranslatedExpression()
        {
            // setup
            var language = Language.English;
            var expressionId = Guid.NewGuid();
            var expression = DbExpressionBuilder.Default.Build();

            _expressions
                .Setup(x => x.Get(expressionId))
                .ReturnsAsync(expression);

            var word = TranslatedWordBuilder.Default.Build();

            _words
                .Setup(x => x.Translate(It.IsAny<Guid>(), language))
                .ReturnsAsync(word);

            _wordsToPhrase
                .Setup(x => x.Convert(It.IsAny<IEnumerable<string>>(), language))
                .Returns("test");

            // act
            var result = await _expression.Translate(expressionId, language);

            // assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Native, Is.EqualTo("test"));
            Assert.That(result.Spoken, Is.EqualTo("test"));
            Assert.That(result.Expression, Is.EqualTo(
                new Expression(expression.Id, expression.ExprType, expression.Cefr)));
        }
    }
}
