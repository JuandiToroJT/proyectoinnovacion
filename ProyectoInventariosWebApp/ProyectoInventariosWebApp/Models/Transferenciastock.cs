namespace ProyectoInventariosWebApp.Models
{
    public class TransferenciaStock
    {
        public int IdTransferencia { get; set; }
        public ProductoInfo Producto { get; set; } = null!;
        public UbicacionInfo Origen { get; set; } = null!;
        public UbicacionInfo Destino { get; set; } = null!;
        public int Cantidad { get; set; }
        public string Motivo { get; set; } = null!;
        public string Estado { get; set; } = null!;
        public DateTime FechaSolicitud { get; set; }
        public DateTime? FechaAprobacion { get; set; }
        public DateTime? FechaEjecucion { get; set; }
        public string UsuarioSolicita { get; set; } = null!;
        public string? UsuarioAprueba { get; set; }
        public string? Observaciones { get; set; }
        public decimal? CostoTransporte { get; set; }
    }

    public class ProductoInfo
    {
        public int IdProducto { get; set; }
        public string? Codigo { get; set; }
        public string Nombre { get; set; } = null!;
    }

    public class UbicacionInfo
    {
        public int IdDependencia { get; set; }
        public string Dependencia { get; set; } = null!;
        public string Sede { get; set; } = null!;
    }

    public class SolicitarTransferenciaDto
    {
        public int IdProducto { get; set; }
        public int IdDependenciaOrigen { get; set; }
        public int IdDependenciaDestino { get; set; }
        public int Cantidad { get; set; }
        public string Motivo { get; set; } = null!;
        public int IdUsuarioSolicita { get; set; }
        public string? Observaciones { get; set; }
        public decimal? CostoTransporte { get; set; }
    }

    public class AprobacionTransferenciaDto
    {
        public int IdUsuarioAprueba { get; set; }
        public string? Observaciones { get; set; }
    }
}