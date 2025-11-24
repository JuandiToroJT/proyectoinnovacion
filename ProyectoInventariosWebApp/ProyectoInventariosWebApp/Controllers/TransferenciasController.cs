using Microsoft.AspNetCore.Mvc;
using ProyectoInventariosWebApp.Models;
using Microsoft.Extensions.Options;
using ProyectoInventariosWebApp.Helpers;

namespace ProyectoInventariosWebApp.Controllers
{
    public class TransferenciasController : Controller
    {
        private readonly string _apiUrl;

        public TransferenciasController(IOptions<ApiUrlsOptions> apiOptions)
        {
            _apiUrl = apiOptions.Value.BaseUrl;
        }

        public IActionResult Index()
        {
            if (!UsuarioLogueado.Id.HasValue && string.IsNullOrEmpty(HttpContext.Session.GetString("IdUsuario")))
            {
                return RedirectToAction("Login", "Account");
            }

            var rol = UsuarioLogueado.Rol ?? HttpContext.Session.GetString("Rol");
            if (rol != "SuperAdmin" && rol != "AdminSede" && rol != "EncargadoDependencia" && rol != "Administrador")
            {
                TempData["Error"] = "No tienes permisos para acceder a esta sección";
                return RedirectToAction("Index", "Home");
            }

            ViewBag.ApiUrl = _apiUrl;
            return View();
        }
    }
}