using MongoDB.Bson.Serialization.Attributes;

namespace Bhasha.Common.MongoDB.Dto
{
    [MongoCollection(Names.Collections.Chapters)]
    public class GenericChapterDto : Dto
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
    }
}
