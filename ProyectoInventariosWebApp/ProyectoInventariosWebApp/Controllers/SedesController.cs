using Microsoft.AspNetCore.Mvc;
using ProyectoInventariosWebApp.Models;
using Microsoft.Extensions.Options;
using ProyectoInventariosWebApp.Helpers;
using Newtonsoft.Json;
using System.Linq;

namespace ProyectoInventariosWebApp.Controllers
{
    public class SedesController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiUrl;

        public SedesController(HttpClient httpClient, IOptions<ApiUrlsOptions> apiOptions)
        {
            _httpClient = httpClient;
            _apiUrl = apiOptions.Value.BaseUrl;
        }

        public async Task<IActionResult> Index()
        {
            if (!UsuarioLogueado.Id.HasValue)
            {
                return RedirectToAction("Login", "Account");
            }

            if (UsuarioLogueado.Rol != "SuperAdmin" && UsuarioLogueado.Rol != "AdminSede")
            {
                TempData["Error"] = "No tienes permisos para acceder a esta sección";
                return RedirectToAction("Index", "Home");
            }

            ViewBag.ApiUrl = _apiUrl;

            List<Sede> sedes = new List<Sede>();
            var response = await _httpClient.GetAsync($"{_apiUrl}/Sedes");

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var sedesApi = JsonConvert.DeserializeObject<List<SedeApi>>(content);

                sedes = sedesApi.Select(s => new Sede
                {
                    IdSede = s.IdSede,
                    Nombre = s.Nombre,
                    Codigo = s.Codigo,
                    Direccion = s.Direccion,
                    Ciudad = s.Ciudad,
                    Telefono = s.Telefono,
                    Activa = s.Estado,
                    FechaCreacion = s.FechaCreacion
                }).ToList();
            }

            return View(sedes);
        }

        public async Task<IActionResult> Details(int id)
        {
            if (!UsuarioLogueado.Id.HasValue)
            {
                return RedirectToAction("Login", "Account");
            }

            ViewBag.ApiUrl = _apiUrl;
            ViewBag.IdSede = id;

            Sede sede = null;
            var response = await _httpClient.GetAsync($"{_apiUrl}/Sedes/{id}");

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var sedeApi = JsonConvert.DeserializeObject<SedeApi>(content);

                sede = new Sede
                {
                    IdSede = sedeApi.IdSede,
                    Nombre = sedeApi.Nombre,
                    Codigo = sedeApi.Codigo,
                    Direccion = sedeApi.Direccion,
                    Ciudad = sedeApi.Ciudad,
                    Telefono = sedeApi.Telefono,
                    Activa = sedeApi.Estado,
                    FechaCreacion = sedeApi.FechaCreacion
                };
            }

            if (sede == null)
            {
                return NotFound();
            }

            return View(sede);
        }

        public IActionResult Create()
        {
            if (!UsuarioLogueado.Id.HasValue || UsuarioLogueado.Rol != "SuperAdmin")
            {
                TempData["Error"] = "No tienes permisos para crear sedes";
                return RedirectToAction("Index");
            }

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Sede sede)
        {
            if (!UsuarioLogueado.Id.HasValue || UsuarioLogueado.Rol != "SuperAdmin")
            {
                TempData["Error"] = "No tienes permisos para crear sedes";
                return RedirectToAction("Index");
            }

            if (ModelState.IsValid)
            {
                var sedeApi = new SedeApi
                {
                    Nombre = sede.Nombre,
                    Codigo = sede.Codigo,
                    Direccion = sede.Direccion,
                    Ciudad = sede.Ciudad,
                    Telefono = sede.Telefono,
                    Estado = sede.Activa,
                    FechaCreacion = DateTime.Now
                };

                var response = await _httpClient.PostAsJsonAsync($"{_apiUrl}/Sedes", sedeApi);

                if (response.IsSuccessStatusCode)
                {
                    TempData["Success"] = "Sede creada exitosamente";
                    return RedirectToAction(nameof(Index));
                }

                await ModelState.AddErrorsFromApiResponseAsync(response);
            }

            return View(sede);
        }

        public async Task<IActionResult> Edit(int id)
        {
            if (!UsuarioLogueado.Id.HasValue || UsuarioLogueado.Rol != "SuperAdmin")
            {
                TempData["Error"] = "No tienes permisos para editar sedes";
                return RedirectToAction("Index");
            }

            Sede sede = null;
            var response = await _httpClient.GetAsync($"{_apiUrl}/Sedes/{id}");

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var sedeApi = JsonConvert.DeserializeObject<SedeApi>(content);

                sede = new Sede
                {
                    IdSede = sedeApi.IdSede,
                    Nombre = sedeApi.Nombre,
                    Codigo = sedeApi.Codigo,
                    Direccion = sedeApi.Direccion,
                    Ciudad = sedeApi.Ciudad,
                    Telefono = sedeApi.Telefono,
                    Activa = sedeApi.Estado,
                    FechaCreacion = sedeApi.FechaCreacion
                };
            }

            if (sede == null)
            {
                return NotFound();
            }

            return View(sede);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Sede sede)
        {
            if (!UsuarioLogueado.Id.HasValue || UsuarioLogueado.Rol != "SuperAdmin")
            {
                TempData["Error"] = "No tienes permisos para editar sedes";
                return RedirectToAction("Index");
            }

            if (id != sede.IdSede)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var sedeApi = new SedeApi
                {
                    IdSede = sede.IdSede,
                    Nombre = sede.Nombre,
                    Codigo = sede.Codigo,
                    Direccion = sede.Direccion,
                    Ciudad = sede.Ciudad,
                    Telefono = sede.Telefono,
                    Estado = sede.Activa,
                    FechaCreacion = sede.FechaCreacion
                };

                var response = await _httpClient.PutAsJsonAsync($"{_apiUrl}/Sedes/{id}", sedeApi);

                if (response.IsSuccessStatusCode)
                {
                    TempData["Success"] = "Sede actualizada exitosamente";
                    return RedirectToAction(nameof(Index));
                }

                await ModelState.AddErrorsFromApiResponseAsync(response);
            }

            return View(sede);
        }
    }


    public class SedeApi
    {
        public int IdSede { get; set; }
        public string Nombre { get; set; } = null!;
        public string? Codigo { get; set; }
        public string? Direccion { get; set; }
        public string? Ciudad { get; set; }
        public string? Telefono { get; set; }
        public bool Estado { get; set; }
        public DateTime? FechaCreacion { get; set; }
    }

    public class Sede
    {
        public int IdSede { get; set; }
        public string Nombre { get; set; } = null!;
        public string? Codigo { get; set; }
        public string? Direccion { get; set; }
        public string? Ciudad { get; set; }
        public string? Telefono { get; set; }
        public bool Activa { get; set; }
        public DateTime? FechaCreacion { get; set; }
    }
}
