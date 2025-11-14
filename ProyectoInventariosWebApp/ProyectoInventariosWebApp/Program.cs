using ProyectoInventariosWebApp.Helpers;
using ProyectoInventariosWebApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddHttpContextAccessor();

builder.Services.AddHttpClient();

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

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Login}/{id?}");

app.Run();