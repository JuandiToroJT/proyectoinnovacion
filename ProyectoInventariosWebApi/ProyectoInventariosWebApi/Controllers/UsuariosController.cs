using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using ProyectoInventariosWebApi.Models;

namespace ProyectoInventariosWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuariosController : ControllerBase
    {
        private readonly ProyectoInventariosDbContext _context;
        private readonly PasswordHasher<Usuarios> _passwordHasher;

        public UsuariosController(ProyectoInventariosDbContext context)
        {
            _context = context;
            _passwordHasher = new PasswordHasher<Usuarios>();
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<object>>> GetUsuarios(
            [FromQuery] string? rol = null,
            [FromQuery] int? idSede = null,
            [FromQuery] bool? soloActivos = true)
        {
            var query = _context.Usuarios
                .Include(u => u.IdSedeNavigation)
                .Include(u => u.IdDependenciaNavigation)
                .AsQueryable();

            if (!string.IsNullOrEmpty(rol))
            {
                query = query.Where(u => u.Rol == rol);
            }

            if (idSede.HasValue)
            {
                query = query.Where(u => u.IdSede == idSede.Value);
            }

            if (soloActivos == true)
            {
                query = query.Where(u => u.Estado);
            }

            var usuarios = await query
                .Select(u => new
                {
                    u.IdUsuario,
                    u.Nombre,
                    u.Correo,
                    u.Rol,
                    u.Estado,
                    Sede = u.IdSede != null ? new
                    {
                        u.IdSede,
                        u.IdSedeNavigation.Nombre,
                        u.IdSedeNavigation.Codigo
                    } : null,
                    Dependencia = u.IdDependencia != null ? new
                    {
                        u.IdDependencia,
                        u.IdDependenciaNavigation.Nombre,
                        u.IdDependenciaNavigation.TipoDependencia
                    } : null,
                    u.FechaCreacion,
                    u.UltimoAcceso
                })
                .OrderBy(u => u.Nombre)
                .ToListAsync();

            return Ok(usuarios);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<object>> GetUsuario(int id)
        {
            var usuario = await _context.Usuarios
                .Include(u => u.IdSedeNavigation)
                .Include(u => u.IdDependenciaNavigation)
                .Where(u => u.IdUsuario == id)
                .Select(u => new
                {
                    u.IdUsuario,
                    u.Nombre,
                    u.Correo,
                    u.Rol,
                    u.Estado,
                    Sede = u.IdSede != null ? new
                    {
                        u.IdSede,
                        u.IdSedeNavigation.Nombre,
                        u.IdSedeNavigation.Codigo,
                        u.IdSedeNavigation.Direccion
                    } : null,
                    Dependencia = u.IdDependencia != null ? new
                    {
                        u.IdDependencia,
                        u.IdDependenciaNavigation.Nombre,
                        u.IdDependenciaNavigation.TipoDependencia,
                        u.IdDependenciaNavigation.Ubicacion,
                        u.IdDependenciaNavigation.Responsable
                    } : null,
                    u.FechaCreacion,
                    u.UltimoAcceso,
                    TotalPedidos = _context.Pedidos.Count(p => p.IdUsuario == id),
                    TransferenciasSolicitadas = _context.TransferenciaStock.Count(t => t.IdUsuarioSolicita == id),
                    TransferenciasAprobadas = _context.TransferenciaStock.Count(t => t.IdUsuarioAprueba == id)
                })
                .FirstOrDefaultAsync();

            if (usuario == null)
            {
                return NotFound(new { message = "Usuario no encontrado" });
            }

            return Ok(usuario);
        }

        [HttpGet("Roles")]
        public ActionResult<IEnumerable<string>> GetRoles()
        {
            var roles = new List<string>
            {
                "SuperAdmin",
                "AdminSede",
                "EncargadoDependencia",
                "Administrador",
                "Cliente"
            };

            return Ok(roles);
        }

        [HttpPost]
        public async Task<ActionResult<Usuarios>> PostUsuario(CrearUsuarioDto dto)
        {
            if (await _context.Usuarios.AnyAsync(u => u.Correo == dto.Correo))
            {
                return BadRequest(new { message = "Ya existe un usuario con ese correo" });
            }

            if (dto.Rol == "SuperAdmin")
            {
                dto.IdSede = null;
                dto.IdDependencia = null;
            }
            else if (dto.Rol == "AdminSede")
            {
                if (!dto.IdSede.HasValue)
                {
                    return BadRequest(new { message = "AdminSede requiere asignación de sede" });
                }

                var sede = await _context.Sedes.FindAsync(dto.IdSede.Value);
                if (sede == null || !sede.Estado)
                {
                    return BadRequest(new { message = "Sede no válida o inactiva" });
                }

                dto.IdDependencia = null;
            }
            else if (dto.Rol == "EncargadoDependencia")
            {
                if (!dto.IdDependencia.HasValue)
                {
                    return BadRequest(new { message = "EncargadoDependencia requiere asignación de dependencia" });
                }

                var dependencia = await _context.Dependencias
                    .Include(d => d.IdSedeNavigation)
                    .FirstOrDefaultAsync(d => d.IdDependencia == dto.IdDependencia.Value);

                if (dependencia == null || !dependencia.Estado)
                {
                    return BadRequest(new { message = "Dependencia no válida o inactiva" });
                }

                dto.IdSede = dependencia.IdSede;
            }

            var usuario = new Usuarios
            {
                Nombre = dto.Nombre,
                Correo = dto.Correo,
                Rol = dto.Rol,
                Estado = true,
                IdSede = dto.IdSede,
                IdDependencia = dto.IdDependencia,
                FechaCreacion = DateTime.Now
            };

            usuario.Contrasena = _passwordHasher.HashPassword(usuario, dto.Contrasena);

            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetUsuario), new { id = usuario.IdUsuario }, new
            {
                usuario.IdUsuario,
                usuario.Nombre,
                usuario.Correo,
                usuario.Rol,
                usuario.IdSede,
                usuario.IdDependencia,
                message = "Usuario creado exitosamente"
            });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutUsuario(int id, ActualizarUsuarioDto dto)
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null)
            {
                return NotFound(new { message = "Usuario no encontrado" });
            }

            if (dto.Correo != usuario.Correo)
            {
                if (await _context.Usuarios.AnyAsync(u => u.Correo == dto.Correo && u.IdUsuario != id))
                {
                    return BadRequest(new { message = "Ya existe otro usuario con ese correo" });
                }
                usuario.Correo = dto.Correo;
            }

            usuario.Nombre = dto.Nombre;
            usuario.Rol = dto.Rol;
            usuario.Estado = dto.Estado;

            if (dto.Rol == "SuperAdmin")
            {
                usuario.IdSede = null;
                usuario.IdDependencia = null;
            }
            else if (dto.Rol == "AdminSede")
            {
                if (!dto.IdSede.HasValue)
                {
                    return BadRequest(new { message = "AdminSede requiere asignación de sede" });
                }
                usuario.IdSede = dto.IdSede;
                usuario.IdDependencia = null;
            }
            else if (dto.Rol == "EncargadoDependencia")
            {
                if (!dto.IdDependencia.HasValue)
                {
                    return BadRequest(new { message = "EncargadoDependencia requiere asignación de dependencia" });
                }

                var dependencia = await _context.Dependencias.FindAsync(dto.IdDependencia.Value);
                if (dependencia == null)
                {
                    return BadRequest(new { message = "Dependencia no válida" });
                }

                usuario.IdDependencia = dto.IdDependencia;
                usuario.IdSede = dependencia.IdSede;
            }

            if (!string.IsNullOrEmpty(dto.NuevaContrasena))
            {
                usuario.Contrasena = _passwordHasher.HashPassword(usuario, dto.NuevaContrasena);
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UsuarioExists(id))
                {
                    return NotFound(new { message = "Usuario no encontrado" });
                }
                else
                {
                    throw;
                }
            }

            return Ok(new { message = "Usuario actualizado exitosamente" });
        }

        [HttpPut("{id}/AsignarSede")]
        public async Task<IActionResult> AsignarSede(int id, [FromBody] AsignarSedeDto dto)
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null)
            {
                return NotFound(new { message = "Usuario no encontrado" });
            }

            if (usuario.Rol == "SuperAdmin")
            {
                return BadRequest(new { message = "Los SuperAdmin no tienen sede asignada" });
            }

            var sede = await _context.Sedes.FindAsync(dto.IdSede);
            if (sede == null || !sede.Estado)
            {
                return BadRequest(new { message = "Sede no válida o inactiva" });
            }

            usuario.IdSede = dto.IdSede;

            if (usuario.IdDependencia.HasValue)
            {
                var dependencia = await _context.Dependencias.FindAsync(usuario.IdDependencia.Value);
                if (dependencia != null && dependencia.IdSede != dto.IdSede)
                {
                    usuario.IdDependencia = null;
                }
            }

            await _context.SaveChangesAsync();

            return Ok(new { message = "Sede asignada exitosamente" });
        }

        [HttpPut("{id}/AsignarDependencia")]
        public async Task<IActionResult> AsignarDependencia(int id, [FromBody] AsignarDependenciaDto dto)
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null)
            {
                return NotFound(new { message = "Usuario no encontrado" });
            }

            if (usuario.Rol != "EncargadoDependencia")
            {
                return BadRequest(new { message = "Solo EncargadoDependencia puede tener dependencia asignada" });
            }

            var dependencia = await _context.Dependencias.FindAsync(dto.IdDependencia);
            if (dependencia == null || !dependencia.Estado)
            {
                return BadRequest(new { message = "Dependencia no válida o inactiva" });
            }

            usuario.IdDependencia = dto.IdDependencia;
            usuario.IdSede = dependencia.IdSede;

            await _context.SaveChangesAsync();

            return Ok(new { message = "Dependencia asignada exitosamente" });
        }

        [HttpPost("Login")]
        public async Task<ActionResult<object>> Login([FromBody] LoginDto dto)
        {
            var usuario = await _context.Usuarios
                .Include(u => u.IdSedeNavigation)
                .Include(u => u.IdDependenciaNavigation)
                .FirstOrDefaultAsync(u => u.Correo == dto.Correo);

            if (usuario == null)
            {
                return Unauthorized(new { message = "Correo o contraseña incorrectos" });
            }

            if (!usuario.Estado)
            {
                return Unauthorized(new { message = "Usuario inactivo" });
            }

            var resultado = _passwordHasher.VerifyHashedPassword(usuario, usuario.Contrasena, dto.Contrasena);
            if (resultado == PasswordVerificationResult.Failed)
            {
                return Unauthorized(new { message = "Correo o contraseña incorrectos" });
            }

            usuario.UltimoAcceso = DateTime.Now;
            await _context.SaveChangesAsync();

            return Ok(new
            {
                usuario.IdUsuario,
                usuario.Nombre,
                usuario.Correo,
                usuario.Rol,
                Sede = usuario.IdSede != null ? new
                {
                    usuario.IdSede,
                    usuario.IdSedeNavigation.Nombre,
                    usuario.IdSedeNavigation.Codigo
                } : null,
                Dependencia = usuario.IdDependencia != null ? new
                {
                    usuario.IdDependencia,
                    usuario.IdDependenciaNavigation.Nombre,
                    usuario.IdDependenciaNavigation.TipoDependencia
                } : null,
                message = "Login exitoso",
                IdSede = usuario.IdSede
            });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUsuario(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null)
            {
                return NotFound(new { message = "Usuario no encontrado" });
            }

            usuario.Estado = false;
            await _context.SaveChangesAsync();

            return Ok(new { message = "Usuario desactivado exitosamente" });
        }

        private bool UsuarioExists(int id)
        {
            return _context.Usuarios.Any(e => e.IdUsuario == id);
        }
    }

    public class CrearUsuarioDto
    {
        public string Nombre { get; set; } = null!;
        public string Correo { get; set; } = null!;
        public string Contrasena { get; set; } = null!;
        public string Rol { get; set; } = null!;
        public int? IdSede { get; set; }
        public int? IdDependencia { get; set; }
    }

    public class ActualizarUsuarioDto
    {
        public string Nombre { get; set; } = null!;
        public string Correo { get; set; } = null!;
        public string Rol { get; set; } = null!;
        public bool Estado { get; set; }
        public int? IdSede { get; set; }
        public int? IdDependencia { get; set; }
        public string? NuevaContrasena { get; set; }
    }

    public class AsignarSedeDto
    {
        public int IdSede { get; set; }
    }

    public class AsignarDependenciaDto
    {
        public int IdDependencia { get; set; }
    }

    public class LoginDto
    {
        public string Correo { get; set; } = null!;
        public string Contrasena { get; set; } = null!;
    }
}