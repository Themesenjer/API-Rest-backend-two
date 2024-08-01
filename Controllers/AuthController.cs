using Microsoft.AspNetCore.Mvc;
using FacturasAPI.Models;
using FacturasAPI.Services;
using System.Threading.Tasks;
using MongoDB.Bson;

namespace FacturasAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(Usuario usuario)
        {
            if (!ObjectId.TryParse(usuario.Id, out var objectId))
            {
                usuario.Id = ObjectId.GenerateNewId().ToString();
            }

            await _authService.Register(usuario);
            return Ok();
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel login)
        {
            var token = await _authService.Authenticate(login.Email, login.Password);
            if (token == null)
            {
                return Unauthorized();
            }

            return Ok(new AuthResponse { Token = token, Email = login.Email });
        }
    }

    public class LoginModel
    {
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
    }
}
