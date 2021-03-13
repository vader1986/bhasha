using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;

namespace Bhasha.Common.MongoDB.Dto
{
    using Translations = Dictionary<string, LanguageTokenDto>;

    public class TokenDto
    {
        [BsonId]
        public Guid Id { get; set; }

        [BsonElement]
        public string Label { get; set; } = string.Empty;

        [BsonElement]
        public int Level { get; set; }

        [BsonElement]
        public string Cefr { get; set; } = string.Empty;

        [BsonElement]
        public string TokenType { get; set; } = string.Empty;

        [BsonElement]
        public string[] Categories { get; set; } = new string[0];

        [BsonElement]
        public string? PictureId { get; set; }

        [BsonElement]
        public Translations Translations { get; set; } = new Translations();
    }
}
