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
            Assert.That(dtos[0].Level == 1);
            Assert.That(dtos[0].Cefr == "A1");
            Assert.That(dtos[0].TokenType == "Noun");
            Assert.That(dtos[0].Categories.Length == 2);
            Assert.That(dtos[0].Categories[0] == "animal");
            Assert.That(dtos[0].Categories[1] == "pet");
            Assert.That(dtos[0].Translations.ContainsKey(Language.English));
            Assert.That(dtos[0].Translations[Language.English].Native == "cat");
            Assert.That(dtos[0].Translations[Language.English].Spoken == "kat");
            Assert.That(dtos[0].Translations.ContainsKey(Language.Bengoli));
            Assert.That(dtos[0].Translations[Language.Bengoli].Native == "বিড়াল");
            Assert.That(dtos[0].Translations[Language.Bengoli].Spoken == "Biṛāla");

            Assert.That(dtos[1].Label == "dog");
            Assert.That(dtos[1].Level == 1);
            Assert.That(dtos[1].Cefr == "A1");
            Assert.That(dtos[1].TokenType == "Noun");
            Assert.That(dtos[1].Categories.Length == 2);
            Assert.That(dtos[1].Categories[0] == "animal");
            Assert.That(dtos[1].Categories[1] == "pet");
            Assert.That(dtos[1].Translations.ContainsKey(Language.English));
            Assert.That(dtos[1].Translations[Language.English].Native == "dog");
            Assert.That(dtos[1].Translations[Language.English].Spoken == "dog");
            Assert.That(dtos[1].Translations.ContainsKey(Language.Bengoli));
            Assert.That(dtos[1].Translations[Language.Bengoli].Native == "কুকুর");
            Assert.That(dtos[1].Translations[Language.Bengoli].Spoken == "Kukura");
        }
    }
}
