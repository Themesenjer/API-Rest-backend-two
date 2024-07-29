using FacturasAPI.Models;
using MongoDB.Driver;
using System.Threading.Tasks;

namespace FacturasAPI.Repositories
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly IMongoCollection<Usuario> _usuarios;

        public UsuarioRepository(IMongoDatabase database)
        {
            _usuarios = database.GetCollection<Usuario>("Usuarios");
        }

        public async Task<Usuario?> GetByEmailAsync(string email)
        {
            return await _usuarios.Find(u => u.Email == email).FirstOrDefaultAsync();
        }

        public async Task CreateAsync(Usuario usuario)
        {
            await _usuarios.InsertOneAsync(usuario);
        }
    }
}
