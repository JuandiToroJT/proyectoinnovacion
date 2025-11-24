namespace ProyectoInventariosWebApi.Services
{
    public class AppSettings
    {
        public ConnectionStringsSettings ConnectionStrings { get; set; } = new ConnectionStringsSettings();

        public GeminiSettings Gemini { get; set; } = new GeminiSettings();
    }

    public class ConnectionStringsSettings
    {
        public string DefaultConnection { get; set; } = string.Empty;
    }

    public class GeminiSettings
    {
        public string ApiKey { get; set; } = string.Empty;
    }
}
