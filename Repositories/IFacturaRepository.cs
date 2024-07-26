using System.Collections.Generic;
using System.Threading.Tasks;
using FacturasAPI.Models;

namespace FacturasAPI.Repositories
{
    public interface IFacturaRepository
    {
        Task<IEnumerable<Factura>> GetAll();
        Task<Factura> GetById(string id);
        Task Create(Factura factura);
        Task<bool> Update(Factura factura);
        Task<bool> Delete(string id);
    }
}
