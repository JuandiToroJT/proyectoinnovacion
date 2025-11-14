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
    public class ProductosController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly string URL_API;

        public ProductosController(HttpClient httpClient, IOptions<ApiUrlsOptions> apiOptions)
        {
            _httpClient = httpClient;
            URL_API = apiOptions.Value.BaseUrl + "Productos";
        }

        // GET: Productos
        public async Task<IActionResult> Index()
        {
            return View(await ObtenerListadoProductos());
        }

        // GET: Productos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Productos productos = await ObtenerProductoXId(id.Value);
            if (productos == null)
            {
                return NotFound();
            }

            return View(productos);
        }

        // GET: Productos/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Productos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdProducto,Nombre,Descripcion,Precio,Stock")] Productos productos)
        {
            if (ModelState.IsValid)
            {
                var respuesta = await _httpClient.PostAsJsonAsync(URL_API, productos);
                if (respuesta.IsSuccessStatusCode)
                {
                    return RedirectToAction(nameof(Index));
                }

                await ModelState.AddErrorsFromApiResponseAsync(respuesta);
            }

            return View(productos);
        }

        // GET: Productos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Productos productos = await ObtenerProductoXId(id.Value);
            if (productos == null)
            {
                return NotFound();
            }

            return View(productos);
        }

        // POST: Productos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdProducto,Nombre,Descripcion,Precio,Stock")] Productos productos)
        {
            if (id != productos.IdProducto)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var respuesta = await _httpClient.PutAsJsonAsync(URL_API + "/" + id, productos);
                if (respuesta.IsSuccessStatusCode)
                {
                    return RedirectToAction(nameof(Index));
                }

                await ModelState.AddErrorsFromApiResponseAsync(respuesta);
            }

            return View(productos);
        }

        // GET: Productos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Productos productos = await ObtenerProductoXId(id.Value);
            if (productos == null)
            {
                return NotFound();
            }

            return View(productos);
        }

        // POST: Productos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var respuesta = await _httpClient.DeleteAsync(URL_API + "/" + id);
            return RedirectToAction(nameof(Index));
        }

        private async Task<List<Productos>> ObtenerListadoProductos()
        {
            List<Productos> productos = new List<Productos>();

            var respuesta = await _httpClient.GetAsync(URL_API);
            if (respuesta.IsSuccessStatusCode)
            {
                var content = await respuesta.Content.ReadAsStringAsync();
                productos = JsonConvert.DeserializeObject<List<Productos>>(content);
            }

            return productos;
        }

        private async Task<Productos> ObtenerProductoXId(int id)
        {
            Productos productos = null;
            var respuesta = await _httpClient.GetAsync(URL_API + "/" + id);
            if (respuesta.IsSuccessStatusCode)
            {
                var content = await respuesta.Content.ReadAsStringAsync();
                productos = JsonConvert.DeserializeObject<Productos>(content);
            }
            return productos;
        }
    }
}
