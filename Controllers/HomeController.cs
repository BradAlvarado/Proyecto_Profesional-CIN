using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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
        private readonly IWebHostEnvironment _hostingEnvironment;

        public HomeController(SistemaCIN_dbContext context, FiltrosPermisos filtro, IWebHostEnvironment hostingEnvironment)
        {
            _context = context;
            _filters = filtro;
            _hostingEnvironment = hostingEnvironment;
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

        public IActionResult AcercaDe()
        {
            return View();
        }
       
        public async Task<IActionResult> Ayuda()
        {
            if (!await VerificarPermiso(18))
            {
                return RedirectToAction("AccessDenied", "Cuenta");
            }
            var pdfFilePath = Path.Combine(_hostingEnvironment.ContentRootPath, "./Manual/Manual de Usuario CIN.pdf");

            // Verificar si el archivo existe
            if (!System.IO.File.Exists(pdfFilePath))
            {
                return NotFound();
            }

            // Leer el contenido del archivo PDF
            var pdfFileStream = System.IO.File.OpenRead(pdfFilePath);

            // Determinar el tipo de contenido
            var contentType = "application/pdf";

            // Retornar el archivo PDF como resultado de la acción
            return File(pdfFileStream, contentType);
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