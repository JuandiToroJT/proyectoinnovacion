using System;
using System.Collections.Generic;

namespace ProyectoInventariosWebApi.Models;


public partial class Sedes
{
    public int IdSede { get; set; }

    public int IdEmpresa { get; set; }

    public string Nombre { get; set; } = null!;

    public string Codigo { get; set; } = null!;

    public string Direccion { get; set; } = null!;

    public string? Telefono { get; set; }

    public string? HorarioLaboral { get; set; }

    public bool EsSedePrincipal { get; set; }

    public bool Estado { get; set; }

    public DateTime FechaCreacion { get; set; }

    public virtual Empresas? IdEmpresaNavigation { get; set; } = null!;

    public virtual ICollection<Dependencias> Dependencias { get; set; } = new List<Dependencias>();

    public virtual ICollection<Usuarios> Usuarios { get; set; } = new List<Usuarios>();

    public virtual ICollection<Clientes> Clientes { get; set; } = new List<Clientes>();

    public virtual ICollection<Pedidos> Pedidos { get; set; } = new List<Pedidos>();
}