using Microsoft.AspNetCore.Mvc;
using ProyectoInventariosWebApp.Models;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Options;
using ProyectoInventariosWebApp.Helpers;

namespace ProyectoInventariosWebApp.Controllers
{
    public class AccountController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiUrl;

        public AccountController(HttpClient httpClient, IOptions<ApiUrlsOptions> apiOptions)
        {
            _httpClient = httpClient;
            _apiUrl = apiOptions.Value.BaseUrl;
        }

        [HttpGet]
        public IActionResult Login()
        {
            if (UsuarioLogueado.Id.HasValue || !string.IsNullOrEmpty(HttpContext.Session.GetString("IdUsuario")))
            {
                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(Usuario usuario)
        {
            try
            {
                Console.WriteLine("👉 POST Login ejecutado");
                var loginData = new
                {
                    correo = usuario.Correo,
                    contrasena = usuario.Contrasena
                };

                var json = JsonSerializer.Serialize(loginData);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync($"{_apiUrl}/Usuarios/Login", content);

                if (response.IsSuccessStatusCode)
                {
                    var responseData = await response.Content.ReadAsStringAsync();
                    var options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    };
                    var usuarioData = JsonSerializer.Deserialize<Usuario>(responseData, options);

                    if (usuarioData != null)
                    {
                        UsuarioLogueado.Id = usuarioData.IdUsuario;
                        UsuarioLogueado.Nombre = usuarioData.Nombre;
                        UsuarioLogueado.Correo = usuarioData.Correo;
                        UsuarioLogueado.Rol = usuarioData.Rol;
                        UsuarioLogueado.IdSede = usuarioData.IdSede;
                        UsuarioLogueado.IdDependencia = usuarioData.IdDependencia;

                        HttpContext.Session.SetString("IdUsuario", usuarioData.IdUsuario.ToString());
                        HttpContext.Session.SetString("Usuario", usuarioData.Nombre);
                        HttpContext.Session.SetString("Correo", usuarioData.Correo);
                        HttpContext.Session.SetString("Rol", usuarioData.Rol);

                        if (usuarioData.Sede != null)
                        {
                            UsuarioLogueado.NombreSede = usuarioData.Sede.Nombre;
                            HttpContext.Session.SetString("IdSede", usuarioData.Sede.IdSede.ToString());
                            HttpContext.Session.SetString("NombreSede", usuarioData.Sede.Nombre);
                            if (!string.IsNullOrEmpty(usuarioData.Sede.Codigo))
                            {
                                HttpContext.Session.SetString("CodigoSede", usuarioData.Sede.Codigo);
                            }
                        }

                        if (usuarioData.Dependencia != null)
                        {
                            UsuarioLogueado.NombreDependencia = usuarioData.Dependencia.Nombre;
                            HttpContext.Session.SetString("IdDependencia", usuarioData.Dependencia.IdDependencia.ToString());
                            HttpContext.Session.SetString("NombreDependencia", usuarioData.Dependencia.Nombre);
                            if (!string.IsNullOrEmpty(usuarioData.Dependencia.TipoDependencia))
                            {
                                HttpContext.Session.SetString("TipoDependencia", usuarioData.Dependencia.TipoDependencia);
                            }
                        }

                        return RedirectToAction("Index", "Home");
                    }
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    try
                    {
                        var errorObj = JsonSerializer.Deserialize<Dictionary<string, string>>(errorContent);
                        ViewBag.Error = errorObj?["message"] ?? "Credenciales incorrectas";
                    }
                    catch
                    {
                        ViewBag.Error = "Credenciales incorrectas";
                    }
                }
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Error al conectar con el servidor: " + ex.Message;
            }

            return View(usuario);
        }

        public IActionResult Logout()
        {
            UsuarioLogueado.Id = null;
            UsuarioLogueado.Nombre = null;
            UsuarioLogueado.Correo = null;
            UsuarioLogueado.Rol = null;
            UsuarioLogueado.IdSede = null;
            UsuarioLogueado.NombreSede = null;
            UsuarioLogueado.IdDependencia = null;
            UsuarioLogueado.NombreDependencia = null;

            HttpContext.Session.Clear();

            return RedirectToAction("Login");
        }
    }
}