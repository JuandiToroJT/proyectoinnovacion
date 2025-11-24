namespace ProyectoInventariosWebApp.Models
{
    public class Sede
    {
        public int IdSede { get; set; }
        public int IdEmpresa { get; set; }
        public string Nombre { get; set; } = null!;
        public string Codigo { get; set; } = null!;
        public string Direccion { get; set; } = null!;
        public string? Telefono { get; set; }
        public string? HorarioLaboral { get; set; }
        public bool EsSedePrincipal { get; set; }
        public bool Estado { get; set; }
        public DateTime FechaCreacion { get; set; }

        public List<Dependencia>? Dependencias { get; set; }
    }
}