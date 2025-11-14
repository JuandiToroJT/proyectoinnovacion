using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using ProyectoInventariosWebApp.Models;

namespace ProyectoInventariosWebApp.Filtro
{
    public class AutenticadoAdministradorAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!UsuarioLogueado.Id.HasValue || UsuarioLogueado.Rol != "Administrador")
            {
                context.Result = new RedirectToActionResult("Login", "Home", null);
            }
            base.OnActionExecuting(context);
        }
    }

    public class AutenticadoEmpleadoAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!UsuarioLogueado.Id.HasValue || UsuarioLogueado.Rol != "Empleado")
            {
                context.Result = new RedirectToActionResult("Login", "Home", null);
            }
            base.OnActionExecuting(context);
        }
    }
}
