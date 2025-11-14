using System;
using System.Collections.Generic;

namespace ProyectoInventariosWebApp.Models;

public partial class Facturas
{
    public int IdFactura { get; set; }

    public int IdPedido { get; set; }

    public DateTime? Fecha { get; set; }

    public decimal? Total { get; set; }

    public virtual Pedidos? IdPedidoNavigation { get; set; } = null!;
}
