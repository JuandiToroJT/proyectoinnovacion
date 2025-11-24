using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using ProyectoInventariosWebApp.Models;

namespace ProyectoInventariosWebApp.Filtro
{
    public class AutenticadoAdministradorAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!UsuarioLogueado.Id.HasValue ||
                (UsuarioLogueado.Rol != "Administrador" &&
                 UsuarioLogueado.Rol != "SuperAdmin" &&
                 UsuarioLogueado.Rol != "AdminSede"))
            {
                context.Result = new RedirectToActionResult("Login", "Account", null);
            }
            base.OnActionExecuting(context);
        }
    }

    public class AutenticadoEmpleadoAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!UsuarioLogueado.Id.HasValue ||
                (UsuarioLogueado.Rol != "Empleado" &&
                 UsuarioLogueado.Rol != "EncargadoDependencia" &&
                 UsuarioLogueado.Rol != "AdminSede" &&
                 UsuarioLogueado.Rol != "SuperAdmin" &&
                 UsuarioLogueado.Rol != "Administrador"))
            {
                context.Result = new RedirectToActionResult("Login", "Account", null);
            }
            base.OnActionExecuting(context);
        }
    }

    public class AutenticadoSuperAdminAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!UsuarioLogueado.Id.HasValue || UsuarioLogueado.Rol != "SuperAdmin")
            {
                context.Result = new RedirectToActionResult("Login", "Account", null);
            }
            base.OnActionExecuting(context);
        }
    }

    public class AutenticadoAdminSedeAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!UsuarioLogueado.Id.HasValue ||
                (UsuarioLogueado.Rol != "AdminSede" && UsuarioLogueado.Rol != "SuperAdmin"))
            {
                context.Result = new RedirectToActionResult("Login", "Account", null);
            }
            base.OnActionExecuting(context);
        }
    }
}