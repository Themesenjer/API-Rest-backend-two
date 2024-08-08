using FacturasAPI.Models;
using System.Threading.Tasks;

namespace FacturasAPI.Services
{
    public interface IAuthService
    {
        Task Register(Usuario usuario);
        Task<AuthResponse?> Authenticate(string email, string password); // Cambio de tipo de retorno a AuthResponse?
        Task<AuthResponse?> RefreshToken(string token, string refreshToken); // Agrega el método RefreshToken
    }
}
