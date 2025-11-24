using Newtonsoft.Json;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;

namespace ProyectoInventariosWebApi.Services
{
    public class AIGeminiService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;

        private const string BaseUrl = "https://generativelanguage.googleapis.com/v1/models/gemini-2.5-flash:generateContent";

        public AIGeminiService(string apiKey)
        {
            _httpClient = new HttpClient();
            _apiKey = apiKey;
        }

        private async Task<string> CallGeminiApiAsync(string prompt)
        {
            var requestBody = new
            {
                contents = new[]
                {
                new { role = "user", parts = new[] { new { text = prompt } } }
            }
            };

            var json = JsonConvert.SerializeObject(requestBody);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync($"{BaseUrl}?key={_apiKey}", content);

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                return $"Error API: {response.StatusCode} - {errorContent}";
            }

            var responseString = await response.Content.ReadAsStringAsync();
            dynamic? result = JsonConvert.DeserializeObject(responseString);

            var textResult = (string?)result?.candidates[0].content.parts[0].text;

            if (textResult == null) return "Error: La respuesta de la IA no contiene texto válido.";

            var match = Regex.Match(textResult, @"(?s)\{.*\}");
            return match.Success ? match.Value : textResult;
        }

        public async Task<AnalisisVentasSalidaDto> AnalizarVentasAsync(List<VentaAIDto> datos)
        {
            var jsonDatos = JsonConvert.SerializeObject(datos);
            var prompt = new StringBuilder()
                .AppendLine("Eres un Analista de Datos experto. Analiza el siguiente JSON de registros de ventas de una universidad. Identifica patrones, tendencias de compra por tipo de cliente (Estudiante/Docente) y sugiere 2-3 recomendaciones concretas.")
                .AppendLine("---")
                .AppendLine("DATOS DE VENTA:")
                .AppendLine(jsonDatos)
                .AppendLine("---")
                .AppendLine("Responde *solamente* con un objeto JSON válido, que contenga una lista de 'HallazgosClave' y una lista de 'Recomendaciones'. NO uses texto, preámbulos, ni envoltorios (como ```json).")
                .AppendLine("FORMATO DE SALIDA REQUERIDO (JSON ESTRICTO):")
                .AppendLine(JsonConvert.SerializeObject(new AnalisisVentasSalidaDto(), Formatting.None))
                .ToString();

            var jsonResponse = await CallGeminiApiAsync(prompt);

            try
            {
                return JsonConvert.DeserializeObject<AnalisisVentasSalidaDto>(jsonResponse) ?? new AnalisisVentasSalidaDto();
            }
            catch (Exception)
            {
                return new AnalisisVentasSalidaDto { HallazgosClave = new List<string> { $"Error al parsear el JSON de la IA. Respuesta cruda: {jsonResponse}" } };
            }
        }

        public async Task<StockSalidaDto> AnalizarStockCriticoAsync(List<StockAIDto> datos)
        {
            var jsonDatos = JsonConvert.SerializeObject(datos);
            var prompt = new StringBuilder()
                .AppendLine("Eres un Jefe de Compras y Logística de una universidad. Analiza el siguiente inventario. Para los ítems donde Stock Actual <= Stock Mínimo, calcula la 'Cantidad a Comprar' (Stock Máximo - Stock Actual) y el 'Costo Estimado' (Cantidad a Comprar * Costo Promedio Unitario).")
                .AppendLine("---")
                .AppendLine("INVENTARIO CRÍTICO:")
                .AppendLine(jsonDatos)
                .AppendLine("---")
                .AppendLine("Responde *solamente* con un objeto JSON válido. El objeto debe contener el 'ResumenCosto' total y una lista de 'ItemsCriticos' (solo los que cumplen la condición de compra). NO uses texto, preámbulos, ni envoltorios.")
                .AppendLine("FORMATO DE SALIDA REQUERIDO (JSON ESTRICTO):")
                .AppendLine(JsonConvert.SerializeObject(new StockSalidaDto
                {
                    ResumenCosto = 0,
                    ItemsCriticos = new List<ItemCriticoSalidaDto> { new ItemCriticoSalidaDto() }
                }, Formatting.None))
                .ToString();

            var jsonResponse = await CallGeminiApiAsync(prompt);

            try
            {
                return JsonConvert.DeserializeObject<StockSalidaDto>(jsonResponse) ?? new StockSalidaDto();
            }
            catch (Exception)
            {
                return new StockSalidaDto { ItemsCriticos = new List<ItemCriticoSalidaDto> { new ItemCriticoSalidaDto { Justificacion = $"Error al parsear el JSON de la IA. Respuesta cruda: {jsonResponse}" } } };
            }
        }

        public async Task<RecomendacionSalidaDto> RecomendarProductosAsync(string tipoCliente, List<string> productosEnCarrito)
        {
            var prompt = new StringBuilder()
                .AppendLine("Eres un experto en ventas cruzadas para una tienda universitaria. El cliente es tipo '").Append(tipoCliente).AppendLine("' y lleva los siguientes productos en su carrito: ").Append(string.Join(", ", productosEnCarrito)).AppendLine(".")
                .AppendLine("---")
                .AppendLine("Sugiérele de 1 a 2 productos complementarios que aumenten el ticket promedio. Inventa una 'FraseVenta' persuasiva para el cajero.")
                .AppendLine("---")
                .AppendLine("Responde *solamente* con un objeto JSON válido. NO uses texto, preámbulos, ni envoltorios (como ```json).")
                .AppendLine("FORMATO DE SALIDA REQUERIDO (JSON ESTRICTO):")
                .AppendLine(JsonConvert.SerializeObject(new RecomendacionSalidaDto(), Formatting.None))
                .ToString();

            var jsonResponse = await CallGeminiApiAsync(prompt);

            try
            {
                return JsonConvert.DeserializeObject<RecomendacionSalidaDto>(jsonResponse) ?? new RecomendacionSalidaDto();
            }
            catch (Exception)
            {
                return new RecomendacionSalidaDto { ProductosSugeridos = new List<string> { $"Error al parsear el JSON de la IA. Respuesta cruda: {jsonResponse}" } };
            }
        }
    }

    public class GeminiResponse { [JsonPropertyName("candidates")] public List<Candidate> Candidates { get; set; } }
    public class Candidate { [JsonPropertyName("content")] public Content Content { get; set; } }
    public class Content { [JsonPropertyName("parts")] public List<Part> Parts { get; set; } }
    public class Part { [JsonPropertyName("text")] public string Text { get; set; } }
}
