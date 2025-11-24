using System;
using System.Collections.Generic;

namespace ProyectoInventariosWebApi.Models;

public partial class DetallesPedido
{
    public int IdDetalle { get; set; }

    public int IdPedido { get; set; }

    public int IdProducto { get; set; }

    public int Cantidad { get; set; }

    public decimal? Subtotal { get; set; }

    public int? IdInventario { get; set; }

    public decimal? PrecioUnitario { get; set; }

    public decimal? Descuento { get; set; }

    public string? Observaciones { get; set; }

    public virtual Pedidos? IdPedidoNavigation { get; set; } = null!;

    public virtual Productos? IdProductoNavigation { get; set; } = null!;

    public virtual InventarioDependencia? IdInventarioNavigation { get; set; }
}