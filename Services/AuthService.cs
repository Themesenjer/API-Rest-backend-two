using FacturasAPI.Models;
using FacturasAPI.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging; // Agregar el uso de ILogger
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
        private readonly ILogger<AuthService> _logger; // Agregar ILogger

        public AuthService(IUsuarioRepository usuarioRepository, IConfiguration configuration, ILogger<AuthService> logger)
        {
            _usuarioRepository = usuarioRepository;
            _configuration = configuration;
            _logger = logger; // Inicializar ILogger
        }

        public async Task Register(Usuario usuario)
        {
            _logger.LogInformation("Registrando nuevo usuario: {Email}", usuario.Email);
            // Encriptar la contraseña antes de guardar el usuario
            usuario.Password = BCrypt.Net.BCrypt.HashPassword(usuario.Password);
            await _usuarioRepository.CreateAsync(usuario);
        }

        public async Task<string?> Authenticate(string email, string password)
        {
            _logger.LogInformation("Autenticando usuario: {Email}", email);
            var usuario = await _usuarioRepository.GetByEmailAsync(email);
            if (usuario == null || !BCrypt.Net.BCrypt.Verify(password, usuario.Password))
            {
                _logger.LogWarning("Autenticación fallida para usuario: {Email}", email);
                return null;
            }

            // Obtener y verificar la clave JWT
            var jwtKey = _configuration["Jwt:Key"];
            if (string.IsNullOrEmpty(jwtKey))
            {
                _logger.LogError("JWT key is not configured.");
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
            _logger.LogInformation("Token JWT generado para usuario: {Email}", email);
            return tokenHandler.WriteToken(token);
        }
    }
}
