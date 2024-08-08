using FacturasAPI.Models;
using FacturasAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FacturasAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class FacturasController : ControllerBase
    {
        private readonly IFacturaService _facturaService;

        public FacturasController(IFacturaService facturaService)
        {
            _facturaService = facturaService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var facturas = await _facturaService.GetAllAsync();
            return Ok(new
            {
                code = 200,
                messages = new[] { "Consulta exitosa" },
                data = facturas
            });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var factura = await _facturaService.GetByIdAsync(id);
            if (factura == null)
            {
                return NotFound(new
                {
                    code = 404,
                    message = "Factura no encontrada"
                });
            }
            return Ok(new
            {
                code = 200,
                messages = new[] { "Consulta exitosa" },
                data = factura
            });
        }

        [HttpPost]
        public async Task<IActionResult> Create(Factura factura)
        {
            await _facturaService.CreateAsync(factura);
            return CreatedAtAction(nameof(GetById), new { id = factura.Id }, new
            {
                code = 201,
                messages = new[] { "Factura creada exitosamente" },
                data = factura
            });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, Factura factura)
        {
            var existingFactura = await _facturaService.GetByIdAsync(id);
            if (existingFactura == null)
            {
                return NotFound(new
                {
                    code = 404,
                    message = "Factura no encontrada para actualización"
                });
            }
            await _facturaService.UpdateAsync(id, factura);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var existingFactura = await _facturaService.GetByIdAsync(id);
            if (existingFactura == null)
            {
                return NotFound(new
                {
                    code = 404,
                    message = "Factura no encontrada para eliminación"
                });
            }
            await _facturaService.DeleteAsync(id);
            return NoContent();
        }
    }
}
