using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using Bhasha.Common.Importers;
using NUnit.Framework;

namespace Bhasha.Common.Tests.Importers
{
    [TestFixture]
    public class ChapterDtoTests
    {
        private const string SampleId = "Bhasha.Common.Tests.Importers.ChapterDtoSample.json";
        
        [Test]
        public void Deserialize_from_JSON()
        {
            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            };

            using var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(SampleId);
            using var reader = new StreamReader(stream);

            var json = reader.ReadToEnd();

            var result = JsonSerializer.Deserialize<ChapterDto>(json, options);

            Assert.That(result.From == Language.English);
            Assert.That(result.To == Language.Bengali);
            Assert.That(result.Level == 1);

            Assert.That(result.Name.Native.Native == "animals");
            Assert.That(result.Name.Native.Spoken == "animals");
            Assert.That(result.Name.Token.Label == "animals");
            Assert.That(result.Name.Token.Cefr == "A2");
            Assert.That(result.Name.Token.TokenType == "Noun");
            Assert.That(result.Name.Token.Categories.Length == 0);

            Assert.That(result.Description.Native.Native == "simple animals");
            Assert.That(result.Description.Native.Spoken == "simple animals");
            Assert.That(result.Description.Token.Label == "simple animals");
            Assert.That(result.Description.Token.Cefr == "A2");
            Assert.That(result.Description.Token.TokenType == "Phrase");
            Assert.That(result.Description.Token.Categories.SequenceEqual(new[] { "animals" }));

            Assert.That(result.Pages.Length == 3);

            Assert.That(result.Pages[0].Token.Label == "dog");
            Assert.That(result.Pages[0].Token.Cefr == "A1");
            Assert.That(result.Pages[0].Token.TokenType == "Noun");
            Assert.That(result.Pages[0].Token.Categories.SequenceEqual(new[] { "pets", "animals" }));
            Assert.That(result.Pages[0].From.Native == "dog");
            Assert.That(result.Pages[0].From.Spoken == "dog");
            Assert.That(result.Pages[0].To.Native == "কুকুর");
            Assert.That(result.Pages[0].To.Spoken == "Kukura");

            Assert.That(result.Pages[1].Token.Label == "cat");
            Assert.That(result.Pages[1].Token.Cefr == "A1");
            Assert.That(result.Pages[1].Token.TokenType == "Noun");
            Assert.That(result.Pages[1].Token.Categories.SequenceEqual(new[] { "pets", "animals" }));
            Assert.That(result.Pages[1].From.Native == "cat");
            Assert.That(result.Pages[1].From.Spoken == "kat");
            Assert.That(result.Pages[1].To.Native == "বিড়াল");
            Assert.That(result.Pages[1].To.Spoken == "Biṛāla");

            Assert.That(result.Pages[2].Token.Label == "cats and dogs are cute");
            Assert.That(result.Pages[2].Token.Cefr == "A2");
            Assert.That(result.Pages[2].Token.TokenType == "Phrase");
            Assert.That(result.Pages[2].Token.Categories.SequenceEqual(new[] { "pets", "animals" }));
            Assert.That(result.Pages[2].From.Native == "cats and dogs are cute");
            Assert.That(result.Pages[2].From.Spoken == "kats and dogs are kute");
            Assert.That(result.Pages[2].To.Native == "বিড়াল এবং কুকুর সুন্দর");
            Assert.That(result.Pages[2].To.Spoken == "Biṛāla ēbaṁ kukura sundara");
            Assert.That(result.Pages[2].Tips.Length == 1);
        }
    }
}
