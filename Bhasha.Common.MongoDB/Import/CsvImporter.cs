using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Bhasha.Common.Extensions;
using Bhasha.Common.MongoDB.Dto;

namespace Bhasha.Common.MongoDB.Import
{
    public static class CsvImporter
    {
        private static readonly IEnumerable<string> RequiredColumns = new []
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
        
        public static TokenDto[] EnglishBengli(string file)
        {
            var lines = File.ReadAllLines(file);
            var columns = lines[0].Split(',');

            var missingColumns = RequiredColumns.Where(x => !columns.Contains(x));
            if (missingColumns.Any())
            {
                throw new ArgumentException("missing columns: " + string.Join(", ", missingColumns));
            }

            var columnIndex = RequiredColumns.ToDictionary(
                x => x,
                x => columns.IndexOf(x));

            var dtos = lines
                .Skip(1)
                .Select(x => x.Split(','))
                .Select(x => new TokenDto {
                    Label = x[columnIndex["eng_native"]],
                    Level = int.Parse(x[columnIndex["level"]]),
                    Cefr = x[columnIndex["cefr"]],
                    TokenType = x[columnIndex["type"]],
                    Categories = x[columnIndex["categories"]].Split(';'),
                    Translations = new Dictionary<string, LanguageTokenDto> {
                        {
                            Language.English, new LanguageTokenDto
                            {
                                Native = x[columnIndex["eng_native"]],
                                Spoken = x[columnIndex["eng_spoken"]]
                            }
                        },
                        {
                            Language.Bengoli, new LanguageTokenDto
                            {
                                Native = x[columnIndex["bn_native"]],
                                Spoken = x[columnIndex["bn_spoken"]]
                            }
                        }
                    }
                });

            return dtos.ToArray();
        }
    }
}
