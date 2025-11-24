using System;
using System.Collections.Generic;

namespace ProyectoInventariosWebApi.Models;

public partial class Dependencias
{
    public int IdDependencia { get; set; }

    public int IdSede { get; set; }

    public string Nombre { get; set; } = null!;

    public string TipoDependencia { get; set; } = null!;

    public string? Ubicacion { get; set; }

    public string? Responsable { get; set; }

    public string? TelefonoContacto { get; set; }

    public bool Estado { get; set; }

    public DateTime FechaCreacion { get; set; }

    public virtual Sedes? IdSedeNavigation { get; set; } = null!;

    public virtual ICollection<Usuarios> Usuarios { get; set; } = new List<Usuarios>();

    public virtual ICollection<InventarioDependencia> InventarioDependencia { get; set; } = new List<InventarioDependencia>();

    public virtual ICollection<TransferenciaStock> TransferenciasOrigen { get; set; } = new List<TransferenciaStock>();

    public virtual ICollection<TransferenciaStock> TransferenciasDestino { get; set; } = new List<TransferenciaStock>();

    public virtual ICollection<Pedidos> Pedidos { get; set; } = new List<Pedidos>();
}