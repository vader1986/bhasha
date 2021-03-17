using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Bhasha.Common.Extensions;
using Bhasha.Common.Services;

namespace Bhasha.Common.Importers
{
    public class CsvImporter
    {
        private static readonly ISet<string> RequiredColumns = new HashSet<string>
        {
            "eng_native",
            "eng_spoken",
            "bn_native",
            "bn_spoken",
            "categories",
            "level",
            "cefr",
            "type"
        };

        private readonly IStore<Token> _tokens;
        private readonly IStore<Translation> _translations;

        public CsvImporter(IStore<Token> tokens, IStore<Translation> translations)
        {
            _tokens = tokens;
            _translations = translations;
        }

        public async Task ImportEnBn(string file)
        {
            var lines = await File.ReadAllLinesAsync(file);
            var columns = lines[0].Split(',');

            var missingColumns = RequiredColumns.Where(x => !columns.Contains(x));
            if (missingColumns.Any())
            {
                throw new ArgumentException("missing columns: " + string.Join(", ", missingColumns));
            }

            var columnIndex = RequiredColumns.ToDictionary(
                x => x,
                x => columns.IndexOf(x));

            async Task ImportRow(string[] row)
            {
                var token = new Token(
                    default,
                    row[columnIndex["eng_native"]],
                    int.Parse(row[columnIndex["level"]]),
                    Enum.Parse<CEFR>(row[columnIndex["cefr"]]),
                    Enum.Parse<TokenType>(row[columnIndex["type"]]),
                    row[columnIndex["categories"]].Split(';'));

                token = await _tokens.Add(token);

                var english = new Translation(
                    token.Id,
                    Language.English,
                    row[columnIndex["eng_native"]],
                    row[columnIndex["eng_spoken"]]);

                await _translations.Add(english);

                var bengoli = new Translation(
                    token.Id,
                    Language.Bengoli,
                    row[columnIndex["bn_native"]],
                    row[columnIndex["bn_spoken"]]);

                await _translations.Add(bengoli);
            }

            var dtos = lines
                .Skip(1)
                .Select(x => x.Split(','))
                .Select(ImportRow);

            await Task.WhenAll(dtos);
        }
    }
}
