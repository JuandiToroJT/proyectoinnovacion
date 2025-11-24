using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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
        private readonly string _apiUrl;
        private readonly string _baseUrl;

        public ProductosController(HttpClient httpClient, IOptions<ApiUrlsOptions> apiOptions)
        {
            _httpClient = httpClient;
            _apiUrl = apiOptions.Value.BaseUrl + "/Productos";
        }

        public async Task<IActionResult> Index()
        {
            ViewBag.ApiUrl = _baseUrl;
            return View(await ObtenerListadoProductos());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var producto = await ObtenerProductoConInventarioXId(id.Value);
            if (producto == null)
            {
                return NotFound();
            }

            ViewBag.ApiUrl = _baseUrl;
            return View(producto);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Nombre,Descripcion,Precio,Categoria,UnidadMedida,EsCompartible")] ProductoConInventario producto)
        {
            if (ModelState.IsValid)
            {
                var productoDto = new
                {
                    nombre = producto.Nombre,
                    descripcion = producto.Descripcion,
                    precio = producto.Precio,
                    categoria = producto.Categoria,
                    unidadMedida = producto.UnidadMedida,
                    esCompartible = producto.EsCompartible,
                    stockMinimoGlobal = 10
                };

                var respuesta = await _httpClient.PostAsJsonAsync(_apiUrl, productoDto);
                if (respuesta.IsSuccessStatusCode)
                {
                    return RedirectToAction(nameof(Index));
                }

                await ModelState.AddErrorsFromApiResponseAsync(respuesta);
            }

            return View(producto);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var producto = await ObtenerProductoConInventarioXId(id.Value);
            if (producto == null)
            {
                return NotFound();
            }

            return View(producto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdProducto,Codigo,Nombre,Descripcion,Precio,Categoria,UnidadMedida,EsCompartible,Estado")] ProductoConInventario producto)
        {
            if (id != producto.IdProducto)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var productoDto = new
                {
                    idProducto = producto.IdProducto,
                    codigo = producto.Codigo,
                    nombre = producto.Nombre,
                    descripcion = producto.Descripcion,
                    precio = producto.Precio,
                    categoria = producto.Categoria,
                    unidadMedida = producto.UnidadMedida,
                    esCompartible = producto.EsCompartible,
                    stockMinimoGlobal = 10,
                    estado = producto.Estado,
                    stockTotal = producto.StockTotal
                };

                var respuesta = await _httpClient.PutAsJsonAsync($"{_apiUrl}/{id}", productoDto);
                if (respuesta.IsSuccessStatusCode)
                {
                    return RedirectToAction(nameof(Index));
                }

                await ModelState.AddErrorsFromApiResponseAsync(respuesta);
            }

            return View(producto);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var producto = await ObtenerProductoConInventarioXId(id.Value);
            if (producto == null)
            {
                return NotFound();
            }

            return View(producto);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var respuesta = await _httpClient.DeleteAsync($"{_apiUrl}/{id}");
            return RedirectToAction(nameof(Index));
        }

        private async Task<List<ProductoConInventario>> ObtenerListadoProductos()
        {
            List<ProductoConInventario> productos = new List<ProductoConInventario>();

            try
            {
                var respuesta = await _httpClient.GetAsync(_apiUrl);
                if (respuesta.IsSuccessStatusCode)
                {
                    var content = await respuesta.Content.ReadAsStringAsync();
                    productos = JsonConvert.DeserializeObject<List<ProductoConInventario>>(content);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener productos: {ex.Message}");
            }

            return productos ?? new List<ProductoConInventario>();
        }

        private async Task<ProductoConInventario> ObtenerProductoConInventarioXId(int id)
        {
            ProductoConInventario producto = null;

            try
            {
                var respuesta = await _httpClient.GetAsync($"{_apiUrl}/{id}");
                if (respuesta.IsSuccessStatusCode)
                {
                    var content = await respuesta.Content.ReadAsStringAsync();
                    producto = JsonConvert.DeserializeObject<ProductoConInventario>(content);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener producto {id}: {ex.Message}");
            }

            return producto;
        }
    }
}