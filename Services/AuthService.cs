using FacturasAPI.Models;
using FacturasAPI.Repositories;
using System.Threading.Tasks;

namespace FacturasAPI.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUsuarioRepository _usuarioRepository;

        public AuthService(IUsuarioRepository usuarioRepository)
        {
            _usuarioRepository = usuarioRepository;
        }

        public async Task Register(Usuario usuario)
        {
            // Implementa la lógica de registro aquí
            await _usuarioRepository.CreateAsync(usuario);
        }

        public async Task<string?> Authenticate(string email, string password)
        {
            var usuario = await _usuarioRepository.GetByEmailAsync(email);
            if (usuario == null || !BCrypt.Net.BCrypt.Verify(password, usuario.Password))
            {
                return null;
            }

            // Implementa la lógica de generación de token aquí
            return "jwt-token"; // Reemplaza con la generación real de token
        }
    }
}
