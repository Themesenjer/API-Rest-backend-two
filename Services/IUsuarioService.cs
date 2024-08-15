// src/Services/IUsuarioService.cs
using FacturasAPI.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FacturasAPI.Services
{
    public interface IUsuarioService
    {
        Task<List<Usuario>> GetAllAsync();
    }
}
