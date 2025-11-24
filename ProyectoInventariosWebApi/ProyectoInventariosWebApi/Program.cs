using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ProyectoInventariosWebApi.Models;
using ProyectoInventariosWebApi.Services;
using System.Text.Json.Serialization;

namespace ProyectoInventariosWebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            var appSettings = builder.Configuration.Get<AppSettings>();

            var connectionString = appSettings?.ConnectionStrings.DefaultConnection;

            if (string.IsNullOrEmpty(connectionString))
            {
                throw new InvalidOperationException("La cadena de conexión 'DefaultConnection' no está configurada en appsettings.");
            }

            builder.Services.AddControllers()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;

                    options.JsonSerializerOptions.WriteIndented = true;

                    options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
                });

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddDbContext<ProyectoInventariosDbContext>(options => options.UseSqlServer(connectionString));
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowFrontend",
                    policy =>
                    {
                        policy.WithOrigins("https://localhost:44360")
                              .AllowAnyHeader()
                              .AllowAnyMethod();
                    });
            });

            var geminiApiKey = appSettings?.Gemini.ApiKey;

            if (string.IsNullOrEmpty(geminiApiKey))
            {
                throw new InvalidOperationException("La clave API de Gemini no está configurada.");
            }

            builder.Services.AddScoped<AIGeminiService>(provider =>
            {
                return new AIGeminiService(geminiApiKey);
            });

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
                        Rol = "SuperAdmin",
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

            app.UseSwagger();
            app.UseSwaggerUI();
            app.UseCors("AllowFrontend");

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}