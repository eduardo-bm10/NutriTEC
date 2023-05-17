using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace API_MongoDB.Models {
    public class Nutritionist {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; } = null!;

        [BsonElement("Nutricionista")]
        public string NombreNutricionista { get; set; } = null!;

        [BsonElement("Paciente")]
        public string NombrePaciente { get; set; } = null!;

        [BsonElement("Fecha")]
        public string Fecha { get; set; } = null!;

        [BsonElement("Mensaje")]
        public string Mensaje { get; set; } = null!;

    }
}