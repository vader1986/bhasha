using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Bhasha.Common.Arguments;
using Bhasha.Common.Services;
using Bhasha.Common.Tests.Support;
using Moq;
using NUnit.Framework;

namespace Bhasha.Common.Tests.Services
{
    [TestFixture]
    public class TranslateChapterTests
    {
        private Mock<ITranslate<Guid, TranslatedExpression>> _translator;
        private Mock<IArgumentAssemblyProvider> _arguments;
        private Mock<IAssembleArguments> _assembly;
        private TranslateChapter _chapter;

        [SetUp]
        public void Before()
        {
            _translator = new Mock<ITranslate<Guid, TranslatedExpression>>();
            _assembly = new Mock<IAssembleArguments>();
            _arguments = new Mock<IArgumentAssemblyProvider>();
            _arguments
                .Setup(x => x.GetAssembly(It.IsAny<PageType>()))
                .Returns(_assembly.Object);
            _chapter = new TranslateChapter(_translator.Object, _arguments.Object);
        }

        [Test]
        public async Task Translate_DbChapterWithMissingTranslations_ReturnsNull()
        {
            // setup
            var chapter = DbChapterBuilder.Default.Build();
            var profile = ProfileBuilder.Default.Build();

            _translator
                .Setup(x => x.Translate(It.IsAny<Guid>(), It.IsAny<Language>()))
                .ReturnsAsync(default(TranslatedExpression));

            // act
            var result = await _chapter.Translate(chapter, profile);

            // assert
            Assert.That(result, Is.Null);
        }

        [Test]
        public async Task Translate_DbChapterForProfile_ReturnsChapter()
        {
            // setup
            var chapter = DbChapterBuilder.Default.Build();
            var profile = ProfileBuilder.Default.Build();

            _translator
                .Setup(x => x.Translate(It.IsAny<Guid>(), It.IsAny<Language>()))
                .ReturnsAsync((Guid id, Language _) => {

                    var expression = ExpressionBuilder
                        .Default
                        .WithId(id)
                        .Build();

                    return TranslatedExpressionBuilder
                        .Default
                        .WithExpression(expression)
                        .Build();
                });

            var argument = new object();

            _assembly
                .Setup(x => x.Assemble(It.IsAny<IEnumerable<TranslatedExpression>>(), It.IsAny<Guid>()))
                .Returns(argument);

            // act
            var result = await _chapter.Translate(chapter, profile);

            // assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Pages, Is.Not.Null);
            Assert.That(result.Pages.Length, Is.EqualTo(chapter.Pages.Length));
        }
    }
}
