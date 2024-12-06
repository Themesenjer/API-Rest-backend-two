using FacturasAPI.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FacturasAPI.Repositories
{
    public interface IUsuarioRepository
    {
        Task<Usuario?> GetByEmailAsync(string email);
        Task CreateAsync(Usuario usuario);
        Task<List<Usuario>> GetAllAsync();  // Nuevo método para obtener todos los usuarios
    }
}
