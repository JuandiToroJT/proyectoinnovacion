using System;
using System.Collections.Generic;

namespace ProyectoInventariosWebApp.Models;

public partial class Clientes
{
    public int IdCliente { get; set; }

    public string Nombre { get; set; } = null!;

    public string Telefono { get; set; } = null!;

    public string Direccion { get; set; } = null!;

    public virtual ICollection<Pedidos>? Pedidos { get; set; } = new List<Pedidos>();
}
