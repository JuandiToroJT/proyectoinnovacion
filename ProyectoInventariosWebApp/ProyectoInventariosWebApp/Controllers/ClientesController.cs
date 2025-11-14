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
    [AutenticadoEmpleado]
    public class ClientesController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly string URL_API;

        public ClientesController(HttpClient httpClient, IOptions<ApiUrlsOptions> apiOptions)
        {
            _httpClient = httpClient;
            URL_API = apiOptions.Value.BaseUrl + "Clientes";
        }

        // GET: Clientes
        public async Task<IActionResult> Index()
        {
            return View(await ObtenerListadoClientes());
        }

        // GET: Clientes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Clientes clientes = await ObtenerClienteXId(id.Value);
            if (clientes == null)
            {
                return NotFound();
            }

            return View(clientes);
        }

        // GET: Clientes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Clientes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdCliente,Nombre,Telefono,Direccion")] Clientes clientes)
        {
            if (ModelState.IsValid)
            {
                var respuesta = await _httpClient.PostAsJsonAsync(URL_API, clientes);
                if (respuesta.IsSuccessStatusCode)
                {
                    return RedirectToAction(nameof(Index));
                }

                await ModelState.AddErrorsFromApiResponseAsync(respuesta);
            }

            return View(clientes);
        }

        // GET: Clientes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Clientes clientes = await ObtenerClienteXId(id.Value);
            if (clientes == null)
            {
                return NotFound();
            }

            return View(clientes);
        }

        // POST: Clientes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdCliente,Nombre,Telefono,Direccion")] Clientes clientes)
        {
            if (id != clientes.IdCliente)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var respuesta = await _httpClient.PutAsJsonAsync(URL_API + "/" + id, clientes);
                if (respuesta.IsSuccessStatusCode)
                {
                    return RedirectToAction(nameof(Index));
                }

                await ModelState.AddErrorsFromApiResponseAsync(respuesta);
            }

            return View(clientes);
        }

        // GET: Clientes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Clientes clientes = await ObtenerClienteXId(id.Value);
            if (clientes == null)
            {
                return NotFound();
            }

            return View(clientes);
        }

        // POST: Clientes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var respuesta = await _httpClient.DeleteAsync(URL_API + "/" + id);
            return RedirectToAction(nameof(Index));
        }

        private async Task<List<Clientes>> ObtenerListadoClientes()
        {
            List<Clientes> clientes = new List<Clientes>();

            var respuesta = await _httpClient.GetAsync(URL_API);
            if (respuesta.IsSuccessStatusCode)
            {
                var content = await respuesta.Content.ReadAsStringAsync();
                clientes = JsonConvert.DeserializeObject<List<Clientes>>(content);
            }

            return clientes;
        }

        private async Task<Clientes> ObtenerClienteXId(int id)
        {
            Clientes clientes = null;
            var respuesta = await _httpClient.GetAsync(URL_API + "/" + id);
            if (respuesta.IsSuccessStatusCode)
            {
                var content = await respuesta.Content.ReadAsStringAsync();
                clientes = JsonConvert.DeserializeObject<Clientes>(content);
            }
            return clientes;
        }
    }
}
