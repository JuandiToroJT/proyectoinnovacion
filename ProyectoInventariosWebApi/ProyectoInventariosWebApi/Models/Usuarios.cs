using System;
using System.Collections.Generic;

namespace ProyectoInventariosWebApi.Models;

public partial class Usuarios
{
    public int IdUsuario { get; set; }

    public string Nombre { get; set; } = null!;

    public string Correo { get; set; } = null!;

    public string? Contrasena { get; set; } = null!;

    public string Rol { get; set; } = null!;

    public bool Estado { get; set; }

    public int? IdSede { get; set; }

    public int? IdDependencia { get; set; }

    public DateTime? FechaCreacion { get; set; }

    public DateTime? UltimoAcceso { get; set; }

    public virtual ICollection<Pedidos>? Pedidos { get; set; } = new List<Pedidos>();

    public virtual Sedes? IdSedeNavigation { get; set; }

    public virtual Dependencias? IdDependenciaNavigation { get; set; }

    public virtual ICollection<TransferenciaStock> TransferenciasSolicitadas { get; set; } = new List<TransferenciaStock>();

    public virtual ICollection<TransferenciaStock> TransferenciasAprobadas { get; set; } = new List<TransferenciaStock>();

    public virtual ICollection<MovimientoInventario> MovimientoInventario { get; set; } = new List<MovimientoInventario>();
}