using System;
using System.Collections.Generic;

namespace ProyectoInventariosWebApi.Models;

public partial class Facturas
{
    public int IdFactura { get; set; }

    public int IdPedido { get; set; }

    public DateTime? Fecha { get; set; }

    public decimal? Total { get; set; }

    public string? NumeroFactura { get; set; }

    public decimal? Subtotal { get; set; }

    public decimal? Iva { get; set; }

    public decimal? Descuentos { get; set; }

    public string? MetodoPago { get; set; }

    public string? EstadoPago { get; set; }

    public string? Observaciones { get; set; }

    public virtual Pedidos? IdPedidoNavigation { get; set; } = null!;
}