using System;
using System.Collections.Generic;

namespace ProyectoInventariosWebApi.Models;

public partial class Entregas
{
    public int IdEntrega { get; set; }

    public int IdPedido { get; set; }

    public string? DireccionEntrega { get; set; }

    public DateTime? FechaEntrega { get; set; }

    public string? Estado { get; set; }

    public virtual Pedidos? IdPedidoNavigation { get; set; } = null!;
}
