using System;
using System.Collections.Generic;

namespace ProyectoInventariosWebApi.Models;

public partial class Pedidos
{
    public int IdPedido { get; set; }

    public int IdCliente { get; set; }

    public int? IdUsuario { get; set; } = null;

    public DateTime? Fecha { get; set; }

    public string? Estado { get; set; }

    public int? IdSede { get; set; }

    public int? IdDependencia { get; set; }

    public decimal? Total { get; set; }

    public string? TipoEntrega { get; set; }

    public string? MetodoPago { get; set; }

    public string? Observaciones { get; set; }

    public DateTime? FechaEstimadaEntrega { get; set; }

    public virtual ICollection<DetallesPedido>? DetallesPedido { get; set; } = new List<DetallesPedido>();

    public virtual ICollection<Entregas>? Entregas { get; set; } = new List<Entregas>();

    public virtual ICollection<Facturas>? Facturas { get; set; } = new List<Facturas>();

    public virtual Clientes? IdClienteNavigation { get; set; } = null!;

    public virtual Usuarios? IdUsuarioNavigation { get; set; } = null!;

    public virtual Sedes? IdSedeNavigation { get; set; }

    public virtual Dependencias? IdDependenciaNavigation { get; set; }
}