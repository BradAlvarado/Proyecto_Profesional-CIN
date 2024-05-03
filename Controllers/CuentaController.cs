using Microsoft.AspNetCore.Mvc;
using Sistema_CIN.Models;
using Sistema_CIN.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Data.Common;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Text;
using System.Security.Cryptography;


namespace Sistema_CIN.Controllers
{

    public class CuentaController : Controller
    {
        private readonly SistemaCIN_dbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CuentaController(SistemaCIN_dbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        public static string EncryptPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                // Convertir la contraseña en un array de bytes
                byte[] bytes = Encoding.UTF8.GetBytes(password);

                // Calcular el hash
                byte[] hash = sha256.ComputeHash(bytes);

                // Convertir el hash en una cadena hexadecimal
                StringBuilder stringBuilder = new StringBuilder();
                for (int i = 0; i < hash.Length; i++)
                {
                    stringBuilder.Append(hash[i].ToString("x2"));
                }
                return stringBuilder.ToString();
            }
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

            var existeCorreo = await _context.Usuarios.FirstOrDefaultAsync(r => r.CorreoU == usuario.CorreoU);

            if (existeCorreo != null)
            {
                ModelState.AddModelError("", "Este correo ya está en uso.");
                return View();
            }

            if (usuario.CorreoU == null || usuario.Clave == null || usuario.NombreU == null || usuario.ConfirmarClave == null)
            {
                ModelState.AddModelError("", "Hay campos incompletos");
                return View();
            }

            try
            {
                var userRol = await _context.Rols.FirstOrDefaultAsync(r => r.NombreRol == "Invitado");
                if (userRol != null)
                {
                    usuario.IdRol = userRol.IdRol;
                }

                if (usuario.Clave != usuario.ConfirmarClave)
                {
                    ModelState.AddModelError("", "Contraseñas no coinciden");
                    return View(usuario);
                }

                // Asigno variables
                usuario.Clave = PasswordHelper.EncryptPassword(usuario.Clave);
                usuario.EstadoU = true;
                usuario.AccesoU = true;
                usuario.FotoU = "default-user-photo.jpg";
                _context.Add(usuario);
                await _context.SaveChangesAsync();

                var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
                identity.AddClaim(new Claim(ClaimTypes.Email, usuario.CorreoU));
                identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, usuario.IdUsuario.ToString()));
               
                // Obtener el rol del usuario y agregarlo como reclamación
                var rolUser = await _context.Rols.FirstOrDefaultAsync(r => r.IdRol == usuario.IdRol);
                if (rolUser != null)
                {
                    identity.AddClaim(new Claim(ClaimTypes.Role, rolUser.NombreRol));
                }
                // Obtener el variables de para mostrar en el layout
                HttpContext.Session.SetString("IdUsuario", usuario.IdUsuario.ToString());
                HttpContext.Session.SetString("CorreoU", usuario.CorreoU);
                HttpContext.Session.SetString("Rol", usuario.IdRolNavigation.NombreRol.ToString());
                HttpContext.Session.SetString("FotoU", usuario.FotoU);

          
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity));
                return RedirectToAction("Index", "Home");

            }
            catch (DbException ex)
            {
                ModelState.AddModelError("", "Error al registrarse: " + ex.Message);
            }
            return View(usuario);
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
            var usuarioRegistrado = await _context.Usuarios.FirstOrDefaultAsync(u => u.CorreoU == correo);

            if (correo == null)
            {

                ModelState.AddModelError("CorreoU", "Correo sin ingresar");
                return View();
            }

            if (clave == null)
            {

                ModelState.AddModelError("Clave", "Contraseña sin ingresar");
                return View();
            }

            if (usuarioRegistrado == null)
            {
                ModelState.AddModelError("", "Este usuario no existe");
                return View();
            }

            if (usuarioRegistrado.AccesoU != true)
            {
                ModelState.AddModelError("", "No tienes acceso al Sistema");
                return View();
            }

            if (usuarioRegistrado.Clave != PasswordHelper.EncryptPassword(clave))
            {
                ModelState.AddModelError("", "Contraseña incorrecta");
                return View();
            }

            try
            {
                // Actualiza el campo EstadoU a true para el usuario que ha iniciado 
                usuarioRegistrado.EstadoU = true;

                _context.Update(usuarioRegistrado);
                await _context.SaveChangesAsync();

                // Creación de las reclamaciones del usuario para la autenticación

                var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
                identity.AddClaim(new Claim(ClaimTypes.Email, usuarioRegistrado.CorreoU));
                identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, usuarioRegistrado.IdUsuario.ToString()));

                // Obtener el rol del usuario y agregarlo como reclamación
                var rolUser = await _context.Rols.FirstOrDefaultAsync(r => r.IdRol == usuarioRegistrado.IdRol);

                if (rolUser != null)
                {
                    identity.AddClaim(new Claim(ClaimTypes.Role, rolUser.NombreRol));
                }
                HttpContext.Session.SetString("CorreoU", usuarioRegistrado.CorreoU);
                HttpContext.Session.SetString("Rol", usuarioRegistrado.IdRolNavigation.NombreRol.ToString());
                HttpContext.Session.SetString("IdUsuario", usuarioRegistrado.IdUsuario.ToString());

                if (!string.IsNullOrEmpty(usuarioRegistrado.FotoU))
                {
                    HttpContext.Session.SetString("FotoU", usuarioRegistrado.FotoU);
                }


                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity));

                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Error al iniciar sesión " + ex);
                return View();
            }
        }



        // GET POST / Logout
        public async Task<IActionResult> Logout()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier); // Obtiene el ID del usuario actual
            var user = await _context.Usuarios.FindAsync(int.Parse(userId)); // Busca al usuario en la base de datos

            if (user != null)
            {
                user.EstadoU = false; // Actualiza la columna AccesoU
                _context.Update(user);
                await _context.SaveChangesAsync(); // Guarda los cambios en la base de datos
            }

            // Cierra la sesión del usuario
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            // Redirige a la página de Logout
            // Cierra sesión
            return View(); //Vista HTML Logout.cshtml <h2>Has cerrado sesión<h2>

        }

        // GET: Usuarios/Edit/5
        public async Task<IActionResult> EditProfile(int? id)
        {
            if (id == null || _context.Usuarios == null)
            {
                return NotFound();
            }

            var usuario = await _context.Usuarios
               .Include(u => u.IdRolNavigation)
               .FirstOrDefaultAsync(m => m.IdUsuario == id);

            if (usuario == null)
            {
                return NotFound();
            }


            ViewData["IdRol"] = new SelectList(_context.Rols, "IdRol", "NombreRol");
            return View(usuario);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditProfile(int id, Usuario usuario, IFormFile imageFile, string currentPassword, string newPassword, string confirmNewPassword, string borrar)
        {
            if (id != usuario.IdUsuario)
            {
                return NotFound();
            }

            try
            {
                var user = await _context.Usuarios.FindAsync(id);



                // Verificar la contraseña actual
                if (!string.IsNullOrEmpty(currentPassword) && !string.IsNullOrEmpty(newPassword) && !string.IsNullOrEmpty(confirmNewPassword))
                {

                    if (!string.IsNullOrEmpty(currentPassword) && user.Clave != PasswordHelper.EncryptPassword(currentPassword))
                    {
                        ModelState.AddModelError("CurrentPassword", "La contraseña actual es incorrecta.");
                        return View(usuario);
                    }

                    // Verificar que la nueva contraseña y la confirmación coincidan
                    if (!string.IsNullOrEmpty(newPassword) && newPassword != confirmNewPassword)
                    {
                        ModelState.AddModelError("ConfirmNewPassword", "Las contraseñas no coinciden.");
                        return View(usuario);
                    }

                    // Actualizar la contraseña
                    if (!string.IsNullOrEmpty(newPassword))
                    {
                        user.Clave = PasswordHelper.EncryptPassword(newPassword);
                    }
                }
                // actualiza el nombre
                user.NombreU = usuario.NombreU;


                // Subimos foto



                var files = HttpContext.Request.Form.Files;

                if (borrar == "true")
                {
                    user.FotoU = usuario.FotoU = "default-user-photo.jpg";
                }

                foreach (var formFile in files)
                {
                    if (formFile.Length > 0)
                    {
                        // Generar un nombre único para el archivo
                        var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(formFile.FileName);

                        var filePath = Path.Combine("wwwroot/images/imageUser", uniqueFileName);

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await formFile.CopyToAsync(stream);
                        }
                        usuario.FotoU = uniqueFileName;
                    }
                }
                // Si se ha seleccionado una nueva foto, actualizarla
                if (!string.IsNullOrEmpty(usuario.FotoU))
                {
                    HttpContext.Session.SetString("FotoU", usuario.FotoU);
                    user.FotoU = usuario.FotoU;
                }
                else
                {
                    // Si no se ha seleccionado una nueva foto, mantener la foto actual
                    usuario.FotoU = user.FotoU;
                }

                


                _context.Update(user);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Perfil actualizado exitosamente!";
                return RedirectToAction("Index", "Home");
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UsuarioExists(usuario.IdUsuario))
                {
                    return NotFound();
                }
                else
                {
                    ModelState.AddModelError("", "Error al actualizar tu perfil");
                    return RedirectToAction("EditProfile", "Cuenta");
                }
            }

        }


        private bool UsuarioExists(int id)
        {
            return _context.Usuarios.Any(e => e.IdUsuario == id);
        }


        [AllowAnonymous]
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
