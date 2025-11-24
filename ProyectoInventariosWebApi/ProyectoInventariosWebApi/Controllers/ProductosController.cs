using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProyectoInventariosWebApi.Models;

namespace ProyectoInventariosWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductosController : ControllerBase
    {
        private readonly ProyectoInventariosDbContext _context;

        public ProductosController(ProyectoInventariosDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<object>>> GetProductos(
            [FromQuery] string? categoria = null,
            [FromQuery] bool? esCompartible = null,
            [FromQuery] bool? soloActivos = true)
        {
            var query = _context.Productos.AsQueryable();

            if (!string.IsNullOrEmpty(categoria))
            {
                query = query.Where(p => p.Categoria == categoria);
            }

            if (esCompartible.HasValue)
            {
                query = query.Where(p => p.EsCompartible == esCompartible.Value);
            }

            if (soloActivos == true)
            {
                query = query.Where(p => p.Estado);
            }

            var productos = await query
                .Select(p => new
                {
                    p.IdProducto,
                    p.Codigo,
                    p.Nombre,
                    p.Descripcion,
                    p.Precio,
                    p.UnidadMedida,
                    p.Categoria,
                    p.EsCompartible,
                    p.StockMinimoGlobal,
                    p.Imagen,
                    p.RequiereRefrigeracion,
                    p.Estado,
                    StockTotal = _context.InventarioDependencia
                        .Where(i => i.IdProducto == p.IdProducto)
                        .Sum(i => (int?)i.StockActual) ?? 0,
                    UbicacionesDisponibles = _context.InventarioDependencia
                        .Count(i => i.IdProducto == p.IdProducto && i.StockActual > 0),
                    StockBajo = _context.InventarioDependencia
                        .Any(i => i.IdProducto == p.IdProducto && i.StockActual <= i.StockMinimo)
                })
                .OrderBy(p => p.Nombre)
                .ToListAsync();

            return Ok(productos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<object>> GetProducto(int id)
        {
            var producto = await _context.Productos
                .Where(p => p.IdProducto == id)
                .Select(p => new
                {
                    p.IdProducto,
                    p.Codigo,
                    p.Nombre,
                    p.Descripcion,
                    p.Precio,
                    p.Stock,
                    p.UnidadMedida,
                    p.Categoria,
                    p.EsCompartible,
                    p.StockMinimoGlobal,
                    p.Imagen,
                    p.RequiereRefrigeracion,
                    p.DiasVidaUtil,
                    p.Estado,
                    p.FechaCreacion,
                    StockTotal = _context.InventarioDependencia
                        .Where(i => i.IdProducto == id)
                        .Sum(i => (int?)i.StockActual) ?? 0,
                    Inventario = _context.InventarioDependencia
                        .Include(i => i.IdDependenciaNavigation)
                            .ThenInclude(d => d.IdSedeNavigation)
                        .Where(i => i.IdProducto == id)
                        .Select(i => new
                        {
                            i.IdInventario,
                            Sede = i.IdDependenciaNavigation.IdSedeNavigation.Nombre,
                            Dependencia = i.IdDependenciaNavigation.Nombre,
                            TipoDependencia = i.IdDependenciaNavigation.TipoDependencia,
                            i.StockActual,
                            i.StockMinimo,
                            i.EstadoInventario,
                            i.Ubicacion,
                            StockBajo = i.StockActual <= i.StockMinimo
                        })
                        .OrderBy(i => i.Sede)
                        .ThenBy(i => i.Dependencia)
                        .ToList()
                })
                .FirstOrDefaultAsync();

            if (producto == null)
            {
                return NotFound(new { message = "Producto no encontrado" });
            }

            return Ok(producto);
        }

        [HttpGet("{id}/Disponibilidad")]
        public async Task<ActionResult<object>> GetDisponibilidad(
            int id,
            [FromQuery] int? idSede = null,
            [FromQuery] int? idDependencia = null,
            [FromQuery] int cantidadRequerida = 1)
        {
            var producto = await _context.Productos.FindAsync(id);
            if (producto == null)
            {
                return NotFound(new { message = "Producto no encontrado" });
            }

            var query = _context.InventarioDependencia
                .Include(i => i.IdDependenciaNavigation)
                    .ThenInclude(d => d.IdSedeNavigation)
                .Where(i => i.IdProducto == id && i.StockActual > 0);

            if (idSede.HasValue)
            {
                query = query.Where(i => i.IdDependenciaNavigation.IdSede == idSede.Value);
            }

            if (idDependencia.HasValue)
            {
                query = query.Where(i => i.IdDependencia == idDependencia.Value);
            }

            var ubicaciones = await query
                .Select(i => new
                {
                    i.IdInventario,
                    i.IdDependencia,
                    Sede = i.IdDependenciaNavigation.IdSedeNavigation.Nombre,
                    Dependencia = i.IdDependenciaNavigation.Nombre,
                    i.StockActual,
                    Disponible = i.StockActual >= cantidadRequerida,
                    CantidadFaltante = cantidadRequerida > i.StockActual
                        ? cantidadRequerida - i.StockActual
                        : 0
                })
                .OrderByDescending(i => i.StockActual)
                .ToListAsync();

            var stockTotal = ubicaciones.Sum(u => u.StockActual);
            var hayDisponibilidad = stockTotal >= cantidadRequerida;

            return Ok(new
            {
                Producto = new
                {
                    producto.IdProducto,
                    producto.Nombre,
                    producto.EsCompartible
                },
                CantidadRequerida = cantidadRequerida,
                StockTotal = stockTotal,
                HayDisponibilidad = hayDisponibilidad,
                UbicacionesDisponibles = ubicaciones.Count,
                Ubicaciones = ubicaciones,
                Sugerencia = !hayDisponibilidad && producto.EsCompartible
                    ? "El producto puede ser transferido desde otras ubicaciones"
                    : hayDisponibilidad
                    ? "Stock disponible"
                    : "Stock insuficiente y producto no es compartible"
            });
        }

        [HttpPost]
        public async Task<ActionResult<Productos>> PostProducto(Productos producto)
        {
            if (!string.IsNullOrEmpty(producto.Codigo))
            {
                var existe = await _context.Productos.AnyAsync(p => p.Codigo == producto.Codigo);
                if (existe)
                {
                    return BadRequest(new { message = "Ya existe un producto con ese código" });
                }
            }
            else
            {
                var ultimoId = await _context.Productos.MaxAsync(p => (int?)p.IdProducto) ?? 0;
                producto.Codigo = $"PROD-{(ultimoId + 1):D4}";
            }

            producto.Estado = true;
            producto.FechaCreacion = DateTime.Now;
            producto.EsCompartible = producto.EsCompartible;

            _context.Productos.Add(producto);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetProducto), new { id = producto.IdProducto }, producto);
        }

        [HttpPost("{id}/AgregarInventario")]
        public async Task<ActionResult> AgregarInventario(int id, [FromBody] AgregarInventarioDto dto)
        {
            var producto = await _context.Productos.FindAsync(id);
            if (producto == null)
            {
                return NotFound(new { message = "Producto no encontrado" });
            }

            var dependencia = await _context.Dependencias.FindAsync(dto.IdDependencia);
            if (dependencia == null)
            {
                return BadRequest(new { message = "Dependencia no encontrada" });
            }

            var inventarioExistente = await _context.InventarioDependencia
                .FirstOrDefaultAsync(i => i.IdProducto == id && i.IdDependencia == dto.IdDependencia);

            if (inventarioExistente != null)
            {
                return BadRequest(new
                {
                    message = "Este producto ya tiene inventario en esa dependencia",
                    idInventario = inventarioExistente.IdInventario
                });
            }

            var inventario = new InventarioDependencia
            {
                IdProducto = id,
                IdDependencia = dto.IdDependencia,
                StockActual = dto.StockInicial,
                StockMinimo = dto.StockMinimo ?? 10,
                StockMaximo = dto.StockMaximo ?? 1000,
                PuntoReorden = dto.PuntoReorden ?? 20,
                CostoPromedio = dto.CostoPromedio ?? (producto.Precio * 0.7m),
                Ubicacion = dto.Ubicacion,
                UltimaActualizacion = DateTime.Now,
                EstadoInventario = dto.StockInicial > 0 ? "Disponible" : "Agotado"
            };

            _context.InventarioDependencia.Add(inventario);
            await _context.SaveChangesAsync();

            var movimiento = new MovimientoInventario
            {
                IdInventario = inventario.IdInventario,
                TipoMovimiento = "Entrada",
                Cantidad = dto.StockInicial,
                StockAnterior = 0,
                StockNuevo = dto.StockInicial,
                Fecha = DateTime.Now,
                IdUsuario = dto.IdUsuario ?? 1,
                TipoReferencia = "Creación",
                Observaciones = "Inventario inicial en dependencia",
                CostoUnitario = inventario.CostoPromedio
            };

            _context.MovimientoInventario.Add(movimiento);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                message = "Inventario agregado exitosamente",
                idInventario = inventario.IdInventario,
                stockInicial = dto.StockInicial
            });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutProducto(int id, Productos producto)
        {
            if (id != producto.IdProducto)
            {
                return BadRequest(new { message = "El ID no coincide" });
            }

            if (!string.IsNullOrEmpty(producto.Codigo))
            {
                var existe = await _context.Productos
                    .AnyAsync(p => p.Codigo == producto.Codigo && p.IdProducto != id);

                if (existe)
                {
                    return BadRequest(new { message = "Ya existe otro producto con ese código" });
                }
            }

            _context.Entry(producto).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductoExists(id))
                {
                    return NotFound(new { message = "Producto no encontrado" });
                }
                else
                {
                    throw;
                }
            }

            return Ok(new { message = "Producto actualizado exitosamente" });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProducto(int id)
        {
            var producto = await _context.Productos.FindAsync(id);
            if (producto == null)
            {
                return NotFound(new { message = "Producto no encontrado" });
            }

            var tieneInventario = await _context.InventarioDependencia
                .AnyAsync(i => i.IdProducto == id && i.StockActual > 0);

            if (tieneInventario)
            {
                return BadRequest(new
                {
                    message = "No se puede desactivar un producto con inventario activo",
                    sugerencia = "Realice transferencias o ajustes para llevar el stock a cero"
                });
            }

            producto.Estado = false;
            await _context.SaveChangesAsync();

            return Ok(new { message = "Producto desactivado exitosamente" });
        }

        [HttpGet("Categorias")]
        public async Task<ActionResult<IEnumerable<string>>> GetCategorias()
        {
            var categorias = await _context.Productos
                .Where(p => p.Estado && !string.IsNullOrEmpty(p.Categoria))
                .Select(p => p.Categoria)
                .Distinct()
                .OrderBy(c => c)
                .ToListAsync();

            return Ok(categorias);
        }

        [HttpGet("StockBajo")]
        public async Task<ActionResult> GetProductosStockBajo()
        {
            var productosStockBajo = await _context.InventarioDependencia
                .Include(i => i.IdProductoNavigation)
                .Include(i => i.IdDependenciaNavigation)
                    .ThenInclude(d => d.IdSedeNavigation)
                .Where(i => i.StockActual <= i.StockMinimo)
                .Select(i => new
                {
                    Producto = new
                    {
                        i.IdProducto,
                        i.IdProductoNavigation.Codigo,
                        i.IdProductoNavigation.Nombre,
                        i.IdProductoNavigation.EsCompartible
                    },
                    Sede = i.IdDependenciaNavigation.IdSedeNavigation.Nombre,
                    Dependencia = i.IdDependenciaNavigation.Nombre,
                    i.StockActual,
                    i.StockMinimo,
                    i.PuntoReorden,
                    Diferencia = i.StockMinimo - i.StockActual,
                    RequiereReorden = i.StockActual <= i.PuntoReorden,
                    Prioridad = i.StockActual <= i.PuntoReorden ? "Alta" : "Media"
                })
                .OrderBy(i => i.StockActual)
                .ToListAsync();

            return Ok(new
            {
                TotalProductosAfectados = productosStockBajo.Select(p => p.Producto.IdProducto).Distinct().Count(),
                TotalUbicaciones = productosStockBajo.Count,
                ProductosStockBajo = productosStockBajo
            });
        }

        private bool ProductoExists(int id)
        {
            return _context.Productos.Any(e => e.IdProducto == id);
        }
    }

    public class AgregarInventarioDto
    {
        public int IdDependencia { get; set; }
        public int StockInicial { get; set; }
        public int? StockMinimo { get; set; }
        public int? StockMaximo { get; set; }
        public int? PuntoReorden { get; set; }
        public decimal? CostoPromedio { get; set; }
        public string? Ubicacion { get; set; }
        public int? IdUsuario { get; set; }
    }
}