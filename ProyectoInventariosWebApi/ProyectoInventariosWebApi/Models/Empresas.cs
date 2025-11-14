using System;
using System.Collections.Generic;

namespace ProyectoInventariosWebApi.Models;

public partial class Empresas
{
    public int IdEmpresa { get; set; }

    public string Nombre { get; set; } = null!;

    public string? Direccion { get; set; }

    public string? Telefono { get; set; }

    public string? Nit { get; set; }

    public string? Ciudad { get; set; }

    public DateTime? FechaCreacion { get; set; }

    public string? RepresentanteLegal { get; set; }

    public string? TipoEmpresa { get; set; }

    public string? PaginaWeb { get; set; }

    public string? EmailContacto { get; set; }
}
