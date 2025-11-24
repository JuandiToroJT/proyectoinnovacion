using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ProyectoInventariosWebApp.Models;
using ProyectoInventariosWebApp.Filtro;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using ProyectoInventariosWebApp.Helpers;
using Microsoft.Extensions.Options;

namespace ProyectoInventariosWebApp.Controllers
{
    [AutenticadoAdministrador]
    public class UsuariosController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiUrl;
        private readonly string _baseUrl;

        public UsuariosController(HttpClient httpClient, IOptions<ApiUrlsOptions> apiOptions)
        {
            _httpClient = httpClient;

            _baseUrl = apiOptions.Value.BaseUrl;
            _apiUrl = _baseUrl + "/Usuarios";
        }

        public async Task<IActionResult> Index()
        {
            ViewBag.ApiUrl = _baseUrl;
            return View(await ObtenerListadoUsuarios());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Usuarios usuarios = await ObtenerUsuarioXId(id.Value);
            if (usuarios == null)
            {
                return NotFound();
            }

            return View(usuarios);
        }

        public async Task<IActionResult> Create()
        {
            await CargarListasSedes();
            await CargarListasDependencias();

            ViewBag.Roles = ObtenerRoles();

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Nombre,Correo,Contrasena,Rol,IdSede,IdDependencia")] Usuarios usuarios, string ConfirmarContrasena)
        {
            if (ModelState.IsValid)
            {
                if (usuarios.Contrasena != ConfirmarContrasena)
                {
                    ModelState.AddModelError("Contrasena", "Las contraseñas no coinciden.");
                    await CargarListasSedes();
                    await CargarListasDependencias();
                    ViewBag.Roles = ObtenerRoles();
                    return View(usuarios);
                }

                usuarios.Estado = true;
                usuarios.FechaCreacion = DateTime.Now;

                var hasher = new PasswordHasher<Usuarios>();
                usuarios.Contrasena = hasher.HashPassword(usuarios, usuarios.Contrasena);

                var usuarioDto = new
                {
                    nombre = usuarios.Nombre,
                    correo = usuarios.Correo,
                    contrasena = usuarios.Contrasena,
                    rol = usuarios.Rol,
                    idSede = usuarios.IdSede,
                    idDependencia = usuarios.IdDependencia,
                    estado = usuarios.Estado,
                    fechaCreacion = usuarios.FechaCreacion
                };

                var respuesta = await _httpClient.PostAsJsonAsync(_apiUrl, usuarioDto);
                if (respuesta.IsSuccessStatusCode)
                {
                    TempData["Success"] = "Usuario creado exitosamente";
                    return RedirectToAction(nameof(Index));
                }

                await ModelState.AddErrorsFromApiResponseAsync(respuesta);
            }

            await CargarListasSedes();
            await CargarListasDependencias();
            ViewBag.Roles = ObtenerRoles();
            return View(usuarios);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Usuarios usuarios = await ObtenerUsuarioXId(id.Value);
            if (usuarios == null)
            {
                return NotFound();
            }

            await CargarListasSedes();
            await CargarListasDependencias();
            ViewBag.Roles = ObtenerRoles();

            return View(usuarios);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdUsuario,Nombre,Correo,Contrasena,Rol,Estado,IdSede,IdDependencia")] Usuarios usuarios, string ConfirmarContrasena = "")
        {
            if (id != usuarios.IdUsuario)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var original = await ObtenerUsuarioXId(id);
                if (original == null)
                {
                    return NotFound();
                }

                if (!string.IsNullOrEmpty(usuarios.Contrasena))
                {
                    if (usuarios.Contrasena != ConfirmarContrasena)
                    {
                        ModelState.AddModelError("Contrasena", "Las contraseñas no coinciden.");
                        await CargarListasSedes();
                        await CargarListasDependencias();
                        ViewBag.Roles = ObtenerRoles();
                        return View(usuarios);
                    }

                    var hasher = new PasswordHasher<Usuarios>();
                    usuarios.Contrasena = hasher.HashPassword(usuarios, usuarios.Contrasena);
                }
                else
                {
                    usuarios.Contrasena = original.Contrasena;
                }

                if (UsuarioLogueado.Id == usuarios.IdUsuario)
                {
                    if (usuarios.Estado == false)
                    {
                        ModelState.AddModelError("", "No se puede bloquear el usuario logueado.");
                        await CargarListasSedes();
                        await CargarListasDependencias();
                        ViewBag.Roles = ObtenerRoles();
                        return View(usuarios);
                    }
                }

                usuarios.FechaCreacion = original.FechaCreacion;

                var usuarioDto = new
                {
                    idUsuario = usuarios.IdUsuario,
                    nombre = usuarios.Nombre,
                    correo = usuarios.Correo,
                    contrasena = usuarios.Contrasena,
                    rol = usuarios.Rol,
                    idSede = usuarios.IdSede,
                    idDependencia = usuarios.IdDependencia,
                    estado = usuarios.Estado,
                    fechaCreacion = usuarios.FechaCreacion
                };

                var respuesta = await _httpClient.PutAsJsonAsync(_apiUrl + "/" + id, usuarioDto);
                if (respuesta.IsSuccessStatusCode)
                {
                    if (UsuarioLogueado.Id == usuarios.IdUsuario)
                    {
                        UsuarioLogueado.Nombre = usuarios.Nombre;
                        UsuarioLogueado.Correo = usuarios.Correo;
                        UsuarioLogueado.Rol = usuarios.Rol;
                        UsuarioLogueado.IdSede = usuarios.IdSede;
                        UsuarioLogueado.IdDependencia = usuarios.IdDependencia;

                        if (original.Correo != usuarios.Correo || original.Contrasena != usuarios.Contrasena)
                        {
                            return RedirectToAction("Login", "Home");
                        }
                    }

                    TempData["Success"] = "Usuario actualizado exitosamente";
                    return RedirectToAction(nameof(Index));
                }

                await ModelState.AddErrorsFromApiResponseAsync(respuesta);
            }

            await CargarListasSedes();
            await CargarListasDependencias();
            ViewBag.Roles = ObtenerRoles();
            return View(usuarios);
        }

        private async Task<List<Usuarios>> ObtenerListadoUsuarios()
        {
            List<Usuarios> usuarios = new List<Usuarios>();

            try
            {
                var respuesta = await _httpClient.GetAsync(_apiUrl);
                if (respuesta.IsSuccessStatusCode)
                {
                    var content = await respuesta.Content.ReadAsStringAsync();
                    usuarios = JsonConvert.DeserializeObject<List<Usuarios>>(content);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] Al obtener usuarios: {ex.Message}");
            }

            return usuarios ?? new List<Usuarios>();
        }

        private async Task<Usuarios> ObtenerUsuarioXId(int id)
        {
            Usuarios usuarios = null;

            try
            {
                var respuesta = await _httpClient.GetAsync(_apiUrl + "/" + id);
                if (respuesta.IsSuccessStatusCode)
                {
                    var content = await respuesta.Content.ReadAsStringAsync();
                    usuarios = JsonConvert.DeserializeObject<Usuarios>(content);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] Al obtener usuario {id}: {ex.Message}");
            }

            return usuarios;
        }

        private async Task CargarListasSedes()
        {
            try
            {
                var respuesta = await _httpClient.GetAsync($"{_baseUrl}/Sedes");
                if (respuesta.IsSuccessStatusCode)
                {
                    var content = await respuesta.Content.ReadAsStringAsync();
                    var sedes = JsonConvert.DeserializeObject<List<dynamic>>(content);

                    ViewBag.Sedes = new SelectList(
                        sedes.Select(s => new {
                            IdSede = (int)s.idSede,
                            Nombre = (string)s.nombre
                        }),
                        "IdSede",
                        "Nombre"
                    );
                }
                else
                {
                    ViewBag.Sedes = new SelectList(new List<object>(), "IdSede", "Nombre");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] Al cargar sedes: {ex.Message}");
                ViewBag.Sedes = new SelectList(new List<object>(), "IdSede", "Nombre");
            }
        }

        private async Task CargarListasDependencias()
        {
            try
            {
                var respuesta = await _httpClient.GetAsync($"{_baseUrl}/Dependencias");
                if (respuesta.IsSuccessStatusCode)
                {
                    var content = await respuesta.Content.ReadAsStringAsync();
                    var dependencias = JsonConvert.DeserializeObject<List<dynamic>>(content);

                    ViewBag.Dependencias = new SelectList(
                        dependencias.Select(d => new {
                            IdDependencia = (int)d.idDependencia,
                            Nombre = (string)d.nombre
                        }),
                        "IdDependencia",
                        "Nombre"
                    );
                }
                else
                {
                    ViewBag.Dependencias = new SelectList(new List<object>(), "IdDependencia", "Nombre");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] Al cargar dependencias: {ex.Message}");
                ViewBag.Dependencias = new SelectList(new List<object>(), "IdDependencia", "Nombre");
            }
        }

        private List<SelectListItem> ObtenerRoles()
        {
            return new List<SelectListItem>
            {
                new SelectListItem { Value = "SuperAdmin", Text = "Super Administrador" },
                new SelectListItem { Value = "AdminSede", Text = "Administrador de Sede" },
                new SelectListItem { Value = "EncargadoDependencia", Text = "Encargado de Dependencia" },
                new SelectListItem { Value = "Cliente", Text = "Cliente" },
                new SelectListItem { Value = "Administrador", Text = "Administrador (Legacy)" },
                new SelectListItem { Value = "Empleado", Text = "Empleado (Legacy)" }
            };
        }
    }
}