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
        public Guid NameId { get; set; }

        [BsonElement]
        public Guid DescriptionId { get; set; }

        [BsonElement]
        public GenericPageDto[] Pages { get; set; } = new GenericPageDto[0];

        public bool Equals(GenericChapter other)
        {
            return other != null && other.Id == Id && other.Level == Level && other.NameId == NameId && other.DescriptionId == DescriptionId && other.Pages.Length == Pages.Length && other.Pages.Select((x, i) => Pages[i].Equals(x)).All(x => x);
        }
    }
}
