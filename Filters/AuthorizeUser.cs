using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using Sistema_CIN.Data;
using Sistema_CIN.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Sistema_CIN.Filters
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class AuthorizeUserAttribute : Attribute, IAsyncAuthorizationFilter
    {
        private readonly int _idOperacion;
        private readonly SistemaCIN_dbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public AuthorizeUserAttribute(int idOperacion = 0)
        {
            _idOperacion = idOperacion;
        }

        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            var usuario = (Usuario)context.HttpContext.Items["Usuario"];

            // Realizar las operaciones de autorización
            var misOperaciones = await _context.RolOperacions
                .Where(m => m.IdRol == usuario.IdRol && m.IdOp == _idOperacion)
                .ToListAsync();

            if (misOperaciones.Count == 0)
            {
                var oOperacion = await _context.Operaciones.FindAsync(_idOperacion);
                int? idModulo = oOperacion.IdModulo;

                context.Result = new RedirectResult("/Cuenta/AccessDenied");
            }
        }
    }
}



// Este medio funcionaba pero me daba error en ejecucion en: oUsuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.CorreoU == user);
//[AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
//public class AuthorizeUser : AuthorizeAttribute, IAsyncAuthorizationFilter
//{
//    private readonly SistemaCIN_dbContext _context;
//    private readonly int idOperacion;
//    private Usuario oUsuario;


//    public AuthorizeUser(int idOperacion = 0)
//    {
//        this.idOperacion = idOperacion;
//    }

//    public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
//    {
//        try
//        {
//            // Obtener el correo del usuario de la sesión

//            var user = context.HttpContext.User.ToString();
//            // Obtener el usuario correspondiente al correo
//            oUsuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.CorreoU == user);



//            // Realizar las operaciones de autorización
//            var misOperaciones = await _context.RolOperacions
//                .Where(m => m.IdRol == oUsuario.IdRol && m.IdOp == idOperacion)
//                .ToListAsync();

//            if (misOperaciones.Count() == 0)
//            {
//                var oOperacion = await _context.Operaciones.FindAsync(idOperacion);
//                int? idModulo = oOperacion?.IdModulo;

//                context.Result = new RedirectResult("/Cuenta/AccessDenied");
//            }
//        }
//        catch (Exception ex)
//        {
//            // Manejar la excepción
//            context.Result = new RedirectResult("/Cuenta/AccessDenied");
//        }
//    }
//}
