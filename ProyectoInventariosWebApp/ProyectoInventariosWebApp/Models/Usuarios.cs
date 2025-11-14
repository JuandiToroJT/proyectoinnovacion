using System;
using System.Collections.Generic;

namespace ProyectoInventariosWebApp.Models;

public partial class Usuarios
{
    public int IdUsuario { get; set; }

    public string Nombre { get; set; } = null!;

    public string Correo { get; set; } = null!;

    public string? Contrasena { get; set; } = null!;

    public string Rol { get; set; } = null!;

    public bool Estado { get; set; }

    public virtual ICollection<Pedidos>? Pedidos { get; set; } = new List<Pedidos>();
}
