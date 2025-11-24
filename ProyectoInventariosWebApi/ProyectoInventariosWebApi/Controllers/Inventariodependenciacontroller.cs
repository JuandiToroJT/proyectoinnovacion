using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProyectoInventariosWebApi.Models;

namespace ProyectoInventariosWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InventarioDependenciaController : ControllerBase
    {
        private readonly ProyectoInventariosDbContext _context;

        public InventarioDependenciaController(ProyectoInventariosDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<object>>> GetInventario(
            [FromQuery] int? idSede = null,
            [FromQuery] int? idDependencia = null,
            [FromQuery] int? idProducto = null,
            [FromQuery] string? estadoInventario = null)
        {
            var query = _context.InventarioDependencia
                .Include(i => i.IdProductoNavigation)
                .Include(i => i.IdDependenciaNavigation)
                    .ThenInclude(d => d.IdSedeNavigation)
                .AsQueryable();

            if (idSede.HasValue)
            {
                query = query.Where(i => i.IdDependenciaNavigation.IdSede == idSede.Value);
            }

            if (idDependencia.HasValue)
            {
                query = query.Where(i => i.IdDependencia == idDependencia.Value);
            }

            if (idProducto.HasValue)
            {
                query = query.Where(i => i.IdProducto == idProducto.Value);
            }

            if (!string.IsNullOrEmpty(estadoInventario))
            {
                query = query.Where(i => i.EstadoInventario == estadoInventario);
            }

            var inventario = await query
                .Select(i => new
                {
                    i.IdInventario,
                    Sede = i.IdDependenciaNavigation.IdSedeNavigation.Nombre,
                    Dependencia = i.IdDependenciaNavigation.Nombre,
                    TipoDependencia = i.IdDependenciaNavigation.TipoDependencia,
                    Producto = new
                    {
                        i.IdProducto,
                        i.IdProductoNavigation.Codigo,
                        i.IdProductoNavigation.Nombre,
                        i.IdProductoNavigation.Precio,
                        i.IdProductoNavigation.UnidadMedida,
                        i.IdProductoNavigation.Categoria
                    },
                    i.StockActual,
                    i.StockMinimo,
                    i.StockMaximo,
                    i.PuntoReorden,
                    i.CostoPromedio,
                    i.Ubicacion,
                    i.EstadoInventario,
                    i.UltimaActualizacion,
                    AlertaStockBajo = i.StockActual <= i.StockMinimo,
                    RequiereReorden = i.StockActual <= i.PuntoReorden
                })
                .OrderBy(i => i.Sede)
                .ThenBy(i => i.Dependencia)
                .ThenBy(i => i.Producto.Nombre)
                .ToListAsync();

            return Ok(inventario);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<object>> GetInventarioById(int id)
        {
            var inventario = await _context.InventarioDependencia
                .Include(i => i.IdProductoNavigation)
                .Include(i => i.IdDependenciaNavigation)
                    .ThenInclude(d => d.IdSedeNavigation)
                .Where(i => i.IdInventario == id)
                .Select(i => new
                {
                    i.IdInventario,
                    Sede = new
                    {
                        i.IdDependenciaNavigation.IdSedeNavigation.IdSede,
                        i.IdDependenciaNavigation.IdSedeNavigation.Nombre,
                        i.IdDependenciaNavigation.IdSedeNavigation.Codigo
                    },
                    Dependencia = new
                    {
                        i.IdDependencia,
                        i.IdDependenciaNavigation.Nombre,
                        i.IdDependenciaNavigation.TipoDependencia,
                        i.IdDependenciaNavigation.Responsable
                    },
                    Producto = new
                    {
                        i.IdProducto,
                        i.IdProductoNavigation.Codigo,
                        i.IdProductoNavigation.Nombre,
                        i.IdProductoNavigation.Descripcion,
                        i.IdProductoNavigation.Precio,
                        i.IdProductoNavigation.UnidadMedida,
                        i.IdProductoNavigation.Categoria,
                        i.IdProductoNavigation.EsCompartible
                    },
                    i.StockActual,
                    i.StockMinimo,
                    i.StockMaximo,
                    i.PuntoReorden,
                    i.CostoPromedio,
                    i.Ubicacion,
                    i.EstadoInventario,
                    i.UltimaActualizacion,
                    ValorInventario = i.StockActual * i.CostoPromedio
                })
                .FirstOrDefaultAsync();

            if (inventario == null)
            {
                return NotFound(new { message = "Registro de inventario no encontrado" });
            }

            return Ok(inventario);
        }

        [HttpGet("Producto/{idProducto}/Ubicaciones")]
        public async Task<ActionResult> GetUbicacionesProducto(int idProducto)
        {
            var producto = await _context.Productos.FindAsync(idProducto);
            if (producto == null)
            {
                return NotFound(new { message = "Producto no encontrado" });
            }

            var ubicaciones = await _context.InventarioDependencia
                .Include(i => i.IdDependenciaNavigation)
                    .ThenInclude(d => d.IdSedeNavigation)
                .Where(i => i.IdProducto == idProducto && i.StockActual > 0)
                .Select(i => new
                {
                    i.IdInventario,
                    Sede = i.IdDependenciaNavigation.IdSedeNavigation.Nombre,
                    Dependencia = i.IdDependenciaNavigation.Nombre,
                    TipoDependencia = i.IdDependenciaNavigation.TipoDependencia,
                    i.StockActual,
                    i.Ubicacion,
                    i.EstadoInventario
                })
                .OrderByDescending(i => i.StockActual)
                .ToListAsync();

            return Ok(new
            {
                Producto = new
                {
                    producto.IdProducto,
                    producto.Codigo,
                    producto.Nombre,
                    producto.EsCompartible
                },
                StockTotal = ubicaciones.Sum(u => u.StockActual),
                TotalUbicaciones = ubicaciones.Count,
                Ubicaciones = ubicaciones
            });
        }


        [HttpPost]
        public async Task<ActionResult<InventarioDependencia>> PostInventario(InventarioDependencia inventario)
        {
            var producto = await _context.Productos.FindAsync(inventario.IdProducto);
            if (producto == null)
            {
                return BadRequest(new { message = "El producto no existe" });
            }

            var dependencia = await _context.Dependencias.FindAsync(inventario.IdDependencia);
            if (dependencia == null)
            {
                return BadRequest(new { message = "La dependencia no existe" });
            }

            var existe = await _context.InventarioDependencia
                .AnyAsync(i => i.IdProducto == inventario.IdProducto &&
                              i.IdDependencia == inventario.IdDependencia);

            if (existe)
            {
                return BadRequest(new { message = "Este producto ya está registrado en esta dependencia" });
            }

            inventario.UltimaActualizacion = DateTime.Now;
            inventario.EstadoInventario = inventario.StockActual > 0 ? "Disponible" : "Agotado";

            _context.InventarioDependencia.Add(inventario);
            await _context.SaveChangesAsync();

            var movimiento = new MovimientoInventario
            {
                IdInventario = inventario.IdInventario,
                TipoMovimiento = "Entrada",
                Cantidad = inventario.StockActual,
                StockAnterior = 0,
                StockNuevo = inventario.StockActual,
                Fecha = DateTime.Now,
                IdUsuario = 1,
                TipoReferencia = "Creación",
                Observaciones = "Registro inicial de inventario",
                CostoUnitario = inventario.CostoPromedio
            };
            _context.MovimientoInventario.Add(movimiento);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetInventarioById), new { id = inventario.IdInventario }, inventario);
        }

        [HttpPut("{id}/AjustarStock")]
        public async Task<IActionResult> AjustarStock(int id, [FromBody] AjusteStockDto ajuste)
        {
            var inventario = await _context.InventarioDependencia.FindAsync(id);
            if (inventario == null)
            {
                return NotFound(new { message = "Registro de inventario no encontrado" });
            }

            var stockAnterior = inventario.StockActual;
            inventario.StockActual = ajuste.NuevoStock;
            inventario.UltimaActualizacion = DateTime.Now;
            inventario.EstadoInventario = ajuste.NuevoStock > 0 ? "Disponible" : "Agotado";

            var movimiento = new MovimientoInventario
            {
                IdInventario = id,
                TipoMovimiento = "Ajuste",
                Cantidad = ajuste.NuevoStock - stockAnterior,
                StockAnterior = stockAnterior,
                StockNuevo = ajuste.NuevoStock,
                Fecha = DateTime.Now,
                IdUsuario = 1,
                TipoReferencia = "Ajuste Manual",
                Observaciones = ajuste.Observaciones ?? "Ajuste manual de inventario"
            };

            _context.MovimientoInventario.Add(movimiento);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                message = "Stock ajustado exitosamente",
                stockAnterior,
                stockNuevo = ajuste.NuevoStock
            });
        }

        [HttpGet("Consolidado")]
        public async Task<ActionResult> GetInventarioConsolidado()
        {
            var consolidado = await _context.InventarioDependencia
                .Include(i => i.IdProductoNavigation)
                .GroupBy(i => new
                {
                    i.IdProducto,
                    i.IdProductoNavigation.Codigo,
                    i.IdProductoNavigation.Nombre,
                    i.IdProductoNavigation.Precio,
                    i.IdProductoNavigation.Categoria,
                    i.IdProductoNavigation.UnidadMedida
                })
                .Select(g => new
                {
                    IdProducto = g.Key.IdProducto,
                    Codigo = g.Key.Codigo,
                    Nombre = g.Key.Nombre,
                    Precio = g.Key.Precio,
                    Categoria = g.Key.Categoria,
                    UnidadMedida = g.Key.UnidadMedida,
                    StockTotal = g.Sum(i => i.StockActual),
                    CantidadUbicaciones = g.Count(),
                    ValorTotal = g.Sum(i => i.StockActual * i.CostoPromedio)
                })
                .OrderBy(p => p.Nombre)
                .ToListAsync();

            return Ok(new
            {
                TotalProductos = consolidado.Count,
                StockTotal = consolidado.Sum(c => c.StockTotal),
                ValorTotal = consolidado.Sum(c => c.ValorTotal),
                Productos = consolidado
            });
        }
    }

    public class AjusteStockDto
    {
        public int NuevoStock { get; set; }
        public string? Observaciones { get; set; }
    }
}