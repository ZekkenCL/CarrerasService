using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace CarrerasService.Models
{
    public class Subject
    {
        [BsonId]
        [BsonRepresentation(BsonType.Int32)]
        public int Id { get; set; }

        [BsonElement("name")]
        public string Name { get; set; }

        [BsonElement("department")]
        public string Department { get; set; }

        [BsonElement("credits")]
        public int Credits { get; set; }

        [BsonElement("semester")]
        public string Semester { get; set; }

        [BsonElement("internalId")]
        public string InternalId { get; set; }

        [BsonElement("code")]
        public string Code { get; set; }
    }
}
