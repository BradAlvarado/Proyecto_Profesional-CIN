using Microsoft.AspNetCore.Mvc;
using Sistema_CIN.Models;
using Sistema_CIN.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Data.Common;
using Microsoft.AspNetCore.Authentication;


namespace Sistema_CIN.Controllers
{
    public class CuentaController : Controller
    {
        private readonly CIN_pruebaContext _context;

        public CuentaController(CIN_pruebaContext context)
        {
            _context = context;
        }

        // GET /Register
        [AllowAnonymous]
        public IActionResult Register()
        {
            return View();
        }

        // POST /Register
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Register(Usuario usuario)
        {
            try
            {
                if (usuario != null)
                {
                    var existeCorreo = await _context.Usuarios.FirstOrDefaultAsync(r => r.CorreoU == usuario.CorreoU);

                    if (existeCorreo != null)
                    {
                        ModelState.AddModelError("", "Este correo ya está en uso");

                        return View(usuario);

                    }

                    var userRol = await _context.Roles.FirstOrDefaultAsync(r => r.NombreRol == "Invitado");

                    if (userRol != null)
                    {
                        usuario.IdRol = userRol.IdRol;
                    }

                    usuario.AccesoU = false;

                    _context.Add(usuario);
                    await _context.SaveChangesAsync();

                    var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
                    identity.AddClaim(new Claim(ClaimTypes.Name, usuario.NombreU));
                    identity.AddClaim(new Claim(ClaimTypes.Role, "Invitado"));

                    await HttpContext.SignInAsync(
                        CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity));


                    return RedirectToAction("Index", "Home");
                }
                return View(usuario);

            }
            catch (DbException dbEx)
            {

                return View(usuario);
            }
        }


        // GET /Login
        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }

        // POST /Register
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Login(string correo, string clave)
        {
            try
            {
                var usuario = await _context.Usuarios.
                    FirstOrDefaultAsync(u => u.CorreoU == correo && u.Clave == clave);
                

                if (usuario != null)
                {
                    var identity = new ClaimsIdentity(
                        CookieAuthenticationDefaults.AuthenticationScheme
                    );
                    identity.AddClaim(new Claim(ClaimTypes.Email, usuario.CorreoU));
                    identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, usuario.IdUsuario.ToString()));

                    var rolUser = await _context.Roles.FirstOrDefaultAsync(r => r.IdRol == usuario.IdRol);

                    if (rolUser != null)
                    {
                        identity.AddClaim(
                            new Claim(ClaimTypes.Role, rolUser.NombreRol)
                            );
                        HttpContext.Session.SetString("NombreRol", rolUser.NombreRol); // Guardar el nombre del rol en la sesión
                    }

                    await HttpContext.SignInAsync(
                       CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity));

                    return RedirectToAction("Index", "Home");
                }
                ModelState.AddModelError("", "Credenciales inválidas!");
                return View();
            }
            catch (Exception)
            {

                throw;
            }
            
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }

        [AllowAnonymous]
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
