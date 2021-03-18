using System;
using MongoDB.Bson.Serialization.Attributes;

namespace Bhasha.Common.MongoDB.Dto
{
    [MongoCollection(Names.Collections.Tips)]
    public class TipDto : Dto, IEquatable<Tip>
    {
        [BsonElement]
        public Guid ChapterId { get; set; }

        [BsonElement]
        public int PageIndex { get; set; }

        [BsonElement]
        public string Text { get; set; } = string.Empty;

        public bool Equals(Tip other)
        {
            return other != null && other.Id == Id && other.ChapterId == ChapterId && other.PageIndex == PageIndex && other.Text == Text;
        }
    }
}
