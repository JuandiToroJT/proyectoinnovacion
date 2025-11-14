using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProyectoInventariosWebApi.Models;

namespace ProyectoInventariosWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DetallesPedidosController : ControllerBase
    {
        private readonly ProyectoInventariosDbContext _context;

        public DetallesPedidosController(ProyectoInventariosDbContext context)
        {
            _context = context;
        }

        // GET: api/DetallesPedidos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DetallesPedido>>> GetDetallesPedido([FromQuery] int? idPedido = null)
        {
            List<DetallesPedido> detallesPedidos = new List<DetallesPedido>();
            if (idPedido.HasValue)
            {
                detallesPedidos = await _context.DetallesPedido
                    .Where(d => d.IdPedido == idPedido)
                    .ToListAsync();
            }
            else
                detallesPedidos = await _context.DetallesPedido.ToListAsync();

            return detallesPedidos;
        }

        // GET: api/DetallesPedidos/5
        [HttpGet("{id}")]
        public async Task<ActionResult<DetallesPedido>> GetDetallesPedido(int id)
        {
            var detallesPedido = await _context.DetallesPedido.FindAsync(id);

            if (detallesPedido == null)
            {
                return NotFound();
            }

            return detallesPedido;
        }

        // PUT: api/DetallesPedidos/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDetallesPedido(int id, DetallesPedido detallesPedido)
        {
            if (id != detallesPedido.IdDetalle)
            {
                return BadRequest();
            }

            _context.Entry(detallesPedido).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DetallesPedidoExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/DetallesPedidos
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<DetallesPedido>> PostDetallesPedido(DetallesPedido detallesPedido)
        {
            _context.DetallesPedido.Add(detallesPedido);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetDetallesPedido", new { id = detallesPedido.IdDetalle }, detallesPedido);
        }

        // DELETE: api/DetallesPedidos/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDetallesPedido(int id)
        {
            var detallesPedido = await _context.DetallesPedido.FindAsync(id);
            if (detallesPedido == null)
            {
                return NotFound();
            }

            _context.DetallesPedido.Remove(detallesPedido);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool DetallesPedidoExists(int id)
        {
            return _context.DetallesPedido.Any(e => e.IdDetalle == id);
        }
    }
}
