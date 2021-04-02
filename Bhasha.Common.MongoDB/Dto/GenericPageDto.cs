using System;
using System.Linq;
using MongoDB.Bson.Serialization.Attributes;

namespace Bhasha.Common.MongoDB.Dto
{
    public class GenericPageDto : IEquatable<GenericPage>
    {
        [BsonElement]
        public Guid TokenId { get; set; }

        [BsonElement]
        public string PageType { get; set; } = string.Empty;

        [BsonElement]
        public Guid[] TipIds { get; set; } = new Guid[0];

        public bool Equals(GenericPage other)
        {
            return other != null && other.PageType.ToString() == PageType && other.TokenId == TokenId && other.TipIds.SequenceEqual(TipIds);
        }
    }
}
