using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc;

namespace ProyectoInventariosWebApp.Helpers
{
    public static class ModelStateExtensions
    {
        /// <summary>
        /// Lee los errores de Validación (ValidationProblemDetails) en la respuesta de la API y los agrega a ModelState.
        /// </summary>
        /// <param name="modelState">El ModelStateDictionary en el que se agregarán los errores.</param>
        /// <param name="response">El HttpResponseMessage devuelto por la llamada a la API.</param>
        /// <returns>Una tarea que termina cuando se hayan agregado los errores (si los hubo).</returns>
        public static async Task AddErrorsFromApiResponseAsync(this ModelStateDictionary modelState, HttpResponseMessage response)
        {
            if (response.Content == null)
            {
                modelState.AddModelError(string.Empty, $"Error en la llamada a la API: {response.StatusCode}");
                return;
            }

            ValidationProblemDetails problemDetails = null;
            try
            {
                problemDetails = await response.Content.ReadFromJsonAsync<ValidationProblemDetails>();
            }
            catch
            {
                modelState.AddModelError(string.Empty, $"Error al deserializar respuesta de la API: {response.StatusCode}");
                return;
            }

            if (problemDetails == null)
            {
                modelState.AddModelError(string.Empty, $"Error inesperado al leer detalles de validación de la API: {response.StatusCode}");
                return;
            }

            if (problemDetails.Errors != null && problemDetails.Errors.Count > 0)
            {
                foreach (var kvp in problemDetails.Errors)
                {
                    foreach (var mensaje in kvp.Value)
                    {
                        modelState.AddModelError(kvp.Key, mensaje);
                    }
                }
                return;
            }

            if (problemDetails.Extensions != null && problemDetails.Extensions.Count > 0)
            {
                foreach (var kvp in problemDetails.Extensions)
                {
                    string clave = kvp.Key;
                    object rawValue = kvp.Value;

                    if (rawValue is JsonElement je && je.ValueKind == JsonValueKind.Array)
                    {
                        foreach (var elem in je.EnumerateArray())
                        {
                            if (elem.ValueKind == JsonValueKind.String)
                            {
                                string msg = elem.GetString();
                                modelState.AddModelError(clave, msg);
                            }
                        }
                    }
                    else
                    {
                        modelState.AddModelError(string.Empty, rawValue?.ToString() ?? $"Error en campo '{clave}'");
                    }
                }
                return;
            }

            modelState.AddModelError(string.Empty, $"Error desconocido de la API: {response.StatusCode}");
        }
    }
}
