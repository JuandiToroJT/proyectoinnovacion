using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProyectoInventariosWebApi.Models;

namespace ProyectoInventariosWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MovimientoInventarioController : ControllerBase
    {
        private readonly ProyectoInventariosDbContext _context;

        public MovimientoInventarioController(ProyectoInventariosDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<object>>> GetMovimientos(
            [FromQuery] int? idInventario = null,
            [FromQuery] int? idProducto = null,
            [FromQuery] int? idDependencia = null,
            [FromQuery] string? tipoMovimiento = null,
            [FromQuery] DateTime? fechaDesde = null,
            [FromQuery] DateTime? fechaHasta = null)
        {
            var query = _context.MovimientoInventario
                .Include(m => m.IdInventarioNavigation)
                    .ThenInclude(i => i.IdProductoNavigation)
                .Include(m => m.IdInventarioNavigation)
                    .ThenInclude(i => i.IdDependenciaNavigation)
                        .ThenInclude(d => d.IdSedeNavigation)
                .Include(m => m.IdUsuarioNavigation)
                .AsQueryable();

            if (idInventario.HasValue)
            {
                query = query.Where(m => m.IdInventario == idInventario.Value);
            }

            if (idProducto.HasValue)
            {
                query = query.Where(m => m.IdInventarioNavigation.IdProducto == idProducto.Value);
            }

            if (idDependencia.HasValue)
            {
                query = query.Where(m => m.IdInventarioNavigation.IdDependencia == idDependencia.Value);
            }

            if (!string.IsNullOrEmpty(tipoMovimiento))
            {
                query = query.Where(m => m.TipoMovimiento == tipoMovimiento);
            }

            if (fechaDesde.HasValue)
            {
                query = query.Where(m => m.Fecha >= fechaDesde.Value);
            }

            if (fechaHasta.HasValue)
            {
                query = query.Where(m => m.Fecha <= fechaHasta.Value);
            }

            var movimientos = await query
                .OrderByDescending(m => m.Fecha)
                .Select(m => new
                {
                    m.IdMovimiento,
                    Sede = m.IdInventarioNavigation.IdDependenciaNavigation.IdSedeNavigation.Nombre,
                    Dependencia = m.IdInventarioNavigation.IdDependenciaNavigation.Nombre,
                    Producto = new
                    {
                        m.IdInventarioNavigation.IdProducto,
                        m.IdInventarioNavigation.IdProductoNavigation.Codigo,
                        m.IdInventarioNavigation.IdProductoNavigation.Nombre
                    },
                    m.TipoMovimiento,
                    m.Cantidad,
                    m.StockAnterior,
                    m.StockNuevo,
                    m.Fecha,
                    Usuario = m.IdUsuarioNavigation.Nombre,
                    m.TipoReferencia,
                    m.IdReferencia,
                    m.Observaciones,
                    m.CostoUnitario,
                    ValorMovimiento = m.Cantidad * (m.CostoUnitario ?? 0)
                })
                .Take(1000)
                .ToListAsync();

            return Ok(movimientos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<object>> GetMovimiento(int id)
        {
            var movimiento = await _context.MovimientoInventario
                .Include(m => m.IdInventarioNavigation)
                    .ThenInclude(i => i.IdProductoNavigation)
                .Include(m => m.IdInventarioNavigation)
                    .ThenInclude(i => i.IdDependenciaNavigation)
                        .ThenInclude(d => d.IdSedeNavigation)
                .Include(m => m.IdUsuarioNavigation)
                .Where(m => m.IdMovimiento == id)
                .Select(m => new
                {
                    m.IdMovimiento,
                    Inventario = new
                    {
                        m.IdInventario,
                        Sede = m.IdInventarioNavigation.IdDependenciaNavigation.IdSedeNavigation.Nombre,
                        Dependencia = m.IdInventarioNavigation.IdDependenciaNavigation.Nombre,
                        Producto = new
                        {
                            m.IdInventarioNavigation.IdProducto,
                            m.IdInventarioNavigation.IdProductoNavigation.Codigo,
                            m.IdInventarioNavigation.IdProductoNavigation.Nombre,
                            m.IdInventarioNavigation.IdProductoNavigation.Precio
                        }
                    },
                    m.TipoMovimiento,
                    m.Cantidad,
                    m.StockAnterior,
                    m.StockNuevo,
                    m.Fecha,
                    Usuario = new
                    {
                        m.IdUsuario,
                        m.IdUsuarioNavigation.Nombre,
                        m.IdUsuarioNavigation.Correo,
                        m.IdUsuarioNavigation.Rol
                    },
                    m.TipoReferencia,
                    m.IdReferencia,
                    m.Observaciones,
                    m.CostoUnitario,
                    ValorMovimiento = m.Cantidad * (m.CostoUnitario ?? 0)
                })
                .FirstOrDefaultAsync();

            if (movimiento == null)
            {
                return NotFound(new { message = "Movimiento no encontrado" });
            }

            return Ok(movimiento);
        }

        [HttpGet("Producto/{idProducto}/Historial")]
        public async Task<ActionResult> GetHistorialProducto(int idProducto, [FromQuery] int? idDependencia = null)
        {
            var producto = await _context.Productos.FindAsync(idProducto);
            if (producto == null)
            {
                return NotFound(new { message = "Producto no encontrado" });
            }

            var query = _context.MovimientoInventario
                .Include(m => m.IdInventarioNavigation)
                    .ThenInclude(i => i.IdDependenciaNavigation)
                        .ThenInclude(d => d.IdSedeNavigation)
                .Include(m => m.IdUsuarioNavigation)
                .Where(m => m.IdInventarioNavigation.IdProducto == idProducto);

            if (idDependencia.HasValue)
            {
                query = query.Where(m => m.IdInventarioNavigation.IdDependencia == idDependencia.Value);
            }

            var historial = await query
                .OrderByDescending(m => m.Fecha)
                .Select(m => new
                {
                    m.IdMovimiento,
                    Sede = m.IdInventarioNavigation.IdDependenciaNavigation.IdSedeNavigation.Nombre,
                    Dependencia = m.IdInventarioNavigation.IdDependenciaNavigation.Nombre,
                    m.TipoMovimiento,
                    m.Cantidad,
                    m.StockAnterior,
                    m.StockNuevo,
                    m.Fecha,
                    Usuario = m.IdUsuarioNavigation.Nombre,
                    m.Observaciones
                })
                .ToListAsync();

            return Ok(new
            {
                Producto = new
                {
                    producto.IdProducto,
                    producto.Codigo,
                    producto.Nombre
                },
                TotalMovimientos = historial.Count,
                Historial = historial
            });
        }

        [HttpGet("Reporte/Diario")]
        public async Task<ActionResult> GetReporteDiario([FromQuery] DateTime? fecha = null)
        {
            var fechaBusqueda = fecha ?? DateTime.Today;
            var fechaInicio = fechaBusqueda.Date;
            var fechaFin = fechaInicio.AddDays(1);

            var movimientos = await _context.MovimientoInventario
                .Include(m => m.IdInventarioNavigation)
                    .ThenInclude(i => i.IdProductoNavigation)
                .Include(m => m.IdInventarioNavigation)
                    .ThenInclude(i => i.IdDependenciaNavigation)
                        .ThenInclude(d => d.IdSedeNavigation)
                .Where(m => m.Fecha >= fechaInicio && m.Fecha < fechaFin)
                .ToListAsync();

            var resumen = new
            {
                Fecha = fechaBusqueda.ToString("yyyy-MM-dd"),
                TotalMovimientos = movimientos.Count,
                Entradas = movimientos.Count(m => m.Cantidad > 0),
                Salidas = movimientos.Count(m => m.Cantidad < 0),
                PorTipo = movimientos.GroupBy(m => m.TipoMovimiento)
                    .Select(g => new
                    {
                        Tipo = g.Key,
                        Cantidad = g.Count(),
                        TotalUnidades = g.Sum(m => Math.Abs(m.Cantidad))
                    })
                    .OrderByDescending(x => x.Cantidad)
                    .ToList(),
                PorSede = movimientos.GroupBy(m => m.IdInventarioNavigation.IdDependenciaNavigation.IdSedeNavigation.Nombre)
                    .Select(g => new
                    {
                        Sede = g.Key,
                        Movimientos = g.Count()
                    })
                    .OrderByDescending(x => x.Movimientos)
                    .ToList(),
                Movimientos = movimientos
                    .OrderByDescending(m => m.Fecha)
                    .Select(m => new
                    {
                        m.IdMovimiento,
                        Sede = m.IdInventarioNavigation.IdDependenciaNavigation.IdSedeNavigation.Nombre,
                        Dependencia = m.IdInventarioNavigation.IdDependenciaNavigation.Nombre,
                        Producto = m.IdInventarioNavigation.IdProductoNavigation.Nombre,
                        m.TipoMovimiento,
                        m.Cantidad,
                        m.Fecha,
                        m.Observaciones
                    })
                    .ToList()
            };

            return Ok(resumen);
        }

        [HttpGet("Reporte/Rango")]
        public async Task<ActionResult> GetReporteRango(
            [FromQuery] DateTime fechaDesde,
            [FromQuery] DateTime fechaHasta)
        {
            if (fechaDesde > fechaHasta)
            {
                return BadRequest(new { message = "La fecha desde debe ser menor o igual a la fecha hasta" });
            }

            var movimientos = await _context.MovimientoInventario
                .Include(m => m.IdInventarioNavigation)
                    .ThenInclude(i => i.IdProductoNavigation)
                .Include(m => m.IdInventarioNavigation)
                    .ThenInclude(i => i.IdDependenciaNavigation)
                        .ThenInclude(d => d.IdSedeNavigation)
                .Where(m => m.Fecha >= fechaDesde && m.Fecha <= fechaHasta)
                .ToListAsync();

            var resumen = new
            {
                FechaDesde = fechaDesde.ToString("yyyy-MM-dd"),
                FechaHasta = fechaHasta.ToString("yyyy-MM-dd"),
                TotalMovimientos = movimientos.Count,
                TotalEntradas = movimientos.Where(m => m.Cantidad > 0).Sum(m => m.Cantidad),
                TotalSalidas = Math.Abs(movimientos.Where(m => m.Cantidad < 0).Sum(m => m.Cantidad)),
                ValorTotal = movimientos.Sum(m => m.Cantidad * (m.CostoUnitario ?? 0)),
                PorTipo = movimientos.GroupBy(m => m.TipoMovimiento)
                    .Select(g => new
                    {
                        Tipo = g.Key,
                        Cantidad = g.Count(),
                        TotalUnidades = g.Sum(m => Math.Abs(m.Cantidad)),
                        Valor = g.Sum(m => m.Cantidad * (m.CostoUnitario ?? 0))
                    })
                    .OrderByDescending(x => x.Cantidad)
                    .ToList(),
                ProductosMasMovidos = movimientos
                    .GroupBy(m => new
                    {
                        m.IdInventarioNavigation.IdProducto,
                        m.IdInventarioNavigation.IdProductoNavigation.Nombre
                    })
                    .Select(g => new
                    {
                        IdProducto = g.Key.IdProducto,
                        Producto = g.Key.Nombre,
                        TotalMovimientos = g.Count(),
                        TotalUnidades = g.Sum(m => Math.Abs(m.Cantidad))
                    })
                    .OrderByDescending(x => x.TotalMovimientos)
                    .Take(10)
                    .ToList()
            };

            return Ok(resumen);
        }

        [HttpGet("TiposMovimiento")]
        public async Task<ActionResult<IEnumerable<string>>> GetTiposMovimiento()
        {
            var tipos = await _context.MovimientoInventario
                .Select(m => m.TipoMovimiento)
                .Distinct()
                .OrderBy(t => t)
                .ToListAsync();

            return Ok(tipos);
        }
    }
}