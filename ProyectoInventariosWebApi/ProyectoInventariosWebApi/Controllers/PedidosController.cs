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
    public class PedidosController : ControllerBase
    {
        private readonly ProyectoInventariosDbContext _context;

        public PedidosController(ProyectoInventariosDbContext context)
        {
            _context = context;
        }

        // GET: api/Pedidos?estado=Pendiente
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Pedidos>>> GetPedidosByEstado([FromQuery] string estado)
        {
            var pedidos = await _context.Pedidos
                .Include(p => p.IdClienteNavigation)
                .Include(p => p.IdUsuarioNavigation)
                .Include(p => p.DetallesPedido)
                .Include(p => p.Entregas)
                .Include(p => p.Facturas)
                .Where(p => p.Estado == estado)
                .ToListAsync();

            foreach (var p in pedidos)
            {
                if (p.IdClienteNavigation != null)
                {
                    p.IdClienteNavigation.Pedidos = null!;
                }

                if (p.IdUsuarioNavigation != null)
                {
                    p.IdUsuarioNavigation.Pedidos = null!;
                }

                if (p.DetallesPedido != null)
                {
                    foreach (var det in p.DetallesPedido)
                    {
                        det.IdPedidoNavigation = null!;
                    }
                }

                if (p.Entregas != null)
                {
                    foreach (var e in p.Entregas)
                    {
                        e.IdPedidoNavigation = null!;
                    }
                }

                if (p.Facturas != null)
                {
                    foreach (var f in p.Facturas)
                    {
                        f.IdPedidoNavigation = null!;
                    }
                }
            }

            return pedidos;
        }

        // GET: api/Pedidos/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Pedidos>> GetPedidos(int id)
        {
            var pedido = await _context.Pedidos
                .Include(p => p.IdClienteNavigation)
                .Include(p => p.IdUsuarioNavigation)
                .Include(p => p.DetallesPedido)
                    .ThenInclude(dp => dp.IdProductoNavigation)
                .Include(p => p.Entregas)
                .Include(p => p.Facturas)
                .FirstOrDefaultAsync(p => p.IdPedido == id);

            if (pedido == null)
            {
                return NotFound();
            }

            if (pedido.IdClienteNavigation != null)
            {
                pedido.IdClienteNavigation.Pedidos = null!;
            }

            if (pedido.IdUsuarioNavigation != null)
            {
                pedido.IdUsuarioNavigation.Pedidos = null!;
            }

            if (pedido.DetallesPedido != null)
            {
                foreach (var det in pedido.DetallesPedido)
                {
                    det.IdPedidoNavigation = null!;
                    if (det.IdProductoNavigation != null)
                    {
                        det.IdProductoNavigation.DetallesPedido = null!;
                    }
                }
            }

            if (pedido.Entregas != null)
            {
                foreach (var e in pedido.Entregas)
                {
                    e.IdPedidoNavigation = null!;
                }
            }

            if (pedido.Facturas != null)
            {
                foreach (var f in pedido.Facturas)
                {
                    f.IdPedidoNavigation = null!;
                }
            }

            return pedido;
        }

        // PUT: api/Pedidos/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPedidos(int id, Pedidos pedidos)
        {
            if (id != pedidos.IdPedido)
            {
                return BadRequest();
            }

            _context.Entry(pedidos).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PedidosExists(id))
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

        // POST: api/Pedidos
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Pedidos>> PostPedidos(Pedidos pedidos)
        {
            _context.Pedidos.Add(pedidos);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPedidos", new { id = pedidos.IdPedido }, pedidos);
        }

        [HttpGet("Entregas/{id}")]
        public async Task<ActionResult<Pedidos>> GetPedidoEntrega(int id)
        {
            var pedido = await _context.Pedidos
                .Include(p => p.IdClienteNavigation)
                .Include(p => p.Entregas)
                .FirstOrDefaultAsync(p => p.IdPedido == id);

            if (pedido == null)
            {
                return NotFound();
            }

            if (pedido.IdClienteNavigation != null)
            {
                pedido.IdClienteNavigation.Pedidos = null!;
            }

            if (pedido.Entregas != null)
            {
                foreach (var entrega in pedido.Entregas)
                {
                    entrega.IdPedidoNavigation = null!;
                }
            }

            return pedido;
        }

        [HttpPost("Entregas")]
        public async Task<ActionResult<Pedidos>> PostEntregas(Entregas entregas)
        {
            _context.Entregas.Add(entregas);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpGet("Facturas/Producto/{id}")]
        public async Task<ActionResult<IEnumerable<Facturas>>> GetFacturasPorProducto(int id)
        {
            var facturas = await _context.Facturas
                .Where(f => f.IdPedido == id)
                .ToListAsync();

            return facturas;
        }

        [HttpPost("Facturas")]
        public async Task<ActionResult<Facturas>> PostFacturas(Facturas facturas)
        {
            _context.Facturas.Add(facturas);
            await _context.SaveChangesAsync();

            return facturas;
        }

        private bool PedidosExists(int id)
        {
            return _context.Pedidos.Any(e => e.IdPedido == id);
        }
    }
}
