using System;
using System.Collections.Generic;

namespace ProyectoInventariosWebApi.Models;

public partial class InventarioDependencia
{
    public int IdInventario { get; set; }

    public int IdProducto { get; set; }

    public int IdDependencia { get; set; }

    public int StockActual { get; set; }

    public int StockMinimo { get; set; }

    public int StockMaximo { get; set; }

    public int PuntoReorden { get; set; }

    public decimal CostoPromedio { get; set; }

    public string? Ubicacion { get; set; }

    public DateTime UltimaActualizacion { get; set; }

    public string EstadoInventario { get; set; } = null!;

    public virtual Productos? IdProductoNavigation { get; set; } = null!;

    public virtual Dependencias? IdDependenciaNavigation { get; set; } = null!;

    public virtual ICollection<MovimientoInventario> MovimientoInventario { get; set; } = new List<MovimientoInventario>();

    public virtual ICollection<DetallesPedido> DetallesPedido { get; set; } = new List<DetallesPedido>();
}