using Microsoft.AspNetCore.Mvc;
using ProyectoInventariosWebApp.Models;
using Microsoft.Extensions.Options;
using ProyectoInventariosWebApp.Helpers;
using Newtonsoft.Json;
using System.Linq;

namespace ProyectoInventariosWebApp.Controllers
{
    public class DependenciasController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiUrl;

        public DependenciasController(HttpClient httpClient, IOptions<ApiUrlsOptions> apiOptions)
        {
            _httpClient = httpClient;
            _apiUrl = apiOptions.Value.BaseUrl;
        }

        public async Task<IActionResult> Index(int? idSede)
        {
            if (!UsuarioLogueado.Id.HasValue)
            {
                return RedirectToAction("Login", "Account");
            }

            ViewBag.ApiUrl = _apiUrl;

            if (UsuarioLogueado.Rol == "AdminSede" && UsuarioLogueado.IdSede.HasValue)
            {
                idSede = UsuarioLogueado.IdSede.Value;
            }

            ViewBag.IdSede = idSede;

            List<Dependencia> dependencias = new List<Dependencia>();
            string url = $"{_apiUrl}/Dependencias";

            if (idSede.HasValue)
            {
                url = $"{_apiUrl}/Sedes/{idSede.Value}/Dependencias";
            }

            var response = await _httpClient.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var dependenciasApi = JsonConvert.DeserializeObject<List<DependenciaApi>>(content);

                dependencias = dependenciasApi.Select(d => new Dependencia
                {
                    IdDependencia = d.IdDependencia,
                    IdSede = d.IdSede,
                    Nombre = d.Nombre,
                    TipoDependencia = d.TipoDependencia,
                    Ubicacion = d.Ubicacion,
                    Activa = d.Estado,
                    FechaCreacion = d.FechaCreacion,
                    NombreSede = d.IdSedeNavigation?.Nombre
                }).ToList();
            }

            return View(dependencias);
        }

        public async Task<IActionResult> Details(int id)
        {
            if (!UsuarioLogueado.Id.HasValue)
            {
                return RedirectToAction("Login", "Account");
            }

            ViewBag.ApiUrl = _apiUrl;
            ViewBag.IdDependencia = id;

            Dependencia dependencia = null;
            var response = await _httpClient.GetAsync($"{_apiUrl}/Dependencias/{id}");

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var dependenciaCompleta = JsonConvert.DeserializeObject<DependenciaCompleta>(content);

                dependencia = new Dependencia
                {
                    IdDependencia = dependenciaCompleta.IdDependencia,
                    IdSede = dependenciaCompleta.IdSede,
                    Nombre = dependenciaCompleta.Nombre,
                    TipoDependencia = dependenciaCompleta.TipoDependencia,
                    Ubicacion = dependenciaCompleta.Ubicacion,
                    Activa = dependenciaCompleta.Estado,
                    FechaCreacion = dependenciaCompleta.FechaCreacion,
                    NombreSede = dependenciaCompleta.IdSedeNavigation?.Nombre
                };
            }

            if (dependencia == null)
            {
                return NotFound();
            }

            return View(dependencia);
        }

        public async Task<IActionResult> Inventario(int id)
        {
            if (!UsuarioLogueado.Id.HasValue)
            {
                return RedirectToAction("Login", "Account");
            }

            ViewBag.ApiUrl = _apiUrl;
            ViewBag.IdDependencia = id;

            Dependencia dependencia = null;
            var response = await _httpClient.GetAsync($"{_apiUrl}/Dependencias/{id}");

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                dependencia = JsonConvert.DeserializeObject<Dependencia>(content);
            }

            if (dependencia == null)
            {
                return NotFound();
            }

            ViewBag.NombreDependencia = dependencia.Nombre;

            return View();
        }

        public IActionResult Create(int? idSede)
        {
            if (!UsuarioLogueado.Id.HasValue)
            {
                return RedirectToAction("Login", "Account");
            }

            if (UsuarioLogueado.Rol != "SuperAdmin" && UsuarioLogueado.Rol != "AdminSede")
            {
                TempData["Error"] = "No tienes permisos para crear dependencias";
                return RedirectToAction("Index");
            }

            if (UsuarioLogueado.Rol == "AdminSede" && UsuarioLogueado.IdSede.HasValue)
            {
                idSede = UsuarioLogueado.IdSede.Value;
            }

            ViewBag.IdSede = idSede;
            return View(new Dependencia { IdSede = idSede ?? 0 });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Dependencia dependencia)
        {
            if (!UsuarioLogueado.Id.HasValue)
            {
                return RedirectToAction("Login", "Account");
            }

            if (UsuarioLogueado.Rol != "SuperAdmin" && UsuarioLogueado.Rol != "AdminSede")
            {
                TempData["Error"] = "No tienes permisos para crear dependencias";
                return RedirectToAction("Index");
            }

            if (ModelState.IsValid)
            {
                var dependenciaApi = new DependenciaApi
                {
                    IdSede = dependencia.IdSede,
                    Nombre = dependencia.Nombre,
                    TipoDependencia = dependencia.TipoDependencia,
                    Ubicacion = dependencia.Ubicacion,
                    Estado = true,
                    FechaCreacion = DateTime.Now
                };

                var response = await _httpClient.PostAsJsonAsync($"{_apiUrl}/Dependencias", dependenciaApi);

                if (response.IsSuccessStatusCode)
                {
                    TempData["Success"] = "Dependencia creada exitosamente";
                    return RedirectToAction(nameof(Index), new { idSede = dependencia.IdSede });
                }

                await ModelState.AddErrorsFromApiResponseAsync(response);
            }

            ViewBag.IdSede = dependencia.IdSede;
            return View(dependencia);
        }

        public async Task<IActionResult> Edit(int id)
        {
            if (!UsuarioLogueado.Id.HasValue)
            {
                return RedirectToAction("Login", "Account");
            }

            if (UsuarioLogueado.Rol != "SuperAdmin" && UsuarioLogueado.Rol != "AdminSede")
            {
                TempData["Error"] = "No tienes permisos para editar dependencias";
                return RedirectToAction("Index");
            }

            Dependencia dependencia = null;
            var response = await _httpClient.GetAsync($"{_apiUrl}/Dependencias/{id}");

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var dependenciaApi = JsonConvert.DeserializeObject<DependenciaApi>(content);

                dependencia = new Dependencia
                {
                    IdDependencia = dependenciaApi.IdDependencia,
                    IdSede = dependenciaApi.IdSede,
                    Nombre = dependenciaApi.Nombre,
                    TipoDependencia = dependenciaApi.TipoDependencia,
                    Ubicacion = dependenciaApi.Ubicacion,
                    Activa = dependenciaApi.Estado,
                    FechaCreacion = dependenciaApi.FechaCreacion,
                    NombreSede = dependenciaApi.IdSedeNavigation?.Nombre
                };
            }

            if (dependencia == null)
            {
                return NotFound();
            }

            return View(dependencia);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Dependencia dependencia)
        {
            if (!UsuarioLogueado.Id.HasValue)
            {
                return RedirectToAction("Login", "Account");
            }

            if (id != dependencia.IdDependencia)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var dependenciaApi = new DependenciaApi
                {
                    IdDependencia = dependencia.IdDependencia,
                    IdSede = dependencia.IdSede,
                    Nombre = dependencia.Nombre,
                    TipoDependencia = dependencia.TipoDependencia,
                    Ubicacion = dependencia.Ubicacion,
                    Estado = dependencia.Activa,
                    FechaCreacion = dependencia.FechaCreacion
                };

                var response = await _httpClient.PutAsJsonAsync($"{_apiUrl}/Dependencias/{id}", dependenciaApi);

                if (response.IsSuccessStatusCode)
                {
                    TempData["Success"] = "Dependencia actualizada exitosamente";
                    return RedirectToAction(nameof(Index), new { idSede = dependencia.IdSede });
                }

                await ModelState.AddErrorsFromApiResponseAsync(response);
            }

            return View(dependencia);
        }
    }

    public class DependenciaApi
    {
        public int IdDependencia { get; set; }
        public int IdSede { get; set; }
        public string Nombre { get; set; } = null!;
        public string? TipoDependencia { get; set; }
        public string? Ubicacion { get; set; }
        public bool Estado { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public SedeNavigation? IdSedeNavigation { get; set; }
    }

    public class Dependencia
    {
        public int IdDependencia { get; set; }
        public int IdSede { get; set; }
        public string Nombre { get; set; } = null!;
        public string? TipoDependencia { get; set; }
        public string? Ubicacion { get; set; }
        public string? Responsable { get; set; }
        public string? Telefono { get; set; }
        public bool Activa { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public string? NombreSede { get; set; }
    }

    public class DependenciaCompleta
    {
        public int IdDependencia { get; set; }
        public int IdSede { get; set; }
        public string Nombre { get; set; } = null!;
        public string? TipoDependencia { get; set; }
        public string? Ubicacion { get; set; }
        public bool Estado { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public SedeNavigation? IdSedeNavigation { get; set; }
    }

    public class SedeNavigation
    {
        public int IdSede { get; set; }
        public string Nombre { get; set; } = null!;
    }
}
