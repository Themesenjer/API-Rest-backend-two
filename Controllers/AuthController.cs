using FacturasAPI.Models;
using FacturasAPI.Services;
using Microsoft.AspNetCore.Mvc;
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
            return Ok(new
            {
                code = 200,
                messages = new[] { "Usuario registrado exitosamente" },
                data = usuario
            });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel login)
        {
            var authResponse = await _authService.Authenticate(login.Email, login.Password);
            if (authResponse == null)
            {
                return Unauthorized(new
                {
                    code = 401,
                    message = "Credenciales incorrectas"
                });
            }

            return Ok(new
            {
                code = 200,
                messages = new[] { "Inicio de sesión exitoso" },
                data = authResponse
            });
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh([FromBody] RefreshTokenRequest request)
        {
            var authResponse = await _authService.RefreshToken(request.Token, request.RefreshToken);
            if (authResponse == null)
            {
                return Unauthorized(new
                {
                    code = 401,
                    message = "Refresh token inválido"
                });
            }

            return Ok(new
            {
                code = 200,
                messages = new[] { "Token refrescado exitosamente" },
                data = authResponse
            });
        }
    }

    public class LoginModel
    {
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
    }
}
