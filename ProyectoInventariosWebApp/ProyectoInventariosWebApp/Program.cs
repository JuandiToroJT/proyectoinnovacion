using ProyectoInventariosWebApp.Helpers;
using ProyectoInventariosWebApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddHttpContextAccessor();
builder.Services.AddHttpClient();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromHours(2);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

builder.Services.Configure<ApiUrlsOptions>(
    builder.Configuration.GetSection("ApiUrls"));

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var httpClient = scope.ServiceProvider.GetRequiredService<HttpClient>();
    Empresas empresaAct = null;
    var config = scope.ServiceProvider.GetRequiredService<IOptions<ApiUrlsOptions>>();
    var respuesta = await httpClient.GetAsync(config.Value.BaseUrl + "Empresas");

    if (respuesta.IsSuccessStatusCode)
    {
        var content = await respuesta.Content.ReadAsStringAsync();
        var empresas = JsonConvert.DeserializeObject<List<Empresas>>(content);
        if (empresas != null && empresas.Any())
            empresaAct = empresas.FirstOrDefault();
    }

    if (empresaAct != null)
    {
        EmpresaActual.Id = empresaAct.IdEmpresa;
        EmpresaActual.Nombre = empresaAct.Nombre;
        EmpresaActual.Correo = empresaAct.EmailContacto;
        EmpresaActual.Telefono = empresaAct.Telefono;
        EmpresaActual.Direccion = empresaAct.Direccion;
        EmpresaActual.Ciudad = empresaAct.Ciudad;
        EmpresaActual.PaginaWeb = empresaAct.PaginaWeb;
    }
}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}


app.UseStaticFiles();

app.UseRouting();

app.UseSession();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Login}/{id?}");

app.Run();
