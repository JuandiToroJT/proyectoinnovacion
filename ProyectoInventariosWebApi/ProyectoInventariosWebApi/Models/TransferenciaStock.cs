using System;
using System.Collections.Generic;

namespace ProyectoInventariosWebApi.Models;

public partial class TransferenciaStock
{
    public int IdTransferencia { get; set; }

    public int IdProducto { get; set; }

    public int IdDependenciaOrigen { get; set; }

    public int IdDependenciaDestino { get; set; }

    public int Cantidad { get; set; }

    public string Motivo { get; set; } = null!;

    public DateTime FechaSolicitud { get; set; }

    public DateTime? FechaAprobacion { get; set; }

    public DateTime? FechaEjecucion { get; set; }

    public int IdUsuarioSolicita { get; set; }

    public int? IdUsuarioAprueba { get; set; }

    public string Estado { get; set; } = null!;

    public string? Observaciones { get; set; }

    public decimal? CostoTransporte { get; set; }

    public virtual Productos? IdProductoNavigation { get; set; } = null!;

    public virtual Dependencias? IdDependenciaOrigenNavigation { get; set; } = null!;

    public virtual Dependencias? IdDependenciaDestinoNavigation { get; set; } = null!;

    public virtual Usuarios? IdUsuarioSolicitaNavigation { get; set; } = null!;

    public virtual Usuarios? IdUsuarioApruebaNavigation { get; set; } = null!;
}