using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProyectoInventariosWebApi.Models;

namespace ProyectoInventariosWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransferenciaStockController : ControllerBase
    {
        private readonly ProyectoInventariosDbContext _context;

        public TransferenciaStockController(ProyectoInventariosDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<object>>> GetTransferencias([FromQuery] string? estado = null)
        {
            var query = _context.TransferenciaStock
                .Include(t => t.IdProductoNavigation)
                .Include(t => t.IdDependenciaOrigenNavigation)
                    .ThenInclude(d => d.IdSedeNavigation)
                .Include(t => t.IdDependenciaDestinoNavigation)
                    .ThenInclude(d => d.IdSedeNavigation)
                .Include(t => t.IdUsuarioSolicitaNavigation)
                .Include(t => t.IdUsuarioApruebaNavigation)
                .AsQueryable();

            if (!string.IsNullOrEmpty(estado))
            {
                query = query.Where(t => t.Estado == estado);
            }

            var transferencias = await query
                .OrderByDescending(t => t.FechaSolicitud)
                .Select(t => new
                {
                    t.IdTransferencia,
                    Producto = new
                    {
                        t.IdProducto,
                        t.IdProductoNavigation.Codigo,
                        t.IdProductoNavigation.Nombre
                    },
                    Origen = new
                    {
                        IdDependencia = t.IdDependenciaOrigen,
                        Dependencia = t.IdDependenciaOrigenNavigation.Nombre,
                        Sede = t.IdDependenciaOrigenNavigation.IdSedeNavigation.Nombre
                    },
                    Destino = new
                    {
                        IdDependencia = t.IdDependenciaDestino,
                        Dependencia = t.IdDependenciaDestinoNavigation.Nombre,
                        Sede = t.IdDependenciaDestinoNavigation.IdSedeNavigation.Nombre
                    },
                    t.Cantidad,
                    t.Motivo,
                    t.Estado,
                    t.FechaSolicitud,
                    t.FechaAprobacion,
                    t.FechaEjecucion,
                    UsuarioSolicita = t.IdUsuarioSolicitaNavigation.Nombre,
                    UsuarioAprueba = t.IdUsuarioApruebaNavigation != null ? t.IdUsuarioApruebaNavigation.Nombre : null,
                    t.Observaciones,
                    t.CostoTransporte
                })
                .ToListAsync();

            return Ok(transferencias);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<object>> GetTransferencia(int id)
        {
            var transferencia = await _context.TransferenciaStock
                .Include(t => t.IdProductoNavigation)
                .Include(t => t.IdDependenciaOrigenNavigation)
                    .ThenInclude(d => d.IdSedeNavigation)
                .Include(t => t.IdDependenciaDestinoNavigation)
                    .ThenInclude(d => d.IdSedeNavigation)
                .Include(t => t.IdUsuarioSolicitaNavigation)
                .Include(t => t.IdUsuarioApruebaNavigation)
                .Where(t => t.IdTransferencia == id)
                .Select(t => new
                {
                    t.IdTransferencia,
                    Producto = new
                    {
                        t.IdProducto,
                        t.IdProductoNavigation.Codigo,
                        t.IdProductoNavigation.Nombre,
                        t.IdProductoNavigation.EsCompartible
                    },
                    Origen = new
                    {
                        IdDependencia = t.IdDependenciaOrigen,
                        Dependencia = t.IdDependenciaOrigenNavigation.Nombre,
                        Sede = t.IdDependenciaOrigenNavigation.IdSedeNavigation.Nombre,
                        IdSede = t.IdDependenciaOrigenNavigation.IdSede
                    },
                    Destino = new
                    {
                        IdDependencia = t.IdDependenciaDestino,
                        Dependencia = t.IdDependenciaDestinoNavigation.Nombre,
                        Sede = t.IdDependenciaDestinoNavigation.IdSedeNavigation.Nombre,
                        IdSede = t.IdDependenciaDestinoNavigation.IdSede
                    },
                    t.Cantidad,
                    t.Motivo,
                    t.Estado,
                    t.FechaSolicitud,
                    t.FechaAprobacion,
                    t.FechaEjecucion,
                    UsuarioSolicita = new
                    {
                        t.IdUsuarioSolicita,
                        t.IdUsuarioSolicitaNavigation.Nombre,
                        t.IdUsuarioSolicitaNavigation.Correo
                    },
                    UsuarioAprueba = t.IdUsuarioAprueba != null ? new
                    {
                        IdUsuario = t.IdUsuarioAprueba,
                        t.IdUsuarioApruebaNavigation.Nombre,
                        t.IdUsuarioApruebaNavigation.Correo
                    } : null,
                    t.Observaciones,
                    t.CostoTransporte
                })
                .FirstOrDefaultAsync();

            if (transferencia == null)
            {
                return NotFound(new { message = "Transferencia no encontrada" });
            }

            return Ok(transferencia);
        }

        [HttpPost("Solicitar")]
        public async Task<ActionResult> SolicitarTransferencia([FromBody] SolicitudTransferenciaDto solicitud)
        {
            var producto = await _context.Productos.FindAsync(solicitud.IdProducto);
            if (producto == null)
            {
                return BadRequest(new { message = "Producto no encontrado" });
            }

            if (!producto.EsCompartible)
            {
                return BadRequest(new { message = "Este producto no puede ser transferido entre dependencias" });
            }

            var dependenciaOrigen = await _context.Dependencias
                .Include(d => d.IdSedeNavigation)
                .FirstOrDefaultAsync(d => d.IdDependencia == solicitud.IdDependenciaOrigen);

            var dependenciaDestino = await _context.Dependencias
                .Include(d => d.IdSedeNavigation)
                .FirstOrDefaultAsync(d => d.IdDependencia == solicitud.IdDependenciaDestino);

            if (dependenciaOrigen == null || dependenciaDestino == null)
            {
                return BadRequest(new { message = "Una o ambas dependencias no existen" });
            }

            if (solicitud.IdDependenciaOrigen == solicitud.IdDependenciaDestino)
            {
                return BadRequest(new { message = "La dependencia de origen y destino no pueden ser la misma" });
            }

            var inventarioOrigen = await _context.InventarioDependencia
                .FirstOrDefaultAsync(i => i.IdProducto == solicitud.IdProducto &&
                                         i.IdDependencia == solicitud.IdDependenciaOrigen);

            if (inventarioOrigen == null)
            {
                return BadRequest(new { message = "El producto no existe en la dependencia de origen" });
            }

            if (inventarioOrigen.StockActual < solicitud.Cantidad)
            {
                return BadRequest(new
                {
                    message = $"Stock insuficiente en origen. Disponible: {inventarioOrigen.StockActual}, Solicitado: {solicitud.Cantidad}"
                });
            }

            var usuario = await _context.Usuarios.FindAsync(solicitud.IdUsuarioSolicita);
            if (usuario == null)
            {
                return BadRequest(new { message = "Usuario no encontrado" });
            }

            var transferencia = new TransferenciaStock
            {
                IdProducto = solicitud.IdProducto,
                IdDependenciaOrigen = solicitud.IdDependenciaOrigen,
                IdDependenciaDestino = solicitud.IdDependenciaDestino,
                Cantidad = solicitud.Cantidad,
                Motivo = solicitud.Motivo,
                Estado = "Pendiente",
                FechaSolicitud = DateTime.Now,
                IdUsuarioSolicita = solicitud.IdUsuarioSolicita,
                Observaciones = solicitud.Observaciones,
                CostoTransporte = solicitud.CostoTransporte
            };

            _context.TransferenciaStock.Add(transferencia);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                message = "Transferencia solicitada exitosamente",
                idTransferencia = transferencia.IdTransferencia,
                estado = transferencia.Estado,
                origen = dependenciaOrigen.Nombre,
                destino = dependenciaDestino.Nombre,
                producto = producto.Nombre,
                cantidad = transferencia.Cantidad
            });
        }

        [HttpPut("{id}/Aprobar")]
        public async Task<IActionResult> AprobarTransferencia(int id, [FromBody] AprobacionDto aprobacion)
        {
            var transferencia = await _context.TransferenciaStock
                .Include(t => t.IdProductoNavigation)
                .FirstOrDefaultAsync(t => t.IdTransferencia == id);

            if (transferencia == null)
            {
                return NotFound(new { message = "Transferencia no encontrada" });
            }

            if (transferencia.Estado != "Pendiente")
            {
                return BadRequest(new { message = $"Esta transferencia ya está en estado: {transferencia.Estado}" });
            }

            var usuario = await _context.Usuarios.FindAsync(aprobacion.IdUsuarioAprueba);
            if (usuario == null)
            {
                return BadRequest(new { message = "Usuario no encontrado" });
            }

            var inventarioOrigen = await _context.InventarioDependencia
                .FirstOrDefaultAsync(i => i.IdProducto == transferencia.IdProducto &&
                                         i.IdDependencia == transferencia.IdDependenciaOrigen);

            if (inventarioOrigen == null || inventarioOrigen.StockActual < transferencia.Cantidad)
            {
                return BadRequest(new { message = "Stock insuficiente en origen para aprobar la transferencia" });
            }

            transferencia.Estado = "Aprobada";
            transferencia.FechaAprobacion = DateTime.Now;
            transferencia.IdUsuarioAprueba = aprobacion.IdUsuarioAprueba;
            transferencia.Observaciones = aprobacion.Observaciones ?? transferencia.Observaciones;

            await _context.SaveChangesAsync();

            return Ok(new { message = "Transferencia aprobada exitosamente" });
        }

        [HttpPut("{id}/Rechazar")]
        public async Task<IActionResult> RechazarTransferencia(int id, [FromBody] RechazoDto rechazo)
        {
            var transferencia = await _context.TransferenciaStock.FindAsync(id);
            if (transferencia == null)
            {
                return NotFound(new { message = "Transferencia no encontrada" });
            }

            if (transferencia.Estado != "Pendiente")
            {
                return BadRequest(new { message = $"Esta transferencia ya está en estado: {transferencia.Estado}" });
            }

            transferencia.Estado = "Rechazada";
            transferencia.IdUsuarioAprueba = rechazo.IdUsuarioRechaza;
            transferencia.Observaciones = rechazo.MotivoRechazo;

            await _context.SaveChangesAsync();

            return Ok(new { message = "Transferencia rechazada" });
        }

        [HttpPut("{id}/Ejecutar")]
        public async Task<IActionResult> EjecutarTransferencia(int id)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var transferencia = await _context.TransferenciaStock
                    .Include(t => t.IdProductoNavigation)
                    .FirstOrDefaultAsync(t => t.IdTransferencia == id);

                if (transferencia == null)
                {
                    return NotFound(new { message = "Transferencia no encontrada" });
                }

                if (transferencia.Estado != "Aprobada")
                {
                    return BadRequest(new { message = $"Solo se pueden ejecutar transferencias aprobadas. Estado actual: {transferencia.Estado}" });
                }

                var inventarioOrigen = await _context.InventarioDependencia
                    .FirstOrDefaultAsync(i => i.IdProducto == transferencia.IdProducto &&
                                             i.IdDependencia == transferencia.IdDependenciaOrigen);

                var inventarioDestino = await _context.InventarioDependencia
                    .FirstOrDefaultAsync(i => i.IdProducto == transferencia.IdProducto &&
                                             i.IdDependencia == transferencia.IdDependenciaDestino);

                if (inventarioOrigen == null || inventarioOrigen.StockActual < transferencia.Cantidad)
                {
                    return BadRequest(new { message = "Stock insuficiente en origen" });
                }

                if (inventarioDestino == null)
                {
                    inventarioDestino = new InventarioDependencia
                    {
                        IdProducto = transferencia.IdProducto,
                        IdDependencia = transferencia.IdDependenciaDestino,
                        StockActual = 0,
                        StockMinimo = inventarioOrigen.StockMinimo,
                        StockMaximo = inventarioOrigen.StockMaximo,
                        PuntoReorden = inventarioOrigen.PuntoReorden,
                        CostoPromedio = inventarioOrigen.CostoPromedio,
                        UltimaActualizacion = DateTime.Now,
                        EstadoInventario = "Disponible"
                    };
                    _context.InventarioDependencia.Add(inventarioDestino);
                    await _context.SaveChangesAsync();
                }

                var stockAnteriorOrigen = inventarioOrigen.StockActual;
                var stockAnteriorDestino = inventarioDestino.StockActual;

                inventarioOrigen.StockActual -= transferencia.Cantidad;
                inventarioOrigen.UltimaActualizacion = DateTime.Now;
                inventarioOrigen.EstadoInventario = inventarioOrigen.StockActual > 0 ? "Disponible" : "Agotado";

                inventarioDestino.StockActual += transferencia.Cantidad;
                inventarioDestino.UltimaActualizacion = DateTime.Now;
                inventarioDestino.EstadoInventario = "Disponible";

                var movimientoSalida = new MovimientoInventario
                {
                    IdInventario = inventarioOrigen.IdInventario,
                    TipoMovimiento = "Transferencia",
                    Cantidad = -transferencia.Cantidad,
                    StockAnterior = stockAnteriorOrigen,
                    StockNuevo = inventarioOrigen.StockActual,
                    Fecha = DateTime.Now,
                    IdUsuario = transferencia.IdUsuarioAprueba ?? transferencia.IdUsuarioSolicita,
                    TipoReferencia = "Transferencia",
                    IdReferencia = transferencia.IdTransferencia,
                    Observaciones = $"Transferencia #{id} - Salida hacia {transferencia.IdDependenciaDestino}",
                    CostoUnitario = inventarioOrigen.CostoPromedio
                };

                var movimientoEntrada = new MovimientoInventario
                {
                    IdInventario = inventarioDestino.IdInventario,
                    TipoMovimiento = "Transferencia",
                    Cantidad = transferencia.Cantidad,
                    StockAnterior = stockAnteriorDestino,
                    StockNuevo = inventarioDestino.StockActual,
                    Fecha = DateTime.Now,
                    IdUsuario = transferencia.IdUsuarioAprueba ?? transferencia.IdUsuarioSolicita,
                    TipoReferencia = "Transferencia",
                    IdReferencia = transferencia.IdTransferencia,
                    Observaciones = $"Transferencia #{id} - Entrada desde {transferencia.IdDependenciaOrigen}",
                    CostoUnitario = inventarioDestino.CostoPromedio
                };

                _context.MovimientoInventario.AddRange(movimientoSalida, movimientoEntrada);
                transferencia.Estado = "Ejecutada";
                transferencia.FechaEjecucion = DateTime.Now;

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return Ok(new
                {
                    message = "Transferencia ejecutada exitosamente",
                    stockOrigenAnterior = stockAnteriorOrigen,
                    stockOrigenNuevo = inventarioOrigen.StockActual,
                    stockDestinoAnterior = stockAnteriorDestino,
                    stockDestinoNuevo = inventarioDestino.StockActual
                });
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return StatusCode(500, new { message = "Error al ejecutar la transferencia", error = ex.Message });
            }
        }

        [HttpDelete("{id}/Cancelar")]
        public async Task<IActionResult> CancelarTransferencia(int id)
        {
            var transferencia = await _context.TransferenciaStock.FindAsync(id);
            if (transferencia == null)
            {
                return NotFound(new { message = "Transferencia no encontrada" });
            }

            if (transferencia.Estado != "Pendiente")
            {
                return BadRequest(new { message = $"Solo se pueden cancelar transferencias pendientes. Estado actual: {transferencia.Estado}" });
            }

            transferencia.Estado = "Cancelada";
            await _context.SaveChangesAsync();

            return Ok(new { message = "Transferencia cancelada exitosamente" });
        }
    }

    public class SolicitudTransferenciaDto
    {
        public int IdProducto { get; set; }
        public int IdDependenciaOrigen { get; set; }
        public int IdDependenciaDestino { get; set; }
        public int Cantidad { get; set; }
        public string Motivo { get; set; } = null!;
        public int IdUsuarioSolicita { get; set; }
        public string? Observaciones { get; set; }
        public decimal? CostoTransporte { get; set; }
    }

    public class AprobacionDto
    {
        public int IdUsuarioAprueba { get; set; }
        public string? Observaciones { get; set; }
    }

    public class RechazoDto
    {
        public int IdUsuarioRechaza { get; set; }
        public string MotivoRechazo { get; set; } = null!;
    }
}