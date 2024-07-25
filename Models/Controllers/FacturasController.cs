// Controllers/FacturasController.cs
using Microsoft.AspNetCore.Mvc;
using FacturasAPI.Models;
using System.Collections.Generic;
using System.Linq;

namespace FacturasAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FacturasController : ControllerBase
    {
        private static List<Factura> facturas = new List<Factura>
        {
            new Factura { Id = 1, Descripcion = "Factura 1" },
            new Factura { Id = 2, Descripcion = "Factura 2" }
        };

        // GET: api/facturas
        [HttpGet]
        public ActionResult<IEnumerable<Factura>> GetFacturas()
        {
            return facturas;
        }

        // GET: api/facturas/5
        [HttpGet("{id}")]
        public ActionResult<Factura> GetFactura(int id)
        {
            var factura = facturas.FirstOrDefault(f => f.Id == id);

            if (factura == null)
            {
                return NotFound();
            }

            return factura;
        }

        // POST: api/facturas
        [HttpPost]
        public ActionResult<Factura> PostFactura(Factura factura)
        {
            if (factura.Descripcion == null)
            {
                return BadRequest(new { message = "La descripción es obligatoria." });
            }

            facturas.Add(factura);
            return CreatedAtAction(nameof(GetFactura), new { id = factura.Id }, factura);
        }

        // PUT: api/facturas/5
        [HttpPut("{id}")]
        public IActionResult PutFactura(int id, Factura factura)
        {
            if (factura.Descripcion == null)
            {
                return BadRequest(new { message = "La descripción es obligatoria." });
            }

            var existingFactura = facturas.FirstOrDefault(f => f.Id == id);

            if (existingFactura == null)
            {
                return NotFound();
            }

            existingFactura.Descripcion = factura.Descripcion;
            return NoContent();
        }

        // DELETE: api/facturas/5
        [HttpDelete("{id}")]
        public IActionResult DeleteFactura(int id)
        {
            var factura = facturas.FirstOrDefault(f => f.Id == id);

            if (factura == null)
            {
                return NotFound();
            }

            facturas.Remove(factura);
            return NoContent();
        }
    }
}
