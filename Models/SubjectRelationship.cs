using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace CarrerasService.Models
{
    public class SubjectRelationship
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("subjectCode")]
        public string SubjectCode { get; set; }

        [BsonElement("relatedSubjectCode")]
        public string RelatedSubjectCode { get; set; }

        [BsonElement("type")]
        public string Type { get; set; } // "prerequisite" o "postrequisite"
    }
}
