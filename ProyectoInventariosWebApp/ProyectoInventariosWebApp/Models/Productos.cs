using System;
using System.Collections.Generic;

namespace ProyectoInventariosWebApp.Models;

public partial class Productos
{
    public int IdProducto { get; set; }

    public string Nombre { get; set; } = null!;

    public string Descripcion { get; set; } = null!;

    public decimal Precio { get; set; }

    public int Stock { get; set; }

    public virtual ICollection<DetallesPedido>? DetallesPedido { get; set; } = new List<DetallesPedido>();
}
