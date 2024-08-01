using FacturasAPI.Models;
using FacturasAPI.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace FacturasAPI.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IConfiguration _configuration;

        public AuthService(IUsuarioRepository usuarioRepository, IConfiguration configuration)
        {
            _usuarioRepository = usuarioRepository;
            _configuration = configuration;
        }

        public async Task Register(Usuario usuario)
        {
            // Encriptar la contraseña antes de guardar el usuario
            usuario.Password = BCrypt.Net.BCrypt.HashPassword(usuario.Password);
            await _usuarioRepository.CreateAsync(usuario);
        }

        public async Task<string?> Authenticate(string email, string password)
        {
            var usuario = await _usuarioRepository.GetByEmailAsync(email);
            if (usuario == null || !BCrypt.Net.BCrypt.Verify(password, usuario.Password))
            {
                return null;
            }

            // Obtener y verificar la clave JWT
            var jwtKey = _configuration["Jwt:Key"];
            if (string.IsNullOrEmpty(jwtKey))
            {
                throw new InvalidOperationException("JWT key is not configured.");
            }

            var key = Encoding.ASCII.GetBytes(jwtKey);

            // Generar el token JWT
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, usuario.Email)
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                Issuer = _configuration["Jwt:Issuer"],
                Audience = _configuration["Jwt:Issuer"]
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
