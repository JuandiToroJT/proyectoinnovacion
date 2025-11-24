using System;
using System.Collections.Generic;

namespace ProyectoInventariosWebApi.Models;

public partial class Productos
{
    public int IdProducto { get; set; }

    public string Nombre { get; set; } = null!;

    public string Descripcion { get; set; } = null!;

    public decimal Precio { get; set; }

    public int Stock { get; set; }

    public string? Codigo { get; set; }

    public string? UnidadMedida { get; set; }

    public string? Categoria { get; set; }

    public bool EsCompartible { get; set; }

    public int? StockMinimoGlobal { get; set; }

    public string? Imagen { get; set; }

    public bool? RequiereRefrigeracion { get; set; }

    public int? DiasVidaUtil { get; set; }

    public bool Estado { get; set; }

    public DateTime? FechaCreacion { get; set; }

    public virtual ICollection<DetallesPedido>? DetallesPedido { get; set; } = new List<DetallesPedido>();

    public virtual ICollection<InventarioDependencia> InventarioDependencia { get; set; } = new List<InventarioDependencia>();

    public virtual ICollection<TransferenciaStock> TransferenciaStock { get; set; } = new List<TransferenciaStock>();
}