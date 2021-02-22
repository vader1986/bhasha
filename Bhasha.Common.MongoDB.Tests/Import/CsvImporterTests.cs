using System.IO;
using System.Reflection;
using Bhasha.Common.MongoDB.Import;
using NUnit.Framework;

namespace Bhasha.Common.MongoDB.Tests.Import
{
    [TestFixture]
    public class CsvImporterTests
    {
        private const string SampleId = "Bhasha.Common.MongoDB.Tests.Import.Sample.csv";
        private const string SampleFile = "sample.csv";

        [SetUp]
        public void Before()
        {
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
        public void Import_valid_csv_file()
        {
            var dtos = CsvImporter.EnglishBengli(SampleFile);

            Assert.That(dtos.Length == 2);

            Assert.That(dtos[0].Label == "cat");
            Assert.That(dtos[0].Level == "A1");
            Assert.That(dtos[0].TokenType == "Noun");
            Assert.That(dtos[0].Categories.Length == 2);
            Assert.That(dtos[0].Categories[0] == "animal");
            Assert.That(dtos[0].Categories[1] == "pet");
            Assert.That(dtos[0].Tokens.Length == 2);
            Assert.That(dtos[0].Tokens[0].LanguageId == Languages.English);
            Assert.That(dtos[0].Tokens[0].Native == "cat");
            Assert.That(dtos[0].Tokens[0].Spoken == "kat");
            Assert.That(dtos[0].Tokens[1].LanguageId == Languages.Bengoli);
            Assert.That(dtos[0].Tokens[1].Native == "বিড়াল");
            Assert.That(dtos[0].Tokens[1].Spoken == "Biṛāla");

            Assert.That(dtos[1].Label == "dog");
            Assert.That(dtos[1].Level == "A1");
            Assert.That(dtos[1].TokenType == "Noun");
            Assert.That(dtos[1].Categories.Length == 2);
            Assert.That(dtos[1].Categories[0] == "animal");
            Assert.That(dtos[1].Categories[1] == "pet");
            Assert.That(dtos[1].Tokens.Length == 2);
            Assert.That(dtos[1].Tokens[0].LanguageId == Languages.English);
            Assert.That(dtos[1].Tokens[0].Native == "dog");
            Assert.That(dtos[1].Tokens[0].Spoken == "dog");
            Assert.That(dtos[1].Tokens[1].LanguageId == Languages.Bengoli);
            Assert.That(dtos[1].Tokens[1].Native == "কুকুর");
            Assert.That(dtos[1].Tokens[1].Spoken == "Kukura");
        }
    }
}
