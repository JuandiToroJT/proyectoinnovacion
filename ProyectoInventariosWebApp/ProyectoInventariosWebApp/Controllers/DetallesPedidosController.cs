using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProyectoInventariosWebApp.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using ProyectoInventariosWebApp.Filtro;
using Newtonsoft.Json;
using ProyectoInventariosWebApp.Helpers;
using Microsoft.Extensions.Options;

namespace ProyectoInventariosWebApp.Controllers
{
    [AutenticadoEmpleado]
    public class DetallesPedidosController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly string URL_API;
        private readonly string URL_API_PEDIDOS;
        private readonly string URL_API_PRODUCTOS;
        public DetallesPedidosController(HttpClient httpClient, IOptions<ApiUrlsOptions> apiOptions)
        {
            _httpClient = httpClient;
            URL_API = apiOptions.Value.BaseUrl + "DetallesPedidos";
            URL_API_PEDIDOS = apiOptions.Value.BaseUrl + "Pedidos";
            URL_API_PRODUCTOS = apiOptions.Value.BaseUrl + "Productos";
        }

        // GET: DetallesPedidos/Create
        public async Task<IActionResult> Create(int idPedido)
        {
            if (await PedidoCerrado(idPedido))
            {
                TempData["Error"] = "No se puede agregar productos: el pedido ya está cerrado.";
                return RedirectToAction("Details", "Pedidos", new { id = idPedido });
            }

            ViewBag.IdPedido = idPedido;
            ViewData["IdProducto"] = new SelectList(await ObtenerListadoProductos(), "IdProducto", "Nombre");
            return View(new DetallesPedido { IdPedido = idPedido });
        }

        // POST: DetallesPedidos/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdPedido,IdProducto,Cantidad")] DetallesPedido detalle)
        {
            if (await PedidoCerrado(detalle.IdPedido))
            {
                TempData["Error"] = "No se puede agregar productos: el pedido ya está cerrado.";
                return RedirectToAction("Details", "Pedidos", new { id = detalle.IdPedido });
            }

            if (detalle.Cantidad <= 0)
                ModelState.AddModelError("Cantidad", "La cantidad debe ser mayor que 0.");

            var producto = await ObtenerProductoXId(detalle.IdProducto);
            if (producto == null)
                ModelState.AddModelError("", "Producto inválido.");
            else if (detalle.Cantidad > producto.Stock)
                ModelState.AddModelError("Cantidad", "No hay suficiente stock disponible.");

            if (!ModelState.IsValid)
            {
                ViewBag.IdPedido = detalle.IdPedido;
                ViewData["IdProducto"] = new SelectList(await ObtenerListadoProductos(), "IdProducto", "Nombre", detalle.IdProducto);
                return View(detalle);
            }

            detalle.Subtotal = producto.Precio * detalle.Cantidad;
            producto.Stock -= detalle.Cantidad;

            await _httpClient.PutAsJsonAsync(URL_API_PRODUCTOS + "/" + detalle.IdProducto, producto);
            await _httpClient.PostAsJsonAsync(URL_API, detalle);

            return RedirectToAction("Details", "Pedidos", new { id = detalle.IdPedido });
        }

        // GET: DetallesPedidos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var detalle = await ObtenerDetallePedidoXId(id.Value);
            if (detalle == null) return NotFound();
            if (await PedidoCerrado(detalle.IdPedido))
            {
                TempData["Error"] = "No se puede modificar productos: el pedido ya está cerrado.";
                return RedirectToAction("Details", "Pedidos", new { id = detalle.IdPedido });
            }

            var producto = await ObtenerProductoXId(detalle.IdProducto);
            ViewBag.NombreProducto = producto?.Nombre ?? string.Empty;
            return View(detalle);
        }

        // POST: DetallesPedidos/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdDetalle,IdPedido,IdProducto,Cantidad")] DetallesPedido detalle)
        {
            if (id != detalle.IdDetalle) return NotFound();
            if (await PedidoCerrado(detalle.IdPedido))
            {
                TempData["Error"] = "No se puede modificar productos: el pedido ya está cerrado.";
                return RedirectToAction("Details", "Pedidos", new { id = detalle.IdPedido });
            }

            if (detalle.Cantidad <= 0)
                ModelState.AddModelError("Cantidad", "La cantidad debe ser mayor que 0.");

            var original = await ObtenerDetallePedidoXId(id);
            var producto = await ObtenerProductoXId(detalle.IdProducto);

            if (producto == null)
                ModelState.AddModelError("", "Producto inválido.");
            else
            {
                producto.Stock += original.Cantidad;
                if (detalle.Cantidad > producto.Stock)
                    ModelState.AddModelError("Cantidad", "No hay suficiente stock disponible para la nueva cantidad.");
            }

            if (!ModelState.IsValid)
            {
                ViewBag.NombreProducto = producto?.Nombre ?? string.Empty;
                return View(detalle);
            }

            detalle.Subtotal = producto.Precio * detalle.Cantidad;
            producto.Stock -= detalle.Cantidad;

            await _httpClient.PutAsJsonAsync(URL_API_PRODUCTOS + "/" + detalle.IdProducto, producto);
            await _httpClient.PutAsJsonAsync(URL_API + "/" + id, detalle);

            return RedirectToAction("Details", "Pedidos", new { id = detalle.IdPedido });
        }

        // POST: DetallesPedidos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var detalle = await ObtenerDetallePedidoXId(id);
            if (detalle != null)
            {
                if (await PedidoCerrado(detalle.IdPedido))
                {
                    TempData["Error"] = "No se puede eliminar productos: el pedido ya está cerrado.";
                    return RedirectToAction("Details", "Pedidos", new { id = detalle.IdPedido });
                }

                var producto = await ObtenerProductoXId(detalle.IdProducto);
                producto.Stock += detalle.Cantidad;

                await _httpClient.PutAsJsonAsync(URL_API_PRODUCTOS + "/" + detalle.IdProducto, producto);
                await _httpClient.DeleteAsync(URL_API + "/" + id);
            }
            return RedirectToAction("Details", "Pedidos", new { id = detalle.IdPedido });
        }

        private async Task<Pedidos> ObtenerPedidoXId(int id)
        {
            Pedidos pedidos = null;
            var respuesta = await _httpClient.GetAsync(URL_API_PEDIDOS + "/" + id);
            if (respuesta.IsSuccessStatusCode)
            {
                var content = await respuesta.Content.ReadAsStringAsync();
                pedidos = JsonConvert.DeserializeObject<Pedidos>(content);
            }
            return pedidos;
        }

        private async Task<List<Productos>> ObtenerListadoProductos()
        {
            List<Productos> productos = new List<Productos>();

            var respuesta = await _httpClient.GetAsync(URL_API_PRODUCTOS);
            if (respuesta.IsSuccessStatusCode)
            {
                var content = await respuesta.Content.ReadAsStringAsync();
                productos = JsonConvert.DeserializeObject<List<Productos>>(content);
            }

            return productos;
        }

        private async Task<DetallesPedido> ObtenerDetallePedidoXId(int id)
        {
            DetallesPedido detallesPedido = null;
            var respuesta = await _httpClient.GetAsync(URL_API + "/" + id);
            if (respuesta.IsSuccessStatusCode)
            {
                var content = await respuesta.Content.ReadAsStringAsync();
                detallesPedido = JsonConvert.DeserializeObject<DetallesPedido>(content);
            }
            return detallesPedido;
        }

        private async Task<Productos> ObtenerProductoXId(int id)
        {
            Productos productos = null;
            var respuesta = await _httpClient.GetAsync(URL_API_PRODUCTOS + "/" + id);
            if (respuesta.IsSuccessStatusCode)
            {
                var content = await respuesta.Content.ReadAsStringAsync();
                productos = JsonConvert.DeserializeObject<Productos>(content);
            }
            return productos;
        }

        private async Task<bool> PedidoCerrado(int idPedido)
        {
            var pedido = await ObtenerPedidoXId(idPedido);
            return pedido != null && (pedido.Facturas.Any() || pedido.Entregas.Any());
        }
    }
}