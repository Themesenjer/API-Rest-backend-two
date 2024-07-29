using Microsoft.AspNetCore.Mvc;
using FacturasAPI.Models;
using FacturasAPI.Repositories;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FacturasAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class FacturasController : ControllerBase
    {
        private readonly IFacturaRepository _facturaRepository;

        public FacturasController(IFacturaRepository facturaRepository)
        {
            _facturaRepository = facturaRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Factura>>> Get()
        {
            var facturas = await _facturaRepository.GetAll();
            return Ok(facturas);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Factura>> GetById(string id)
        {
            var factura = await _facturaRepository.GetById(id);
            if (factura == null)
            {
                return NotFound();
            }
            return Ok(factura);
        }

        [HttpPost]
        public async Task<ActionResult<Factura>> Post(Factura factura)
        {
            await _facturaRepository.Create(factura);
            return CreatedAtAction(nameof(GetById), new { id = factura.Id }, factura);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, Factura factura)
        {
            var existingFactura = await _facturaRepository.GetById(id);
            if (existingFactura == null)
            {
                return NotFound();
            }

            factura.Id = existingFactura.Id;
            var result = await _facturaRepository.Update(factura);
            if (!result)
            {
                return BadRequest();
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var result = await _facturaRepository.Delete(id);
            if (!result)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
