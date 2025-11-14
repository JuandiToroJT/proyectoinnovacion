using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProyectoInventariosWebApp.Models;
using ProyectoInventariosWebApp.Filtro;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using System.Net;
using System.Text.Json;
using ProyectoInventariosWebApp.Helpers;
using Microsoft.Extensions.Options;

namespace ProyectoInventariosWebApp.Controllers
{
    [AutenticadoAdministrador]
    public class UsuariosController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly string URL_API;

        public UsuariosController(HttpClient httpClient, IOptions<ApiUrlsOptions> apiOptions)
        {
            _httpClient = httpClient;
            URL_API = apiOptions.Value.BaseUrl + "Usuarios";
        }

        // GET: Usuarios
        public async Task<IActionResult> Index()
        {
            return View(await ObtenerListadoUsuarios());
        }

        // GET: Usuarios/Details/5
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

        // GET: Usuarios/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Usuarios/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdUsuario,Nombre,Correo,Contrasena,Rol,Estado")] Usuarios usuarios, string ConfirmarContrasena)
        {
            if (ModelState.IsValid)
            {
                if (usuarios.Contrasena != ConfirmarContrasena)
                {
                    ModelState.AddModelError("Contrasena", "Las contraseñas no coinciden.");
                    return View(usuarios);
                }

                usuarios.Estado = true;

                var hasher = new PasswordHasher<Usuarios>();
                usuarios.Contrasena = hasher.HashPassword(usuarios, usuarios.Contrasena);

                var respuesta = await _httpClient.PostAsJsonAsync(URL_API, usuarios);
                if (respuesta.IsSuccessStatusCode)
                {
                    return RedirectToAction(nameof(Index));
                }

                await ModelState.AddErrorsFromApiResponseAsync(respuesta);
            }

            return View(usuarios);
        }

        // GET: Usuarios/Edit/5
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

            return View(usuarios);
        }

        // POST: Usuarios/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdUsuario,Nombre,Correo,Contrasena,Rol,Estado")] Usuarios usuarios, string ConfirmarContrasena = "")
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
                        return View(usuarios);
                    }
                }

                var respuesta = await _httpClient.PutAsJsonAsync(URL_API + "/" + id, usuarios);
                if (respuesta.IsSuccessStatusCode)
                {
                    if (UsuarioLogueado.Id == usuarios.IdUsuario)
                    {
                        UsuarioLogueado.Nombre = usuarios.Nombre;
                        UsuarioLogueado.Correo = usuarios.Correo;
                        UsuarioLogueado.Rol = usuarios.Rol;

                        if (original.Correo != usuarios.Correo || original.Contrasena != usuarios.Contrasena || original.Rol != original.Rol)
                        {
                            return RedirectToAction("Login", "Home");
                        }
                    }

                    return RedirectToAction(nameof(Index));
                }

                await ModelState.AddErrorsFromApiResponseAsync(respuesta);
            }

            return View(usuarios);
        }

        private async Task<List<Usuarios>> ObtenerListadoUsuarios()
        {
            List<Usuarios> usuarios = new List<Usuarios>();

            var respuesta = await _httpClient.GetAsync(URL_API);
            if (respuesta.IsSuccessStatusCode)
            {
                var content = await respuesta.Content.ReadAsStringAsync();
                usuarios = JsonConvert.DeserializeObject<List<Usuarios>>(content);
            }

            return usuarios;
        }

        private async Task<Usuarios> ObtenerUsuarioXId(int id)
        {
            Usuarios usuarios = null;

            var respuesta = await _httpClient.GetAsync(URL_API + "/" + id);
            if (respuesta.IsSuccessStatusCode)
            {
                var content = await respuesta.Content.ReadAsStringAsync();
                usuarios = JsonConvert.DeserializeObject<Usuarios>(content);
            }

            return usuarios;
        }
    }
}
