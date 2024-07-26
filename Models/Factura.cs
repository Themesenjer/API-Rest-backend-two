using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace FacturasAPI.Models
{
    public class Factura
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        public string Descripcion { get; set; } = null!;
    }
}
