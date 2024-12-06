// src/Services/UsuarioService.cs
using FacturasAPI.Models;
using FacturasAPI.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FacturasAPI.Services
{
    public class UsuarioService : IUsuarioService
    {
        private readonly IUsuarioRepository _usuarioRepository;

        public UsuarioService(IUsuarioRepository usuarioRepository)
        {
            _usuarioRepository = usuarioRepository;
        }

        public async Task<List<Usuario>> GetAllAsync()
        {
            return await _usuarioRepository.GetAllAsync();
        }
    }
}
