using FacturasAPI.Models;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FacturasAPI.Repositories
{
    public class FacturaRepository : IFacturaRepository
    {
        private readonly IMongoCollection<Factura> _facturas;

        public FacturaRepository(IMongoDatabase database)
        {
            _facturas = database.GetCollection<Factura>("Facturas");
        }

        public async Task<IEnumerable<Factura>> GetAllAsync()
        {
            return await _facturas.Find(factura => true).ToListAsync();
        }

        public async Task<Factura> GetByIdAsync(string id)
        {
            return await _facturas.Find<Factura>(factura => factura.Id == id).FirstOrDefaultAsync();
        }

        public async Task CreateAsync(Factura factura)
        {
            await _facturas.InsertOneAsync(factura);
        }

        public async Task UpdateAsync(string id, Factura factura)
        {
            var filter = Builders<Factura>.Filter.Eq(f => f.Id, id);

            // Mantener el campo _id igual al del documento existente
            var updateDefinition = Builders<Factura>.Update
                .Set(f => f.Descripcion, factura.Descripcion);

            await _facturas.UpdateOneAsync(filter, updateDefinition);
        }
        public async Task DeleteAsync(string id)
        {
            await _facturas.DeleteOneAsync(factura => factura.Id == id);
        }
    }
}
