namespace ProyectoInventariosWebApp.Models
{
    public static class UsuarioLogueado
    {
        public static int? Id { get; set; } = null;

        public static string Nombre { get; set; } = null!;

        public static string Correo { get; set; } = null!;

        public static string Rol { get; set; } = null!;
    }
}
