using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Sistema_CIN.Data;
using Sistema_CIN.Filters;
using Sistema_CIN.Models;
using Sistema_CIN.Services;


namespace Sistema_CIN.Controllers
{

	public class PersonalController : Controller
	{
		private readonly SistemaCIN_dbContext _context;
        private readonly FiltrosPermisos _filters;

        public PersonalController(SistemaCIN_dbContext context, FiltrosPermisos filtro)
		{
			_context = context;
            _filters = filtro;
        }

		// Funcion para verificar si el usuario en sesion tiene permiso para acceder a los modulos
        private async Task<bool> VerificarPermiso(int idOp)
        {
            string emailUser = User.FindFirst(ClaimTypes.Email)?.Value ?? "desconocido";
            int cantidadOperaciones = await _filters.VerificarPermiso(emailUser, idOp);
            return cantidadOperaciones > 0;
        }

        // GET: Personal
        public async Task<IActionResult> Index(string buscarEmpleado)
		{
            if (!await VerificarPermiso(1))
            {
                return RedirectToAction("AccessDenied", "Cuenta");
            }
            var empleado = from personal in _context.Personals select personal;
			empleado = _context.Personals.Include(p => p.IdRolNavigation);
	

			if(empleado.Count() < 1)
			{
				ModelState.AddModelError("", "No existe Personal");
			}

            if (!String.IsNullOrEmpty(buscarEmpleado))
            {
                empleado = empleado.Where(s => s.NombreP!.Contains(buscarEmpleado));
            }
            else
            {
                return View(await empleado.ToListAsync());
            }

            return View(await empleado.ToListAsync());
        }


		// GET: Personal/Details/5
	
		public async Task<IActionResult> Details(int? id)
		{
            if (!await VerificarPermiso(1))
            {
                return RedirectToAction("AccessDenied", "Cuenta");
            }
            if (id == null || _context.Personals == null)
			{
				return NotFound();
			}

			var personal = await _context.Personals
				.Include(p => p.IdRolNavigation)
				.FirstOrDefaultAsync(m => m.IdPersonal == id);
			if (personal == null)
			{
				return NotFound();
			}

			return View(personal);
		}

		// GET: Personal/Create

		public IActionResult Create()
		{

			ViewData["IdRol"] = new SelectList(_context.Rols, "IdRol", "NombreRol");
			return View();


		}


		// POST: Personal/Create
		// To protect from overposting attacks, enable the specific properties you want to bind to.
		// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.


		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create([Bind("IdPersonal,CedulaP,NombreP,ApellidosP,CorreoP,PuestoP,FechaNaceP,EdadP,GeneroP,ProvinciaP,CantonP,DistritoP,TelefonoP,IdRol")] Personal personal)
		{
            if (!await VerificarPermiso(2))
            {
                return RedirectToAction("AccessDenied", "Cuenta");
            }
            if (ModelState.IsValid)
			{

				var existeCedula = await _context.Personals.FirstOrDefaultAsync(r => r.CedulaP == personal.CedulaP);
				var existeTel = await _context.Personals.FirstOrDefaultAsync(r => r.TelefonoP == personal.TelefonoP);

				if (existeCedula != null)
				{
					ModelState.AddModelError("", "Esta cédula ya está en uso");

					return View(personal);
				}

				if (existeTel != null)
				{
					ModelState.AddModelError("TelefonoP", "Este número de teléfono ya está en uso");

					return View(personal);
				}

				_context.Add(personal);
				await _context.SaveChangesAsync();
				TempData["SuccessMessage"] = "Personal registrado exitosamente!";
				return RedirectToAction(nameof(Index));
			}


			ViewData["IdRol"] = new SelectList(_context.Rols, "IdRol", "IdRol", personal.IdRol);
			return View(personal);
		}



		// GET: Personal/Edit/5

		public async Task<IActionResult> Edit(int? id)
		{
            if (!await VerificarPermiso(3))
            {
                return RedirectToAction("AccessDenied", "Cuenta");
            }
            if (id == null)
			{
				return NotFound();
			}

			var personal = await _context.Personals
				.Include(u => u.IdRolNavigation)
				.FirstOrDefaultAsync(m => m.IdPersonal == id);

			if (personal == null)
			{
				return NotFound();
			}

			ViewData["IdRol"] = new SelectList(_context.Rols, "IdRol", "NombreRol", personal.IdRol);
			return View(personal);
		}

		// POST: Personal/Edit/5

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(int id, [Bind("IdPersonal,CedulaP,NombreP,ApellidosP,CorreoP,FechaNaceP,EdadP,GeneroP,ProvinciaP,CantonP,DistritoP,TelefonoP,IdRol")] Personal personal)
		{
            if (!await VerificarPermiso(3))
            {
                return RedirectToAction("AccessDenied", "Cuenta");
            }

            if (id != personal.IdPersonal)
			{
				return NotFound();
			}
			if (ModelState.IsValid)
			{
				try
				{
					var existeCelular = await _context.Personals.AsNoTracking().FirstOrDefaultAsync(r => r.TelefonoP == personal.TelefonoP);

					if (existeCelular != null)
					{
						// Si el nombre de rol ya existe y no es el mismo que el original, mostrar un error
						if (existeCelular.IdPersonal != personal.IdPersonal)
						{
							ModelState.AddModelError("", "Este número de celular ya está en uso.");
							return View(personal);
						}
					}

					_context.Update(personal);
					await _context.SaveChangesAsync();

					TempData["SuccessMessage"] = "Personal " + personal.NombreP + " actualizado exitosamente!";

				}
				catch (DbUpdateConcurrencyException)
				{
					if (!PersonalExists(personal.IdPersonal))
					{
						return NotFound();
					}
					else
					{
						throw;
					}
				}
				return RedirectToAction(nameof(Index));
			}
			ViewData["IdRol"] = new SelectList(_context.Rols, "IdRol", "IdRol", personal.IdRol);
			return View(personal);

		}

        // POST: Personal/Delete
        [HttpPost]

		public async Task<IActionResult> Delete(int id)
        {
            if (!await VerificarPermiso(5))
            {
                return RedirectToAction("AccessDenied", "Cuenta");
            }

            if (_context.Personals == null)
            {
                return Problem("Entity set 'CINContext.Personal'  is null.");
            }
            var personal = await _context.Personals.FindAsync(id);
            if (personal == null)
            {
                return NotFound();
            }

            _context.Personals.Remove(personal);
            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = "Personal " + personal.NombreP + " eliminado exitosamente!";


            return Json(new { success = true });
        }


        private bool PersonalExists(int id)
		{
			return (_context.Personals?.Any(e => e.IdPersonal == id)).GetValueOrDefault();
		}
	}
}
