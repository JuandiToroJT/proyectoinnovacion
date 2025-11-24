using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProyectoInventariosWebApi.Models;

namespace ProyectoInventariosWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DependenciasController : ControllerBase
    {
        private readonly ProyectoInventariosDbContext _context;

        public DependenciasController(ProyectoInventariosDbContext context)
        {
            _context = context;
        }

        // GET: api/Dependencias
        /// <summary>
        /// Obtiene todas las dependencias
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Dependencias>>> GetDependencias([FromQuery] string? tipo = null)
        {
            var query = _context.Dependencias
                .Include(d => d.IdSedeNavigation)
                .Where(d => d.Estado);

            // Filtrar por tipo si se proporciona
            if (!string.IsNullOrEmpty(tipo))
            {
                query = query.Where(d => d.TipoDependencia == tipo);
            }

            var dependencias = await query
                .OrderBy(d => d.IdSedeNavigation.Nombre)
                .ThenBy(d => d.Nombre)
                .ToListAsync();

            return dependencias;
        }

        // GET: api/Dependencias/5
        /// <summary>
        /// Obtiene una dependencia por ID
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<Dependencias>> GetDependencia(int id)
        {
            var dependencia = await _context.Dependencias
                .Include(d => d.IdSedeNavigation)
                .FirstOrDefaultAsync(d => d.IdDependencia == id);

            if (dependencia == null)
            {
                return NotFound(new { message = "Dependencia no encontrada" });
            }

            return dependencia;
        }

        // GET: api/Dependencias/5/Inventario
        /// <summary>
        /// Obtiene el inventario de una dependencia específica
        /// </summary>
        [HttpGet("{id}/Inventario")]
        public async Task<ActionResult> GetInventarioDependencia(int id, [FromQuery] string? estado = null)
        {
            var dependencia = await _context.Dependencias
                .Include(d => d.IdSedeNavigation)
                .FirstOrDefaultAsync(d => d.IdDependencia == id);

            if (dependencia == null)
            {
                return NotFound(new { message = "Dependencia no encontrada" });
            }

            var query = _context.InventarioDependencia
                .Include(i => i.IdProductoNavigation)
                .Where(i => i.IdDependencia == id);

            // Filtrar por estado si se proporciona
            if (!string.IsNullOrEmpty(estado))
            {
                query = query.Where(i => i.EstadoInventario == estado);
            }

            var inventario = await query
                .Select(i => new
                {
                    i.IdInventario,
                    i.IdProducto,
                    Producto = new
                    {
                        i.IdProductoNavigation.Codigo,
                        i.IdProductoNavigation.Nombre,
                        i.IdProductoNavigation.Descripcion,
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
                    StockBajo = i.StockActual <= i.StockMinimo
                })
                .OrderBy(i => i.Producto.Nombre)
                .ToListAsync();

            return Ok(new
            {
                Dependencia = new
                {
                    dependencia.IdDependencia,
                    dependencia.Nombre,
                    dependencia.TipoDependencia,
                    Sede = dependencia.IdSedeNavigation.Nombre
                },
                TotalProductos = inventario.Count,
                StockTotal = inventario.Sum(i => i.StockActual),
                ProductosStockBajo = inventario.Count(i => i.StockBajo),
                Inventario = inventario
            });
        }

        // GET: api/Dependencias/5/StockBajo
        /// <summary>
        /// Obtiene productos con stock bajo en una dependencia
        /// </summary>
        [HttpGet("{id}/StockBajo")]
        public async Task<ActionResult> GetProductosStockBajo(int id)
        {
            var dependencia = await _context.Dependencias.FindAsync(id);
            if (dependencia == null)
            {
                return NotFound(new { message = "Dependencia no encontrada" });
            }

            var productosStockBajo = await _context.InventarioDependencia
                .Include(i => i.IdProductoNavigation)
                .Where(i => i.IdDependencia == id && i.StockActual <= i.StockMinimo)
                .Select(i => new
                {
                    i.IdProducto,
                    i.IdProductoNavigation.Nombre,
                    i.StockActual,
                    i.StockMinimo,
                    i.PuntoReorden,
                    Diferencia = i.StockMinimo - i.StockActual,
                    RequiereReorden = i.StockActual <= i.PuntoReorden
                })
                .OrderBy(i => i.StockActual)
                .ToListAsync();

            return Ok(productosStockBajo);
        }

        // GET: api/Dependencias/Tipos
        /// <summary>
        /// Obtiene los tipos de dependencias disponibles
        /// </summary>
        [HttpGet("Tipos")]
        public async Task<ActionResult<IEnumerable<string>>> GetTiposDependencias()
        {
            var tipos = await _context.Dependencias
                .Where(d => d.Estado)
                .Select(d => d.TipoDependencia)
                .Distinct()
                .OrderBy(t => t)
                .ToListAsync();

            return tipos;
        }

        // PUT: api/Dependencias/5
        /// <summary>
        /// Actualiza una dependencia
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDependencia(int id, Dependencias dependencia)
        {
            if (id != dependencia.IdDependencia)
            {
                return BadRequest(new { message = "El ID no coincide" });
            }

            // Verificar que la sede existe
            var sede = await _context.Sedes.FindAsync(dependencia.IdSede);
            if (sede == null)
            {
                return BadRequest(new { message = "La sede especificada no existe" });
            }

            _context.Entry(dependencia).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DependenciaExists(id))
                {
                    return NotFound(new { message = "Dependencia no encontrada" });
                }
                else
                {
                    throw;
                }
            }

            return Ok(new { message = "Dependencia actualizada exitosamente" });
        }

        // POST: api/Dependencias
        /// <summary>
        /// Crea una nueva dependencia
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<Dependencias>> PostDependencia(Dependencias dependencia)
        {
            // Verificar que la sede existe y está activa
            var sede = await _context.Sedes.FindAsync(dependencia.IdSede);
            if (sede == null)
            {
                return BadRequest(new { message = "La sede especificada no existe" });
            }

            if (!sede.Estado)
            {
                return BadRequest(new { message = "La sede está inactiva" });
            }

            dependencia.FechaCreacion = DateTime.Now;
            dependencia.Estado = true;

            _context.Dependencias.Add(dependencia);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetDependencia), new { id = dependencia.IdDependencia }, dependencia);
        }

        // DELETE: api/Dependencias/5
        /// <summary>
        /// Desactiva una dependencia (soft delete)
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDependencia(int id)
        {
            var dependencia = await _context.Dependencias.FindAsync(id);
            if (dependencia == null)
            {
                return NotFound(new { message = "Dependencia no encontrada" });
            }

            // Verificar si hay inventario activo
            var tieneInventario = await _context.InventarioDependencia
                .AnyAsync(i => i.IdDependencia == id && i.StockActual > 0);

            if (tieneInventario)
            {
                return BadRequest(new { message = "No se puede desactivar una dependencia con inventario activo" });
            }

            // Soft delete
            dependencia.Estado = false;
            await _context.SaveChangesAsync();

            return Ok(new { message = "Dependencia desactivada exitosamente" });
        }

        private bool DependenciaExists(int id)
        {
            return _context.Dependencias.Any(e => e.IdDependencia == id);
        }
    }
}