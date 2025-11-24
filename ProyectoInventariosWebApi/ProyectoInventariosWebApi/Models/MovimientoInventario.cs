using System;
using System.Collections.Generic;

namespace ProyectoInventariosWebApi.Models;

/// <summary>
/// Registra TODOS los movimientos de inventario para auditoría
/// Cada cambio en el stock genera un registro aquí
/// </summary>
public partial class MovimientoInventario
{
    public int IdMovimiento { get; set; }

    public int IdInventario { get; set; }

    public string TipoMovimiento { get; set; } = null!;

    public int Cantidad { get; set; }

    public int StockAnterior { get; set; }

    public int StockNuevo { get; set; }

    public DateTime Fecha { get; set; }

    public int IdUsuario { get; set; }

    public string? TipoReferencia { get; set; }

    public int? IdReferencia { get; set; }

    public string? Observaciones { get; set; }

    public decimal? CostoUnitario { get; set; }

    public virtual InventarioDependencia? IdInventarioNavigation { get; set; } = null!;

    public virtual Usuarios? IdUsuarioNavigation { get; set; } = null!;
}