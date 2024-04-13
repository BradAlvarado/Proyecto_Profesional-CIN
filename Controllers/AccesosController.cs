using Microsoft.AspNetCore.Mvc;

namespace Sistema_CIN.Controllers
{
    public class AccesosController : Controller
    {
        public IActionResult Login()
        {
            return View();
        }

        public IActionResult Register()
        {
            return View();
        }

        public IActionResult Logout()
        {
            return View();
        }
    }
}
