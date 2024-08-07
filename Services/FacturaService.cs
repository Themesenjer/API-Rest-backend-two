using FacturasAPI.Models;
using FacturasAPI.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FacturasAPI.Services
{
    public class FacturaService : IFacturaService
    {
        private readonly IFacturaRepository _facturaRepository;

        public FacturaService(IFacturaRepository facturaRepository)
        {
            _facturaRepository = facturaRepository;
        }

        public async Task<IEnumerable<Factura>> GetAllAsync()
        {
            return await _facturaRepository.GetAllAsync();
        }

        public async Task<Factura> GetByIdAsync(string id)
        {
            return await _facturaRepository.GetByIdAsync(id);
        }

        public async Task CreateAsync(Factura factura)
        {
            await _facturaRepository.CreateAsync(factura);
        }

        public async Task UpdateAsync(string id, Factura factura)
        {
            await _facturaRepository.UpdateAsync(id, factura);
        }

        public async Task DeleteAsync(string id)
        {
            await _facturaRepository.DeleteAsync(id);
        }
    }
}
