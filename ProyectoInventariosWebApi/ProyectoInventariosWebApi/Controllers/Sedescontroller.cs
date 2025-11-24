using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProyectoInventariosWebApi.Models;

namespace ProyectoInventariosWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SedesController : ControllerBase
    {
        private readonly ProyectoInventariosDbContext _context;

        public SedesController(ProyectoInventariosDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Sedes>>> GetSedes()
        {
            return await _context.Sedes
                .Include(s => s.IdEmpresaNavigation)
                .Include(s => s.Dependencias)
                .Where(s => s.Estado)
                .OrderBy(s => s.Nombre)
                .ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Sedes>> GetSede(int id)
        {
            var sede = await _context.Sedes
                .Include(s => s.IdEmpresaNavigation)
                .Include(s => s.Dependencias)
                .FirstOrDefaultAsync(s => s.IdSede == id);

            if (sede == null)
            {
                return NotFound(new { message = "Sede no encontrada" });
            }

            return sede;
        }

        [HttpGet("Codigo/{codigo}")]
        public async Task<ActionResult<Sedes>> GetSedeByCodigo(string codigo)
        {
            var sede = await _context.Sedes
                .Include(s => s.IdEmpresaNavigation)
                .Include(s => s.Dependencias)
                .FirstOrDefaultAsync(s => s.Codigo == codigo);

            if (sede == null)
            {
                return NotFound(new { message = "Sede no encontrada" });
            }

            return sede;
        }

        [HttpGet("{id}/Dependencias")]
        public async Task<ActionResult<IEnumerable<Dependencias>>> GetDependenciasDeSede(int id)
        {
            var sede = await _context.Sedes.FindAsync(id);
            if (sede == null)
            {
                return NotFound(new { message = "Sede no encontrada" });
            }

            var dependencias = await _context.Dependencias
                .Where(d => d.IdSede == id && d.Estado)
                .OrderBy(d => d.Nombre)
                .ToListAsync();

            return dependencias;
        }

        [HttpGet("{id}/Inventario")]
        public async Task<ActionResult> GetInventarioDeSede(int id)
        {
            var sede = await _context.Sedes.FindAsync(id);
            if (sede == null)
            {
                return NotFound(new { message = "Sede no encontrada" });
            }

            var inventario = await _context.InventarioDependencia
                .Include(i => i.IdProductoNavigation)
                .Include(i => i.IdDependenciaNavigation)
                .Where(i => i.IdDependenciaNavigation.IdSede == id)
                .GroupBy(i => new
                {
                    i.IdProducto,
                    i.IdProductoNavigation.Nombre,
                    i.IdProductoNavigation.Codigo,
                    i.IdProductoNavigation.Precio
                })
                .Select(g => new
                {
                    IdProducto = g.Key.IdProducto,
                    Codigo = g.Key.Codigo,
                    Nombre = g.Key.Nombre,
                    Precio = g.Key.Precio,
                    StockTotal = g.Sum(i => i.StockActual),
                    Dependencias = g.Select(i => new
                    {
                        i.IdDependenciaNavigation.Nombre,
                        i.StockActual,
                        i.EstadoInventario
                    }).ToList()
                })
                .ToListAsync();

            return Ok(new
            {
                Sede = sede.Nombre,
                TotalProductos = inventario.Count,
                StockTotal = inventario.Sum(i => i.StockTotal),
                Inventario = inventario
            });
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSede(int id, Sedes sede)
        {
            if (id != sede.IdSede)
            {
                return BadRequest(new { message = "El ID no coincide" });
            }

                       var sedeExistente = await _context.Sedes
                .FirstOrDefaultAsync(s => s.Codigo == sede.Codigo && s.IdSede != id);

            if (sedeExistente != null)
            {
                return BadRequest(new { message = "Ya existe una sede con ese código" });
            }

            _context.Entry(sede).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SedeExists(id))
                {
                    return NotFound(new { message = "Sede no encontrada" });
                }
                else
                {
                    throw;
                }
            }

            return Ok(new { message = "Sede actualizada exitosamente" });
        }


        [HttpPost]
        public async Task<ActionResult<Sedes>> PostSede(Sedes sede)
        {
            var sedeExistente = await _context.Sedes
                .FirstOrDefaultAsync(s => s.Codigo == sede.Codigo);

            if (sedeExistente != null)
            {
                return BadRequest(new { message = "Ya existe una sede con ese código" });
            }

            var empresa = await _context.Empresas.FindAsync(sede.IdEmpresa);
            if (empresa == null)
            {
                return BadRequest(new { message = "La empresa no existe" });
            }

            sede.FechaCreacion = DateTime.Now;
            sede.Estado = true;

            _context.Sedes.Add(sede);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetSede), new { id = sede.IdSede }, sede);
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSede(int id)
        {
            var sede = await _context.Sedes.FindAsync(id);
            if (sede == null)
            {
                return NotFound(new { message = "Sede no encontrada" });
            }

            var tieneDependenciasActivas = await _context.Dependencias
                .AnyAsync(d => d.IdSede == id && d.Estado);

            if (tieneDependenciasActivas)
            {
                return BadRequest(new { message = "No se puede desactivar una sede con dependencias activas" });
            }

            sede.Estado = false;
            await _context.SaveChangesAsync();

            return Ok(new { message = "Sede desactivada exitosamente" });
        }

        private bool SedeExists(int id)
        {
            return _context.Sedes.Any(e => e.IdSede == id);
        }
    }
}