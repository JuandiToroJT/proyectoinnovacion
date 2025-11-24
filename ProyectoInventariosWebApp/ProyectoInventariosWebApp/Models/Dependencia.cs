namespace ProyectoInventariosWebApp.Models
{
    public class Dependencia
    {
        public int IdDependencia { get; set; }
        public int IdSede { get; set; }
        public string Nombre { get; set; } = null!;
        public string TipoDependencia { get; set; } = null!;
        public string? Ubicacion { get; set; }
        public string? Responsable { get; set; }
        public string? TelefonoContacto { get; set; }
        public bool Estado { get; set; }
        public DateTime FechaCreacion { get; set; }

        public Sede? Sede { get; set; }
    }
}