using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sistema_CIN.Data;
using Sistema_CIN.Models;
using System.Diagnostics;

namespace Sistema_CIN.Controllers
{
    public class HomeController : Controller
    {
        private readonly CIN_pruebaContext _context;
        private readonly ILogger<HomeController> _logger;

        public HomeController(CIN_pruebaContext context, ILogger<HomeController> logger)
        {
            _context = context;
            _logger = logger;
        }

        public IActionResult Index()
        {
            int personalCount = _context.Personals.Count();
            int pmeCount = _context.Pmes.Count();
            int encargadosCount = _context.Encargados.Count();
            int usuariosCount = _context.Usuarios.Count();

            ViewData["PersonalCount"] = personalCount;
            ViewData["PmeCount"] = pmeCount;
            ViewData["EncargadosCount"] = encargadosCount;
            ViewData["UsuariosCount"] = usuariosCount;

            return View();
        }

        public IActionResult Privacy()
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