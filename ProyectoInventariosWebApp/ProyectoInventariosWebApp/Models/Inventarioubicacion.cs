namespace ProyectoInventariosWebApp.Models
{
    public class InventarioUbicacion
    {
        public int IdInventario { get; set; }
        public string Sede { get; set; } = null!;
        public string Dependencia { get; set; } = null!;
        public string TipoDependencia { get; set; } = null!;
        public int StockActual { get; set; }
        public int StockMinimo { get; set; }
        public string EstadoInventario { get; set; } = null!;
        public string? Ubicacion { get; set; }
        public bool StockBajo { get; set; }
    }

    public class ProductoConInventario
    {
        public int IdProducto { get; set; }
        public string? Codigo { get; set; }
        public string Nombre { get; set; } = null!;
        public string? Descripcion { get; set; }
        public decimal Precio { get; set; }
        public string? UnidadMedida { get; set; }
        public string? Categoria { get; set; }
        public bool EsCompartible { get; set; }
        public bool Estado { get; set; }
        public int StockTotal { get; set; }
        public List<InventarioUbicacion>? Inventario { get; set; }
    }
}