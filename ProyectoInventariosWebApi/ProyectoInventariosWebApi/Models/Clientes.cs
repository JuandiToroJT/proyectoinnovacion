using System;
using System.Collections.Generic;

namespace ProyectoInventariosWebApi.Models;

public partial class Clientes
{
    public int IdCliente { get; set; }

    public string Nombre { get; set; } = null!;

    public string Telefono { get; set; } = null!;

    public string Direccion { get; set; } = null!;

    public string? Email { get; set; }

    public int? IdSedePredeterminada { get; set; }

    public string? TipoCliente { get; set; }

    public string? DocumentoIdentidad { get; set; }

    public DateTime? FechaRegistro { get; set; }

    public bool Estado { get; set; }

    public virtual ICollection<Pedidos>? Pedidos { get; set; } = new List<Pedidos>();

    public virtual Sedes? IdSedePredeterminadaNavigation { get; set; }
}