using Microsoft.AspNetCore.Mvc;
using Sistema_CIN.Models;
using Sistema_CIN.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Data.Common;
using Microsoft.AspNetCore.Authentication;



namespace Sistema_CIN.Controllers
{

	public class CuentaController : Controller
	{
		private readonly SistemaCIN_dbContext _context;

		public CuentaController(SistemaCIN_dbContext context)
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

					if (usuario.Clave != usuario.ConfirmarClave)
					{

						return View(usuario);
					}

					var userRol = await _context.Rols
						.FirstOrDefaultAsync(r => r.NombreRol == "Invitado");

					if (userRol != null)
					{
						usuario.IdRol = userRol.IdRol;
						usuario.EstadoU = true;
						usuario.AccesoU = true;
					}

					_context.Add(usuario);
					await _context.SaveChangesAsync();

					var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
					identity.AddClaim(new Claim(ClaimTypes.Name, usuario.NombreU));
					identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, usuario.IdUsuario.ToString()));
					identity.AddClaim(new Claim(ClaimTypes.Role, "Invitado"));

					await HttpContext.SignInAsync(
						CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity));


					return RedirectToAction("Index", "Home");
				}
				return View(usuario);

			}
			catch (DbException)
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
				var usuarioRegistrado = await _context.Usuarios
					.FirstOrDefaultAsync(u => u.CorreoU == correo.Trim() && u.Clave == clave.Trim());
		
				if (usuarioRegistrado != null)
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

				ModelState.AddModelError("", "Correo o contraseña inválidas");
				return View();
			}
			catch (Exception)
			{
				throw;
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

			return View(usuario);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> EditProfile(int id, Usuario usuario)
		{
			if (id != usuario.IdUsuario)
			{
				return NotFound();
			}
			try
			{
				_context.Update(usuario);
				await _context.SaveChangesAsync();
				TempData["SuccessMessage"] = "Tu perfil fue actualizado exitosamente!";
				return View();
			}
			catch (DbUpdateConcurrencyException)
			{
				if (!UsuarioExists(usuario.IdUsuario))
				{
					ModelState.AddModelError("", "Error al obtener el Usuario");
					return View();
				}
				else
				{
					throw;
				}
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
