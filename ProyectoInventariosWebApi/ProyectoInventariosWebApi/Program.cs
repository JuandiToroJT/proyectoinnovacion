using ProyectoInventariosWebApi.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ProyectoInventariosWebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddDbContext<ProyectoInventariosDbContext>();

            var app = builder.Build();

            using (var scope = app.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<ProyectoInventariosDbContext>();

                if (!context.Usuarios.Any())
                {
                    var hasher = new PasswordHasher<Usuarios>();

                    var admin = new Usuarios
                    {
                        Nombre = "Admin",
                        Correo = "admin@ucaldas.com",
                        Rol = "Administrador",
                        Estado = true
                    };
                    admin.Contrasena = hasher.HashPassword(admin, "admin123");

                    context.Usuarios.Add(admin);
                    context.SaveChanges();
                }

                if (!context.Empresas.Any())
                {
                    Empresas empresa = new Empresas
                    {
                        Nombre = "Tienda de la UCALDAS",
                        Nit = "900123456-7",
                        Ciudad = "Manizales",
                        Direccion = "Cra 23 #45-67",
                        Telefono = "3001234567",
                        EmailContacto = "contacto@ucaldas.com",
                        PaginaWeb = "https://tiendaucaldas.com",
                        RepresentanteLegal = "Juan Pérez",
                        TipoEmpresa = "SAS",
                        FechaCreacion = DateTime.Now
                    };

                    context.Empresas.Add(empresa);
                    context.SaveChanges();
                }
            }

            // Configure the HTTP request pipeline.
            app.UseSwagger();
            app.UseSwaggerUI();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
