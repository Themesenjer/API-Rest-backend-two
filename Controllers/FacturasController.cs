using FacturasAPI.Models;
using FacturasAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FacturasAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class FacturasController : ControllerBase
    {
        private readonly IFacturaService _facturaService;
        private readonly ILogger<FacturasController> _logger; // Agregar ILogger

        public FacturasController(IFacturaService facturaService, ILogger<FacturasController> logger)
        {
            _facturaService = facturaService;
            _logger = logger; // Inicializar ILogger
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Factura>>> GetAll()
        {
            _logger.LogInformation("Iniciando GET all facturas");
            var facturas = await _facturaService.GetAllAsync();
            return Ok(facturas);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Factura>> GetById(string id)
        {
            _logger.LogInformation("Iniciando GET factura con ID: {Id}", id);
            var factura = await _facturaService.GetByIdAsync(id);
            if (factura == null)
            {
                _logger.LogWarning("Factura con ID: {Id} no encontrada", id);
                return NotFound();
            }
            return Ok(factura);
        }

        [HttpPost]
        public async Task<ActionResult> Create(Factura factura)
        {
            _logger.LogInformation("Iniciando POST para crear nueva factura");
            await _facturaService.CreateAsync(factura);
            return CreatedAtAction(nameof(GetById), new { id = factura.Id }, factura);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, Factura factura)
        {
            _logger.LogInformation("Iniciando PUT para actualizar factura con ID: {Id}", id);
            var existingFactura = await _facturaService.GetByIdAsync(id);
            if (existingFactura == null)
            {
                _logger.LogWarning("Factura con ID: {Id} no encontrada para actualización", id);
                return NotFound();
            }
            // Mantener el campo _id del documento existente
            factura.Id = id;
            await _facturaService.UpdateAsync(id, factura);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            _logger.LogInformation("Iniciando DELETE para eliminar factura con ID: {Id}", id);
            var existingFactura = await _facturaService.GetByIdAsync(id);
            if (existingFactura == null)
            {
                _logger.LogWarning("Factura con ID: {Id} no encontrada para eliminación", id);
                return NotFound();
            }
            await _facturaService.DeleteAsync(id);
            return NoContent();
        }
    }
}