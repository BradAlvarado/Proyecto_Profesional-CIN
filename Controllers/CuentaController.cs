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

        public CuentaController(SistemaCIN_dbContext context)
        {
            _context = context;
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

                    if (usuario.Clave != usuario.ConfirmarClave)
                    {

                        return View(usuario);
                    }

                    usuario.Clave = PasswordHelper.EncryptPassword(usuario.Clave);
                    usuario.FotoU = "~/images/Default_Profile_photo.jpg";
                    var userRol = await _context.Rols
                        .FirstOrDefaultAsync(r => r.NombreRol == "Invitado");

                    if (userRol != null)
                    {
                        usuario.IdRol = userRol.IdRol;
                        usuario.EstadoU = true;
                        usuario.AccesoU = true;
                    }
                    var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
                    identity.AddClaim(new Claim(ClaimTypes.Email, usuario.CorreoU));
                    identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, usuario.IdUsuario.ToString()));
                    identity.AddClaim(new Claim(ClaimTypes.Name, usuario.NombreU));

                    // Obtener el rol del usuario y agregarlo como reclamación
                    var rolUser = await _context.Rols.FirstOrDefaultAsync(r => r.IdRol == usuario.IdRol);
                    if (rolUser != null)
                    {
                        identity.AddClaim(new Claim(ClaimTypes.Role, rolUser.NombreRol));
                    }
                    _context.Add(usuario);
                    await _context.SaveChangesAsync();
                    // Inicia Sesión
                    await HttpContext.SignInAsync(
                        CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity));


                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("", "Ingrese los datos");
                    return View(usuario);
                }


            }
            catch (DbException ex)
            {
                ModelState.AddModelError("", "Error al registrarse " + ex);
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
                var usuarioRegistrado = await _context.Usuarios.FirstOrDefaultAsync(u => u.CorreoU == correo);

                if (correo == null || clave == null)
                {
                    ModelState.AddModelError("", "Correo o contraseña sin ingresar");
                    return View();
                }

                if (usuarioRegistrado != null)
                {
                    // Encriptar la contraseña ingresada por el usuario para compararla con la contraseña encriptada almacenada
                    string claveEncriptada = PasswordHelper.EncryptPassword(clave);

                    if (claveEncriptada == usuarioRegistrado.Clave)
                    {
                        if (usuarioRegistrado.AccesoU == true)
                        {
                            // Actualiza el campo EstadoU a true para el usuario que ha iniciado sesión
                            usuarioRegistrado.EstadoU = true;
                            _context.Update(usuarioRegistrado);
                            await _context.SaveChangesAsync();

                            // Creación de las reclamaciones del usuario para la autenticación
                            var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
                            identity.AddClaim(new Claim(ClaimTypes.Email, usuarioRegistrado.CorreoU));
                            identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, usuarioRegistrado.IdUsuario.ToString()));
                            identity.AddClaim(new Claim(ClaimTypes.Name, usuarioRegistrado.NombreU));

                            // Obtener el rol del usuario y agregarlo como reclamación
                            var rolUser = await _context.Rols.FirstOrDefaultAsync(r => r.IdRol == usuarioRegistrado.IdRol);
                            if (rolUser != null)
                            {
                                identity.AddClaim(new Claim(ClaimTypes.Role, rolUser.NombreRol));
                            }

                            // Iniciar sesión
                            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity));
                            return RedirectToAction("Index", "Home");
                        }
                        else
                        {
                            ModelState.AddModelError("", "No tienes acceso al Sistema. Consulta con el administrador");
                            return View();
                        }
                    }
                }

                ModelState.AddModelError("", "Correo o contraseña inválidas");
                return View();
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

            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null)
            {
                return NotFound();
            }

            ViewData["IdRol"] = new SelectList(_context.Rols, "IdRol", "NombreRol");
            return View(usuario);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditProfile(Usuario usuario, IFormFile photoFile)
        {
            try
            {
                if (usuario != null)
                {
                    var existeCorreo = await _context.Usuarios.FirstOrDefaultAsync(r => r.CorreoU == usuario.CorreoU && r.IdUsuario != usuario.IdUsuario);

                    if (existeCorreo != null)
                    {
                        ModelState.AddModelError("", "Este correo ya está en uso");
                        return View(usuario);
                    }

                    if (!string.IsNullOrEmpty(usuario.Clave))
                    {
                        if (usuario.Clave != usuario.ConfirmarClave)
                        {
                            ModelState.AddModelError("", "Las contraseñas no coinciden");
                            return View(usuario);
                        }
                    }

                    // Manejar la carga de la imagen de perfil
                    if (photoFile != null && photoFile.Length > 0)
                    {
                        // Guardar la imagen en tu sistema de archivos
                        var imagePath = "path/to/save/images"; // Reemplaza con la ruta donde deseas guardar las imágenes
                        var fileName = Guid.NewGuid().ToString() + Path.GetExtension(photoFile.FileName);
                        var filePath = Path.Combine(imagePath, fileName);
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await photoFile.CopyToAsync(stream);
                        }
                        // Actualizar el camino de la foto en el modelo de usuario
                        usuario.FotoU = filePath;
                    }

                    // Actualizar el usuario en la base de datos
                    _context.Update(usuario);
                    await _context.SaveChangesAsync();

                    TempData["SuccessMessage"] = "Perfil actualizado exitosamente!";
                    return RedirectToAction(nameof(Index), "Home");
                }
                else
                {
                    ModelState.AddModelError("", "Ingrese los datos");
                    return View(usuario);
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Error al actualizar el perfil " + ex.Message);
                return View(usuario);
            }
        }


        private bool UsuarioExists(int id)
        {
            return (_context.Usuarios?.Any(e => e.IdUsuario == id)).GetValueOrDefault();
        }


        [AllowAnonymous]
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
