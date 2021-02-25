using MongoDB.Bson.Serialization.Attributes;

namespace Bhasha.Common.MongoDB.Dto
{
    [BsonIgnoreExtraElements]
    public class ProcedureDto
    {
        [BsonElement]
        public string ProcedureId { get; set; } = "";

        [BsonElement]
        public string Description { get; set; } = "";

        [BsonElement]
        public string[]? Tutorial { get; set; }

        [BsonElement]
        public string? AudioId { get; set; }

        [BsonElement]
        public string[] Support { get; set; } = new string[0];
    }
}
