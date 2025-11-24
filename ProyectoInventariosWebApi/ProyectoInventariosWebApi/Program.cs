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
                    var empresa = new Empresas
                    {
                        Nombre = "Universidad de Caldas",
                        Nit = "890800640-1",
                        Ciudad = "Manizales",
                        Direccion = "Calle 65 No 26-10",
                        Telefono = "6068781500",
                        EmailContacto = "rectoría@ucaldas.edu.co",
                        PaginaWeb = "https://ucaldas.edu.com",
                        RepresentanteLegal = "Dr. Fabio Hernando Arias Orozco",
                        TipoEmpresa = "Universidad Pública",
                        FechaCreacion = DateTime.Now
                    };
                    context.Empresas.Add(empresa);
                    context.SaveChanges();

                    var sede = new Sedes
                    {
                        IdEmpresa = 1,
                        Nombre = "Sede Principal",
                        Codigo = "PRINCIPAL",
                        Direccion = "Calle 65 No 26-10",
                        Telefono = "6068781500",
                        HorarioLaboral = "Lunes a Jueves: 7:45am-11:45am / 1:45pm-5:45pm | Viernes: 7:00am-3:30pm",
                        EsSedePrincipal = true,
                        Estado = true,
                        FechaCreacion = DateTime.Now
                    };
                    context.Sedes.Add(sede);
                    context.SaveChanges();

                    var dependencia = new Dependencias
                    {
                        IdSede = 1,
                        Nombre = "Papelería Sede Principal",
                        TipoDependencia = "Papelería",
                        Ubicacion = "Edificio Administrativo - Piso 1",
                        Responsable = "María González",
                        TelefonoContacto = "3001234567",
                        Estado = true,
                        FechaCreacion = DateTime.Now
                    };
                    context.Dependencias.Add(dependencia);
                    context.SaveChanges();

                    var hasher = new PasswordHasher<Usuarios>();

                    var admin = new Usuarios
                    {
                        Nombre = "Administrador del Sistema",
                        Correo = "admin@ucaldas.edu.co",
                        Rol = "SuperAdmin",
                        Estado = true,
                        IdSede = null,
                        IdDependencia = null,
                        FechaCreacion = DateTime.Now
                    };
                    admin.Contrasena = hasher.HashPassword(admin, "admin123");
                    context.Usuarios.Add(admin);

                    var adminSede = new Usuarios
                    {
                        Nombre = "Coordinador Sede Principal",
                        Correo = "coord.principal@ucaldas.edu.co",
                        Rol = "AdminSede",
                        Estado = true,
                        IdSede = 1,
                        IdDependencia = null,
                        FechaCreacion = DateTime.Now
                    };
                    adminSede.Contrasena = hasher.HashPassword(adminSede, "admin123");
                    context.Usuarios.Add(adminSede);

                    var encargado = new Usuarios
                    {
                        Nombre = "María González",
                        Correo = "maria.gonzalez@ucaldas.edu.co",
                        Rol = "EncargadoDependencia",
                        Estado = true,
                        IdSede = 1,
                        IdDependencia = 1,
                        FechaCreacion = DateTime.Now
                    };
                    encargado.Contrasena = hasher.HashPassword(encargado, "admin123");
                    context.Usuarios.Add(encargado);

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