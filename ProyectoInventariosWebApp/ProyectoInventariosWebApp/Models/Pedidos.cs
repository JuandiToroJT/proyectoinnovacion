using System;
using System.Collections.Generic;

namespace ProyectoInventariosWebApp.Models;

public partial class Pedidos
{
    public int IdPedido { get; set; }

    public int IdCliente { get; set; }

    public int? IdUsuario { get; set; } = null;

    public DateTime? Fecha { get; set; }

    public string? Estado { get; set; }

    public virtual ICollection<DetallesPedido>? DetallesPedido { get; set; } = new List<DetallesPedido>();

    public virtual ICollection<Entregas>? Entregas { get; set; } = new List<Entregas>();

    public virtual ICollection<Facturas>? Facturas { get; set; } = new List<Facturas>();

    public virtual Clientes? IdClienteNavigation { get; set; } = null!;

    public virtual Usuarios? IdUsuarioNavigation { get; set; } = null!;
}
