using System.Collections.Generic;
using System.Threading.Tasks;
using FacturasAPI.Models;
using MongoDB.Driver;
using Microsoft.Extensions.Options;

namespace FacturasAPI.Repositories
{
    public class FacturaRepository : IFacturaRepository
    {
        private readonly IMongoCollection<Factura> _facturas;

        public FacturaRepository(IMongoDatabase database)
        {
            _facturas = database.GetCollection<Factura>("Facturas");
        }

        public async Task<IEnumerable<Factura>> GetAll()
        {
            return await _facturas.Find(f => true).ToListAsync();
        }

        public async Task<Factura> GetById(string id)
        {
            return await _facturas.Find(f => f.Id == id).FirstOrDefaultAsync();
        }

        public async Task Create(Factura factura)
        {
            await _facturas.InsertOneAsync(factura);
        }

        public async Task<bool> Update(Factura factura)
        {
            var result = await _facturas.ReplaceOneAsync(f => f.Id == factura.Id, factura);
            return result.IsAcknowledged && result.ModifiedCount > 0;
        }

        public async Task<bool> Delete(string id)
        {
            var result = await _facturas.DeleteOneAsync(f => f.Id == id);
            return result.IsAcknowledged && result.DeletedCount > 0;
        }
    }
}
