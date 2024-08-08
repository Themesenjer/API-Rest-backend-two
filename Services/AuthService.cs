using FacturasAPI.Models;
using FacturasAPI.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace FacturasAPI.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IConfiguration _configuration;
        private readonly ILogger<AuthService> _logger;
        private static readonly Dictionary<string, string> _refreshTokens = new(); // Almacenamiento en memoria para los refresh tokens

        public AuthService(IUsuarioRepository usuarioRepository, IConfiguration configuration, ILogger<AuthService> logger)
        {
            _usuarioRepository = usuarioRepository;
            _configuration = configuration;
            _logger = logger;
        }

        public async Task Register(Usuario usuario)
        {
            _logger.LogInformation("Registrando nuevo usuario: {Email}", usuario.Email);
            // Encriptar la contraseña antes de guardar el usuario
            usuario.Password = BCrypt.Net.BCrypt.HashPassword(usuario.Password);
            await _usuarioRepository.CreateAsync(usuario); // Asegurarse de usar await aquí
        }

        public async Task<AuthResponse?> Authenticate(string email, string password)
        {
            _logger.LogInformation("Autenticando usuario: {Email}", email);
            var usuario = await _usuarioRepository.GetByEmailAsync(email); // Asegurarse de usar await aquí
            if (usuario == null || !BCrypt.Net.BCrypt.Verify(password, usuario.Password))
            {
                _logger.LogWarning("Autenticación fallida para usuario: {Email}", email);
                return null;
            }

            var token = GenerateJwtToken(usuario.Email);
            var refreshToken = GenerateRefreshToken();

            // Guardar el refresh token
            _refreshTokens[refreshToken] = usuario.Email;

            _logger.LogInformation("Token JWT generado para usuario: {Email}", email);
            return new AuthResponse
            {
                Token = token,
                RefreshToken = refreshToken,
                Email = usuario.Email
            };
        }

        public Task<AuthResponse?> RefreshToken(string token, string refreshToken)
        {
            if (!_refreshTokens.TryGetValue(refreshToken, out var email))
            {
                _logger.LogWarning("Refresh token inválido.");
                return Task.FromResult<AuthResponse?>(null);
            }

            _refreshTokens.Remove(refreshToken); // Eliminar el refresh token usado

            var newToken = GenerateJwtToken(email);
            var newRefreshToken = GenerateRefreshToken();

            // Guardar el nuevo refresh token
            _refreshTokens[newRefreshToken] = email;

            var response = new AuthResponse
            {
                Token = newToken,
                RefreshToken = newRefreshToken,
                Email = email
            };

            return Task.FromResult<AuthResponse?>(response);
        }

        private string GenerateJwtToken(string email)
        {
            var jwtKey = _configuration["Jwt:Key"];
            if (string.IsNullOrEmpty(jwtKey))
            {
                _logger.LogError("JWT key is not configured.");
                throw new InvalidOperationException("JWT key is not configured.");
            }

            var key = Encoding.ASCII.GetBytes(jwtKey);
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, email)
                }),
                Expires = DateTime.UtcNow.AddMinutes(5), // Tiempo de vida del access token
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                Issuer = _configuration["Jwt:Issuer"],
                Audience = _configuration["Jwt:Issuer"]
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        private string GenerateRefreshToken()
        {
            var randomBytes = new byte[64];
            RandomNumberGenerator.Fill(randomBytes);
            return Convert.ToBase64String(randomBytes);
        }
    }
}
