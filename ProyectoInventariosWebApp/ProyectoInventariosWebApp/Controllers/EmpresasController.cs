using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProyectoInventariosWebApp.Models;
using ProyectoInventariosWebApp.Filtro;
using Newtonsoft.Json;
using ProyectoInventariosWebApp.Helpers;
using Microsoft.Extensions.Options;

namespace ProyectoInventariosWebApp.Controllers
{
    [AutenticadoAdministrador]
    public class EmpresasController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly string URL_API;

        public EmpresasController(HttpClient httpClient, IOptions<ApiUrlsOptions> apiOptions)
        {
            _httpClient = httpClient;
            URL_API = apiOptions.Value.BaseUrl + "Empresas";
        }

        // GET: Empresas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Empresas empresas = await ObtenerEmpresaXId(id.Value);
            if (empresas == null)
            {
                return NotFound();
            }

            return View(empresas);
        }

        // GET: Empresas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Empresas empresas = await ObtenerEmpresaXId(id.Value);
            if (empresas == null)
            {
                return NotFound();
            }

            return View(empresas);
        }

        // POST: Empresas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdEmpresa,Nombre,Direccion,Telefono,Nit,Ciudad,FechaCreacion,RepresentanteLegal,TipoEmpresa,PaginaWeb,EmailContacto")] Empresas empresas)
        {
            if (id != empresas.IdEmpresa)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var respuesta = await _httpClient.PutAsJsonAsync(URL_API + "/" + id, empresas);
                if (respuesta.IsSuccessStatusCode)
                {
                    EmpresaActual.Id = empresas.IdEmpresa;
                    EmpresaActual.Nombre = empresas.Nombre;
                    EmpresaActual.Correo = empresas.EmailContacto;
                    EmpresaActual.Telefono = empresas.Telefono;
                    EmpresaActual.Direccion = empresas.Direccion;
                    EmpresaActual.Ciudad = empresas.Ciudad;
                    EmpresaActual.PaginaWeb = empresas.PaginaWeb;

                    return RedirectToAction(nameof(Details), new { id = empresas.IdEmpresa });
                }

                await ModelState.AddErrorsFromApiResponseAsync(respuesta);
            }

            return View(empresas);
        }

        private async Task<Empresas> ObtenerEmpresaXId(int id)
        {
            Empresas empresas = null;

            var respuesta = await _httpClient.GetAsync(URL_API + "/" + id);
            if (respuesta.IsSuccessStatusCode)
            {
                var content = await respuesta.Content.ReadAsStringAsync();
                empresas = JsonConvert.DeserializeObject<Empresas>(content);
            }

            return empresas;
        }
    }
}
