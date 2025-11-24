namespace ProyectoInventariosWebApp.Models
{
    public class Usuario
    {
        public int IdUsuario { get; set; }
        public string Nombre { get; set; } = null!;
        public string Correo { get; set; } = null!;
        public string? Contrasena { get; set; }
        public string Rol { get; set; } = null!;
        public bool Estado { get; set; }

        public int? IdSede { get; set; }
        public int? IdDependencia { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public DateTime? UltimoAcceso { get; set; }

        public SedeInfo? Sede { get; set; }
        public DependenciaInfo? Dependencia { get; set; }
    }

    public class SedeInfo
    {
        public int IdSede { get; set; }
        public string Nombre { get; set; } = null!;
        public string? Codigo { get; set; }
    }

    public class DependenciaInfo
    {
        public int IdDependencia { get; set; }
        public string Nombre { get; set; } = null!;
        public string? TipoDependencia { get; set; }
    }
}