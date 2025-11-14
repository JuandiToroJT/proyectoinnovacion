using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using ProyectoInventariosWebApp.Models;
using ProyectoInventariosWebApp.Filtro;
using Newtonsoft.Json;
using ProyectoInventariosWebApp.Helpers;
using Microsoft.Extensions.Options;

namespace ProyectoInventariosWebApp.Controllers
{
    [AutenticadoEmpleado]
    public class PedidosController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly string URL_API;
        private readonly string URL_API_CLIENTES;
        private readonly string URL_API_DETALLES;

        public PedidosController(HttpClient httpClient, IOptions<ApiUrlsOptions> apiOptions)
        {
            _httpClient = httpClient;
            URL_API = apiOptions.Value.BaseUrl + "Pedidos";
            URL_API_CLIENTES = apiOptions.Value.BaseUrl + "Clientes";
            URL_API_DETALLES = apiOptions.Value.BaseUrl + "DetallesPedidos";
        }

        // GET: Pedidos
        public async Task<IActionResult> Index()
        {
            ViewData["Title"] = "Pedidos Pendientes";
            return View(await ObtenerListadoPedidos("Pendiente"));
        }

        // GET: Pedidos/Procesados
        public async Task<IActionResult> Procesados()
        {
            ViewData["Title"] = "Pedidos Procesados";
            return View("Index", await ObtenerListadoPedidos("Procesado"));
        }

        // GET: Pedidos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            Pedidos pedido = await ObtenerPedidoXId(id.Value);
            if (pedido == null) return NotFound();

            ViewData["EsProcesado"] = pedido.Estado == "Procesado";
            return View(pedido);
        }

        // GET: Pedidos/Create
        public async Task<IActionResult> Create()
        {
            ViewData["IdCliente"] = new SelectList(await ObtenerListadoClientes(), "IdCliente", "Nombre");
            return View();
        }

        // POST: Pedidos/Create
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdCliente")] Pedidos pedido)
        {
            if (ModelState.IsValid)
            {
                pedido.IdUsuario = UsuarioLogueado.Id.Value;
                pedido.Fecha = DateTime.Now;
                pedido.Estado = "Pendiente";

                var respuesta = await _httpClient.PostAsJsonAsync(URL_API, pedido);
                if (respuesta.IsSuccessStatusCode)
                {
                    var content = await respuesta.Content.ReadAsStringAsync();
                    pedido = JsonConvert.DeserializeObject<Pedidos>(content);
                    return RedirectToAction("Create", "DetallesPedidos", new { idPedido = pedido.IdPedido });
                }

                await ModelState.AddErrorsFromApiResponseAsync(respuesta);
            }

            ViewData["IdCliente"] = new SelectList(await ObtenerListadoClientes(), "IdCliente", "Nombre", pedido.IdCliente);
            return View(pedido);
        }

        // POST: Pedidos/GenerarEntrega/5
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> GenerarEntrega(int id)
        {
            Pedidos pedido = null;
            var respuesta = await _httpClient.GetAsync(URL_API + "/Entregas/" + id);
            if (respuesta.IsSuccessStatusCode)
            {
                var content = await respuesta.Content.ReadAsStringAsync();
                pedido = JsonConvert.DeserializeObject<Pedidos>(content);
            }

            if (pedido == null) return NotFound();

            if (pedido.Entregas.Any())
            {
                TempData["AlertaEntrega"] = "Este pedido ya tiene una entrega programada.";
                return RedirectToAction(nameof(Index));
            }

            var entrega = new Entregas
            {
                IdPedido = id,
                DireccionEntrega = pedido.IdClienteNavigation.Direccion,
                FechaEntrega = DateTime.Now.AddDays(1),
                Estado = "Programado"
            };

            await _httpClient.PostAsJsonAsync(URL_API + "/Entregas", entrega);

            TempData["AlertaEntrega"] = $"Entrega programada para {entrega.FechaEntrega:yyyy-MM-dd}.";
            return RedirectToAction(nameof(Index));
        }

        // POST: Pedidos/GenerarFactura/5
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> GenerarFactura(int id)
        {
            List<Facturas> facturas = await ObtenerListadoFacturasXPedido(id);
            if (facturas.Any())
            {
                TempData["AlertaFactura"] = "Ya existe una factura para este pedido.";
                return RedirectToAction(nameof(Index));
            }

            List<DetallesPedido> detallesPedidos = await ObtenerListadoDetallesPedidos(id);

            var total = detallesPedidos.Sum(d => d.Subtotal);

            var factura = new Facturas
            {
                IdPedido = id,
                Total = total,
                Fecha = DateTime.Now
            };

            var respuesta = await _httpClient.PostAsJsonAsync(URL_API + "/Facturas", factura);
            if (respuesta.IsSuccessStatusCode)
            {
                var content = await respuesta.Content.ReadAsStringAsync();
                factura = JsonConvert.DeserializeObject<Facturas>(content);
            }

            TempData["AlertaFactura"] = $"Factura #{factura.IdFactura} generada por {factura.Total:C}.";
            return RedirectToAction(nameof(Index));
        }

        // POST: Pedidos/MarcarProcesado/5
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> MarcarProcesado(int id)
        {
            var pedido = await ObtenerPedidoXId(id);
            if (pedido != null)
            {
                pedido.Estado = "Procesado";

                await _httpClient.PutAsJsonAsync(URL_API + "/" + id, pedido);

                TempData["AlertaProcesado"] = "Pedido marcado como procesado.";
            }
            return RedirectToAction(nameof(Index));
        }

        private async Task<List<Pedidos>> ObtenerListadoPedidos(string estado)
        {
            List<Pedidos> pedidos = new List<Pedidos>();

            var respuesta = await _httpClient.GetAsync(URL_API + "?estado=" + estado);
            if (respuesta.IsSuccessStatusCode)
            {
                var content = await respuesta.Content.ReadAsStringAsync();
                pedidos = JsonConvert.DeserializeObject<List<Pedidos>>(content);
            }

            return pedidos;
        }

        private async Task<Pedidos> ObtenerPedidoXId(int id)
        {
            Pedidos pedidos = null;
            var respuesta = await _httpClient.GetAsync(URL_API + "/" + id);
            if (respuesta.IsSuccessStatusCode)
            {
                var content = await respuesta.Content.ReadAsStringAsync();
                pedidos = JsonConvert.DeserializeObject<Pedidos>(content);
            }
            return pedidos;
        }

        private async Task<List<Clientes>> ObtenerListadoClientes()
        {
            List<Clientes> clientes = new List<Clientes>();

            var respuesta = await _httpClient.GetAsync(URL_API_CLIENTES);
            if (respuesta.IsSuccessStatusCode)
            {
                var content = await respuesta.Content.ReadAsStringAsync();
                clientes = JsonConvert.DeserializeObject<List<Clientes>>(content);
            }

            return clientes;
        }

        private async Task<List<Facturas>> ObtenerListadoFacturasXPedido(int id)
        {
            List<Facturas> facturas = new List<Facturas>();

            var respuesta = await _httpClient.GetAsync(URL_API + "/Facturas/Producto/" + id);
            if (respuesta.IsSuccessStatusCode)
            {
                var content = await respuesta.Content.ReadAsStringAsync();
                facturas = JsonConvert.DeserializeObject<List<Facturas>>(content);
            }

            return facturas;
        }

        private async Task<List<DetallesPedido>> ObtenerListadoDetallesPedidos(int id)
        {
            List<DetallesPedido> detallesPedidos = new List<DetallesPedido>();

            var respuesta = await _httpClient.GetAsync(URL_API_DETALLES + "?idPedido=" + id);
            if (respuesta.IsSuccessStatusCode)
            {
                var content = await respuesta.Content.ReadAsStringAsync();
                detallesPedidos = JsonConvert.DeserializeObject<List<DetallesPedido>>(content);
            }

            return detallesPedidos;
        }
    }
}
