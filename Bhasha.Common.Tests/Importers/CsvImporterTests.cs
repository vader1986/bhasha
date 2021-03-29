using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using Bhasha.Common.Importers;
using Bhasha.Common.Services;
using FakeItEasy;
using NUnit.Framework;

namespace Bhasha.Common.Tests.Importers
{
    [TestFixture]
    public class CsvImporterTests
    {
        private const string SampleId = "Bhasha.Common.Tests.Importers.CsvImporterSample.csv";
        private const string SampleFile = "sample.csv";

        private IStore<Token> _tokens;
        private IStore<Translation> _translations;
        private CsvImporter _importer;

        [SetUp]
        public void Before()
        {
            _tokens = A.Fake<IStore<Token>>();
            _translations = A.Fake<IStore<Translation>>();
            _importer = new CsvImporter(_tokens, _translations);

            using var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(SampleId);
            using var reader = new StreamReader(stream);

            var content = reader.ReadToEnd();

            File.WriteAllText(SampleFile, content);
        }

        [TearDown]
        public void After()
        {
            File.Delete(SampleFile);
        }

        [Test]
        public async Task ImportEnBn_sample_file()
        {
            await _importer.ImportEnBn(SampleFile);

            A.CallTo(() => _tokens.Add(A<Token>._))
                .MustHaveHappened(2, Times.Exactly);

            A.CallTo(() => _tokens.Add(A<Token>
                .That.Matches(
                    x => x.Cefr == CEFR.A1 &&
                         x.Label == "cat" &&
                         x.Level == 1 &&
                         x.TokenType == TokenType.Noun)))
                .MustHaveHappenedOnceExactly();

            A.CallTo(() => _tokens.Add(A<Token>
                .That.Matches(
                    x => x.Cefr == CEFR.A1 &&
                         x.Label == "dog" &&
                         x.Level == 1 &&
                         x.TokenType == TokenType.Noun)))
                .MustHaveHappenedOnceExactly();

            A.CallTo(() => _translations.Add(A<Translation>._))
                .MustHaveHappened(4, Times.Exactly);

            A.CallTo(() => _translations.Add(A<Translation>
                .That.Matches(
                    x => x.Language == Language.English &&
                         x.Native == "dog" &&
                         x.Spoken == "dog")))
                .MustHaveHappenedOnceExactly();

            A.CallTo(() => _translations.Add(A<Translation>
                .That.Matches(
                    x => x.Language == Language.English &&
                         x.Native == "cat" &&
                         x.Spoken == "kat")))
                .MustHaveHappenedOnceExactly();

            A.CallTo(() => _translations.Add(A<Translation>
                .That.Matches(
                    x => x.Language == Language.Bengali &&
                         x.Native == "বিড়াল" &&
                         x.Spoken == "Biṛāla")))
                .MustHaveHappenedOnceExactly();

            A.CallTo(() => _translations.Add(A<Translation>
                .That.Matches(
                    x => x.Language == Language.Bengali &&
                         x.Native == "কুকুর" &&
                         x.Spoken == "Kukura")))
                .MustHaveHappenedOnceExactly();
        }
    }
}
