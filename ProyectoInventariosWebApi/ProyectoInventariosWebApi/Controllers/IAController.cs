using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProyectoInventariosWebApi.Models;
using ProyectoInventariosWebApi.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProyectoInventariosWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IAController : ControllerBase
    {
        private readonly ProyectoInventariosDbContext _context;
        private readonly AIGeminiService _aiService;

        public IAController(ProyectoInventariosDbContext context, AIGeminiService aiService)
        {
            _context = context;
            _aiService = aiService;
        }

        [HttpGet("analisis-ventas")]
        public async Task<IActionResult> GetAnalisisVentas([FromQuery] int? idSede)
        {
            var query = _context.Pedidos
                .Where(p => p.Fecha >= DateTime.Now.AddDays(-60) && p.Estado == "Entregado")
                .Include(p => p.IdClienteNavigation)
                .Include(p => p.IdSedeNavigation)
                .Include(p => p.IdDependenciaNavigation)
                .Include(p => p.DetallesPedido).ThenInclude(dp => dp.IdProductoNavigation)
                .AsQueryable();

            if (idSede.HasValue)
            {
                query = query.Where(p => p.IdSede == idSede.Value);
            }

            var ventasData = await query.ToListAsync();

            var datosParaIA = ventasData.Select(v => new VentaAIDto
            {
                Fecha = v.Fecha?.ToShortDateString(),
                Sede = v.IdSedeNavigation?.Nombre,
                Dependencia = v.IdDependenciaNavigation?.Nombre,
                TipoCliente = v.IdClienteNavigation?.TipoCliente,
                TotalVenta = v.Total ?? 0,
                Productos = v.DetallesPedido.Select(d => d.IdProductoNavigation.Nombre).ToList()
            }).ToList();

            AnalisisVentasSalidaDto reporte = await _aiService.AnalizarVentasAsync(datosParaIA);

            return Ok(reporte);
        }

        [HttpGet("sugerir-reorden")]
        public async Task<IActionResult> GetSugerenciaStock([FromQuery] int? idSede)
        {
            IQueryable<InventarioDependencia> query = _context.InventarioDependencia
                .Where(i => i.StockActual <= i.StockMinimo || i.StockActual <= i.StockMaximo * 0.1);

            if (idSede.HasValue)
            {
                query = query.Where(i => i.IdDependenciaNavigation.IdSede == idSede.Value);
            }

            var stockData = await query
                .Include(i => i.IdProductoNavigation)
                .Include(i => i.IdDependenciaNavigation).ThenInclude(d => d.IdSedeNavigation)
                .ToListAsync();

            var datosParaIA = stockData.Select(i => new StockAIDto
            {
                Producto = i.IdProductoNavigation.Nombre,
                Categoria = i.IdProductoNavigation.Categoria,
                Ubicacion = $"{i.IdDependenciaNavigation.IdSedeNavigation.Nombre} - {i.IdDependenciaNavigation.Nombre}",
                Actual = i.StockActual,
                Minimo = i.StockMinimo,
                Maximo = i.StockMaximo,
                CostoPromedio = i.CostoPromedio
            }).ToList();

            StockSalidaDto sugerencia = await _aiService.AnalizarStockCriticoAsync(datosParaIA);

            return Ok(sugerencia);
        }

        [HttpPost("recomendacion-caja")]
        public async Task<IActionResult> GetRecomendacion([FromBody] RecomendacionRequest req)
        {
            RecomendacionSalidaDto recomendacion = await _aiService.RecomendarProductosAsync(req.TipoCliente, req.Productos);

            return Ok(recomendacion);
        }
    }

    public class RecomendacionRequest
    {
        public string TipoCliente { get; set; }
        public List<string> Productos { get; set; }
    }
}
