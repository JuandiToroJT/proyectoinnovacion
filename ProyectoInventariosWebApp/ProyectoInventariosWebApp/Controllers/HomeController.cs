using System.Diagnostics;
using ProyectoInventariosWebApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ProyectoInventariosWebApp.Filtro;
using Newtonsoft.Json;
using ProyectoInventariosWebApp.Helpers;
using Microsoft.Extensions.Options;

namespace ProyectoInventariosWebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly string URL_API;

        public HomeController(HttpClient httpClient, IOptions<ApiUrlsOptions> apiOptions)
        {
            _httpClient = httpClient;
            URL_API = apiOptions.Value.BaseUrl + "Usuarios";
        }

        public IActionResult Index()
        {
            if (UsuarioLogueado.Id.HasValue)
                return View();
            else
                return RedirectToAction("Login");
        }

        public IActionResult Login()
        {
            UsuarioLogueado.Id = null;
            UsuarioLogueado.Nombre = null;
            UsuarioLogueado.Correo = null;
            UsuarioLogueado.Rol = null;

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var usuarios = new List<Usuarios>();

                var respuesta = await _httpClient.GetAsync(URL_API);
                if (respuesta.IsSuccessStatusCode)
                {
                    var content = await respuesta.Content.ReadAsStringAsync();
                    usuarios = JsonConvert.DeserializeObject<List<Usuarios>>(content);
                }

                var usuario = usuarios
                    .FirstOrDefault(u => u.Correo == model.Correo && u.Estado == true);

                if (usuario == null)
                {
                    ModelState.AddModelError("", "Usuario no encontrado o inactivo.");
                    return View(model);
                }

                var hasher = new PasswordHasher<Usuarios>();
                var result = hasher.VerifyHashedPassword(usuario, usuario.Contrasena, model.Contrasena);

                if (result == PasswordVerificationResult.Failed)
                {
                    ModelState.AddModelError("", "Contraseña incorrecta.");
                    return View(model);
                }

                UsuarioLogueado.Id = usuario.IdUsuario;
                UsuarioLogueado.Nombre = usuario.Nombre;
                UsuarioLogueado.Correo = usuario.Correo;
                UsuarioLogueado.Rol = usuario.Rol;

                return RedirectToAction("Index");
            }

            return View(model);
        }

        public IActionResult Logout()
        {
            return RedirectToAction("Login");
        }
    }
}