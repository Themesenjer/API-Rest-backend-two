using FacturasAPI.Models;

namespace FacturasAPI.Services
{
    public interface IAuthService
    {
        Task Register(Usuario usuario);
        Task<string?> Authenticate(string email, string password);
    }
}
