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

        [HttpGet]
        public async Task<ActionResult<IEnumerable<object>>> GetPedidos(
            [FromQuery] string? estado = null,
            [FromQuery] int? idSede = null,
            [FromQuery] int? idDependencia = null,
            [FromQuery] int? idCliente = null)
        {
            var query = _context.Pedidos
                .Include(p => p.IdClienteNavigation)
                .Include(p => p.IdUsuarioNavigation)
                .Include(p => p.IdSedeNavigation)
                .Include(p => p.IdDependenciaNavigation)
                .Include(p => p.DetallesPedido)
                .AsQueryable();

            if (!string.IsNullOrEmpty(estado))
            {
                query = query.Where(p => p.Estado == estado);
            }

            if (idSede.HasValue)
            {
                query = query.Where(p => p.IdSede == idSede.Value);
            }

            if (idDependencia.HasValue)
            {
                query = query.Where(p => p.IdDependencia == idDependencia.Value);
            }

            if (idCliente.HasValue)
            {
                query = query.Where(p => p.IdCliente == idCliente.Value);
            }

            var pedidos = await query
                .OrderByDescending(p => p.Fecha)
                .Select(p => new
                {
                    p.IdPedido,
                    Cliente = p.IdClienteNavigation.Nombre,
                    Usuario = p.IdUsuarioNavigation != null ? p.IdUsuarioNavigation.Nombre : null,
                    Sede = p.IdSedeNavigation != null ? p.IdSedeNavigation.Nombre : null,
                    Dependencia = p.IdDependenciaNavigation != null ? p.IdDependenciaNavigation.Nombre : null,
                    p.Fecha,
                    p.Estado,
                    p.Total,
                    p.TipoEntrega,
                    p.MetodoPago,
                    CantidadProductos = p.DetallesPedido.Count,
                    TotalUnidades = p.DetallesPedido.Sum(d => d.Cantidad),
                    IdCliente = p.IdCliente,
                    IdClienteNavigation = p.IdClienteNavigation,
                    IdUsuario = p.IdUsuario,
                    IdUsuarioNavigation = p.IdUsuarioNavigation,
                    DetallesPedido = p.DetallesPedido
                })
                .ToListAsync();

            return Ok(pedidos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<object>> GetPedido(int id)
        {
            var pedido = await _context.Pedidos
                .Include(p => p.IdClienteNavigation)
                .Include(p => p.IdUsuarioNavigation)
                .Include(p => p.IdSedeNavigation)
                .Include(p => p.IdDependenciaNavigation)
                .Include(p => p.DetallesPedido)
                    .ThenInclude(d => d.IdProductoNavigation)
                .Include(p => p.DetallesPedido)
                    .ThenInclude(d => d.IdInventarioNavigation)
                .Include(p => p.Entregas)
                .Include(p => p.Facturas)
                .Where(p => p.IdPedido == id)
                .Select(p => new
                {
                    p.IdPedido,
                    Cliente = new
                    {
                        p.IdCliente,
                        p.IdClienteNavigation.Nombre,
                        p.IdClienteNavigation.Telefono,
                        p.IdClienteNavigation.Direccion,
                        p.IdClienteNavigation.Email
                    },
                    Usuario = p.IdUsuario != null ? new
                    {
                        p.IdUsuario,
                        p.IdUsuarioNavigation.Nombre,
                        p.IdUsuarioNavigation.Rol
                    } : null,
                    Sede = p.IdSede != null ? new
                    {
                        p.IdSede,
                        p.IdSedeNavigation.Nombre,
                        p.IdSedeNavigation.Direccion
                    } : null,
                    Dependencia = p.IdDependencia != null ? new
                    {
                        p.IdDependencia,
                        p.IdDependenciaNavigation.Nombre,
                        p.IdDependenciaNavigation.TipoDependencia
                    } : null,
                    p.Fecha,
                    p.Estado,
                    p.Total,
                    p.TipoEntrega,
                    p.MetodoPago,
                    p.Observaciones,
                    p.FechaEstimadaEntrega,
                    Detalles = p.DetallesPedido.Select(d => new
                    {
                        d.IdDetalle,
                        Producto = new
                        {
                            d.IdProducto,
                            d.IdProductoNavigation.Codigo,
                            d.IdProductoNavigation.Nombre,
                            d.IdProductoNavigation.UnidadMedida
                        },
                        d.Cantidad,
                        d.PrecioUnitario,
                        d.Subtotal,
                        d.Descuento,
                        Inventario = d.IdInventario != null ? new
                        {
                            d.IdInventario,
                            Dependencia = d.IdInventarioNavigation.IdDependenciaNavigation.Nombre
                        } : null
                    }).ToList(),
                    Entregas = p.Entregas.Select(e => new
                    {
                        e.IdEntrega,
                        e.DireccionEntrega,
                        e.FechaEntrega,
                        e.Estado,
                        e.Transportista,
                        e.CostoEnvio
                    }).ToList(),
                    Facturas = p.Facturas.Select(f => new
                    {
                        f.IdFactura,
                        f.NumeroFactura,
                        f.Fecha,
                        f.Subtotal,
                        f.Iva,
                        f.Descuentos,
                        f.Total,
                        f.EstadoPago
                    }).ToList(),
                    IdCliente = p.IdCliente,
                    IdClienteNavigation = p.IdClienteNavigation,
                    IdUsuario = p.IdUsuario,
                    IdUsuarioNavigation = p.IdUsuarioNavigation,
                    DetallesPedido = p.DetallesPedido
                })
                .FirstOrDefaultAsync();

            if (pedido == null)
            {
                return NotFound(new { message = "Pedido no encontrado" });
            }

            return Ok(pedido);
        }

        [HttpPost]
        public async Task<ActionResult<Pedidos>> PostPedido(CrearPedidoDto dto)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var cliente = await _context.Clientes.FindAsync(dto.IdCliente);
                if (cliente == null)
                {
                    return BadRequest(new { message = "Cliente no encontrado" });
                }

                int? idSede = dto.IdSede ?? cliente.IdSedePredeterminada;
                int? idDependencia = dto.IdDependencia;

                if (!idDependencia.HasValue && idSede.HasValue)
                {
                    idDependencia = await _context.Dependencias
                        .Where(d => d.IdSede == idSede.Value &&
                                   d.TipoDependencia == "Papelería" &&
                                   d.Estado)
                        .Select(d => d.IdDependencia)
                        .FirstOrDefaultAsync();
                }

                var detallesPedido = new List<DetallesPedido>();
                decimal totalPedido = 0;

                foreach (var item in dto.Detalles)
                {
                    var producto = await _context.Productos.FindAsync(item.IdProducto);
                    if (producto == null)
                    {
                        await transaction.RollbackAsync();
                        return BadRequest(new { message = $"Producto {item.IdProducto} no encontrado" });
                    }

                    if (!producto.Estado)
                    {
                        await transaction.RollbackAsync();
                        return BadRequest(new { message = $"El producto '{producto.Nombre}' está inactivo" });
                    }

                    InventarioDependencia? inventario = null;

                    if (idDependencia.HasValue)
                    {
                        inventario = await _context.InventarioDependencia
                            .FirstOrDefaultAsync(i => i.IdProducto == item.IdProducto &&
                                                     i.IdDependencia == idDependencia.Value &&
                                                     i.StockActual >= item.Cantidad);
                    }

                    if (inventario == null && idSede.HasValue)
                    {
                        inventario = await _context.InventarioDependencia
                            .Include(i => i.IdDependenciaNavigation)
                            .Where(i => i.IdProducto == item.IdProducto &&
                                       i.IdDependenciaNavigation.IdSede == idSede.Value &&
                                       i.StockActual >= item.Cantidad)
                            .OrderByDescending(i => i.StockActual)
                            .FirstOrDefaultAsync();
                    }

                    if (inventario == null && producto.EsCompartible)
                    {
                        inventario = await _context.InventarioDependencia
                            .Where(i => i.IdProducto == item.IdProducto &&
                                       i.StockActual >= item.Cantidad)
                            .OrderByDescending(i => i.StockActual)
                            .FirstOrDefaultAsync();
                    }

                    if (inventario == null)
                    {
                        await transaction.RollbackAsync();
                        return BadRequest(new
                        {
                            message = $"Stock insuficiente para el producto '{producto.Nombre}'",
                            idProducto = item.IdProducto,
                            cantidadSolicitada = item.Cantidad
                        });
                    }

                    var precioUnitario = item.PrecioUnitario ?? producto.Precio;
                    var subtotal = precioUnitario * item.Cantidad;
                    var descuento = item.Descuento ?? 0;
                    var subtotalFinal = subtotal - descuento;

                    var detalle = new DetallesPedido
                    {
                        IdProducto = item.IdProducto,
                        IdInventario = inventario.IdInventario,
                        Cantidad = item.Cantidad,
                        PrecioUnitario = precioUnitario,
                        Descuento = descuento,
                        Subtotal = subtotalFinal,
                        Observaciones = item.Observaciones
                    };

                    detallesPedido.Add(detalle);
                    totalPedido += subtotalFinal;
                }

                var pedido = new Pedidos
                {
                    IdCliente = dto.IdCliente,
                    IdUsuario = dto.IdUsuario,
                    IdSede = idSede,
                    IdDependencia = idDependencia,
                    Fecha = DateTime.Now,
                    Estado = "Pendiente",
                    Total = totalPedido,
                    TipoEntrega = dto.TipoEntrega ?? "Retiro",
                    MetodoPago = dto.MetodoPago,
                    Observaciones = dto.Observaciones,
                    FechaEstimadaEntrega = dto.FechaEstimadaEntrega
                };

                _context.Pedidos.Add(pedido);
                await _context.SaveChangesAsync();

                foreach (var detalle in detallesPedido)
                {
                    detalle.IdPedido = pedido.IdPedido;
                    _context.DetallesPedido.Add(detalle);
                }

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return CreatedAtAction(nameof(GetPedido), new { id = pedido.IdPedido }, new
                {
                    pedido.IdPedido,
                    pedido.Estado,
                    pedido.Total,
                    TotalProductos = detallesPedido.Count,
                    Sede = idSede,
                    Dependencia = idDependencia,
                    message = "Pedido creado exitosamente"
                });
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return StatusCode(500, new { message = "Error al crear el pedido", error = ex.Message });
            }
        }

        // PUT: api/Pedidos/5/Estado
        /// <summary>
        /// Actualiza el estado de un pedido y reduce el stock cuando se entrega
        /// </summary>
        [HttpPut("{id}/Estado")]
        public async Task<IActionResult> ActualizarEstado(int id, [FromBody] ActualizarEstadoDto dto)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var pedido = await _context.Pedidos
                    .Include(p => p.DetallesPedido)
                        .ThenInclude(d => d.IdInventarioNavigation)
                    .FirstOrDefaultAsync(p => p.IdPedido == id);

                if (pedido == null)
                {
                    return NotFound(new { message = "Pedido no encontrado" });
                }

                var estadoAnterior = pedido.Estado;
                pedido.Estado = dto.NuevoEstado;

                // Si se marca como ENTREGADO, reducir el stock
                if (dto.NuevoEstado == "Entregado" && estadoAnterior != "Entregado")
                {
                    foreach (var detalle in pedido.DetallesPedido)
                    {
                        if (detalle.IdInventario.HasValue)
                        {
                            var inventario = detalle.IdInventarioNavigation;
                            if (inventario == null)
                            {
                                inventario = await _context.InventarioDependencia
                                    .FindAsync(detalle.IdInventario.Value);
                            }

                            if (inventario == null || inventario.StockActual < detalle.Cantidad)
                            {
                                await transaction.RollbackAsync();
                                return BadRequest(new
                                {
                                    message = "Stock insuficiente para completar la entrega",
                                    idDetalle = detalle.IdDetalle
                                });
                            }

                            var stockAnterior = inventario.StockActual;
                            inventario.StockActual -= detalle.Cantidad;
                            inventario.UltimaActualizacion = DateTime.Now;
                            inventario.EstadoInventario = inventario.StockActual > 0 ? "Disponible" : "Agotado";

                            // Registrar movimiento
                            var movimiento = new MovimientoInventario
                            {
                                IdInventario = inventario.IdInventario,
                                TipoMovimiento = "Salida",
                                Cantidad = -detalle.Cantidad,
                                StockAnterior = stockAnterior,
                                StockNuevo = inventario.StockActual,
                                Fecha = DateTime.Now,
                                IdUsuario = dto.IdUsuario ?? 1,
                                TipoReferencia = "Pedido",
                                IdReferencia = pedido.IdPedido,
                                Observaciones = $"Venta - Pedido #{pedido.IdPedido}",
                                CostoUnitario = inventario.CostoPromedio
                            };

                            _context.MovimientoInventario.Add(movimiento);
                        }
                    }
                }

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return Ok(new
                {
                    message = "Estado actualizado exitosamente",
                    estadoAnterior,
                    estadoNuevo = dto.NuevoEstado
                });
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return StatusCode(500, new { message = "Error al actualizar el estado", error = ex.Message });
            }
        }

        // PUT: api/Pedidos/5
        /// <summary>
        /// Actualiza un pedido
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPedido(int id, Pedidos pedido)
        {
            if (id != pedido.IdPedido)
            {
                return BadRequest(new { message = "El ID no coincide" });
            }

            _context.Entry(pedido).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PedidoExists(id))
                {
                    return NotFound(new { message = "Pedido no encontrado" });
                }
                else
                {
                    throw;
                }
            }

            return Ok(new { message = "Pedido actualizado exitosamente" });
        }

        private bool PedidoExists(int id)
        {
            return _context.Pedidos.Any(e => e.IdPedido == id);
        }
    }

    // DTOs
    public class CrearPedidoDto
    {
        public int IdCliente { get; set; }
        public int? IdUsuario { get; set; }
        public int? IdSede { get; set; }
        public int? IdDependencia { get; set; }
        public string? TipoEntrega { get; set; }
        public string? MetodoPago { get; set; }
        public string? Observaciones { get; set; }
        public DateTime? FechaEstimadaEntrega { get; set; }
        public List<DetallePedidoDto> Detalles { get; set; } = new();
    }

    public class DetallePedidoDto
    {
        public int IdProducto { get; set; }
        public int Cantidad { get; set; }
        public decimal? PrecioUnitario { get; set; }
        public decimal? Descuento { get; set; }
        public string? Observaciones { get; set; }
    }

    public class ActualizarEstadoDto
    {
        public string NuevoEstado { get; set; } = null!;
        public int? IdUsuario { get; set; }
    }
}