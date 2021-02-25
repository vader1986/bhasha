using MongoDB.Bson.Serialization.Attributes;

namespace Bhasha.Common.MongoDB.Dto
{
    [BsonIgnoreExtraElements]
    public class UserProgressDto
    {
        [BsonElement]
        public string UserId { get; set; } = "";

        [BsonElement]
        public string From { get; set; } = "";

        [BsonElement]
        public string To { get; set; } = "";

        [BsonElement]
        public int GroupId { get; set; }

        [BsonElement]
        public int CompletedTokens { get; set; }

        [BsonElement]
        public int CompletedChapters { get; set; }

        [BsonElement]
        public int[] CompletedSequenceNumbers { get; set; } = new int[0];

        [BsonElement]
        public string Level { get; set; } = "";
    }
}
