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
        private readonly string BASE_URL;

        public HomeController(HttpClient httpClient, IOptions<ApiUrlsOptions> apiOptions)
        {
            _httpClient = httpClient;
            URL_API = apiOptions.Value.BaseUrl + "/Usuarios";
            BASE_URL = apiOptions.Value.BaseUrl;
        }

        public IActionResult Index()
        {
            if (!UsuarioLogueado.Id.HasValue)
            {
                return RedirectToAction("Login", "Account");
            }

            ViewBag.ApiUrl = BASE_URL;
            return View();
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

            return RedirectToAction("Login", "Account");
        }
    }
}