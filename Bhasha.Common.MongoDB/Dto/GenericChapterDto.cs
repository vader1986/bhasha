using System;
using System.Linq;
using Bhasha.Common.MongoDB.Attributes;
using MongoDB.Bson.Serialization.Attributes;

namespace Bhasha.Common.MongoDB.Dto
{
    [MongoCollection(Names.Collections.Chapters)]
    public class GenericChapterDto : Dto, IEquatable<GenericChapter>
    {
        [BsonElement]
        public int Level { get; set; }

        [BsonElement]
        public string Name { get; set; } = string.Empty;

        [BsonElement]
        public string Description { get; set; } = string.Empty;

        [BsonElement]
        public GenericPageDto[] Pages { get; set; } = new GenericPageDto[0];

        [BsonElement]
        public string? PictureId { get; set; }

        public bool Equals(GenericChapter other)
        {
            return other != null && other.Id == Id && other.Level == Level && other.Name == Name && other.Description == Description && other.PictureId == PictureId && other.Pages.Length == Pages.Length && other.Pages.Select((x, i) => Pages[i].Equals(x)).All(x => x);
        }
    }
}
