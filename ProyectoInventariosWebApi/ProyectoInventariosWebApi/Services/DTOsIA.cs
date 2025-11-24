namespace ProyectoInventariosWebApi.Services
{
    public class VentaAIDto
    {
        public string Fecha { get; set; }
        public string Sede { get; set; }
        public string Dependencia { get; set; }
        public string TipoCliente { get; set; }
        public decimal TotalVenta { get; set; }
        public List<string> Productos { get; set; } = new();
    }
    public class StockAIDto
    {
        public string Producto { get; set; }
        public string Categoria { get; set; }
        public string Ubicacion { get; set; }
        public int Actual { get; set; }
        public int Minimo { get; set; }
        public int Maximo { get; set; }
        public decimal CostoPromedio { get; set; }
    }

    public class MovimientoAIDto
    {
        public string Fecha { get; set; }
        public string Tipo { get; set; }
        public int Cantidad { get; set; }
    }

    public class AnalisisVentasSalidaDto
    {
        public List<string>? HallazgosClave { get; set; }
        public List<string>? Recomendaciones { get; set; }
    }

    public class ItemCriticoSalidaDto
    {
        public string? Producto { get; set; }
        public string? Sede { get; set; }
        public int CantidadComprar { get; set; }
        public decimal CostoEstimado { get; set; }
        public string? Justificacion { get; set; }
    }

    public class StockSalidaDto
    {
        public decimal ResumenCosto { get; set; }
        public List<ItemCriticoSalidaDto>? ItemsCriticos { get; set; }
    }

    public class RecomendacionSalidaDto
    {
        public List<string>? ProductosSugeridos { get; set; }
        public string? FraseVenta { get; set; }
    }
}
