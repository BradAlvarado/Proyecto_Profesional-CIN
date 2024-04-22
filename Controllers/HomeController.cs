using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sistema_CIN.Data;
using Sistema_CIN.Models;
using Sistema_CIN.Services;
using System.Diagnostics;
using System.Security.Claims;

namespace Sistema_CIN.Controllers
{

    [Authorize]
    public class HomeController : Controller
    {
        private readonly SistemaCIN_dbContext _context;
        private readonly FiltrosPermisos _filters;

        public HomeController(SistemaCIN_dbContext context, FiltrosPermisos filtro)
        {
            _context = context;
            _filters = filtro;
        }

        private async Task<bool> VerificarPermiso(int idOp)
        {
            string emailUser = User.FindFirst(ClaimTypes.Email)?.Value ?? "desconocido";
            int cantidadOperaciones = await _filters.VerificarPermiso(emailUser, idOp);
            return cantidadOperaciones > 0;
        }



        public IActionResult Index()
        {
          
            string userRol = User.FindFirst(ClaimTypes.Role)?.Value ?? "desconocido";

            int personalCount = _context.Personals.Count();
            int pmeCount = _context.Pmes.Count();
            int encargadosCount = _context.Encargados.Count();
            int usuariosCount = _context.Usuarios.Count();


            ViewData["PersonalCount"] = personalCount;
            ViewData["PmeCount"] = pmeCount;
            ViewData["EncargadosCount"] = encargadosCount;

            if(userRol == "Administrador") {

                ViewData["UsuariosCount"] = usuariosCount;
            }
            else
            {
                ViewData["UsuariosCount"] = -1;
            }
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult NotFound()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}