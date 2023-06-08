using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace API_MongoDB.Models {
    
    /// <summary>
    /// Feedback Class: Saves the retrieved information of feedbacks from the Mongo Database.
    /// </summary> 
    public class Feedback {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; } = null!;

        [BsonElement("Sender")]
        public string SenderSsn { get; set; } = null!;

        [BsonElement("Receptor")]
        public string ReceptorSsn { get; set; } = null!;

        [BsonElement("Date")]
        public string Date { get; set; } = null!;

        [BsonElement("Message")]
        public string Message { get; set; } = null!;

    }
}