using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace FacturasAPI.Models
{
    public class Usuario
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = ObjectId.GenerateNewId().ToString();

        [BsonElement("email")]
        public required string Email { get; set; } // Usando el modificador required

        [BsonElement("password")]
        public required string Password { get; set; } // Usando el modificador required
    }
}
