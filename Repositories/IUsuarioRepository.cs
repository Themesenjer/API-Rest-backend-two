using FacturasAPI.Models;
using System.Threading.Tasks;

namespace FacturasAPI.Repositories
{
    public interface IUsuarioRepository
    {
        Task<Usuario?> GetByEmailAsync(string email);
        Task CreateAsync(Usuario usuario);
    }
}
