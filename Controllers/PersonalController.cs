using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing.Printing;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using PdfSharpCore.Drawing;
using PdfSharpCore.Pdf;
using Sistema_CIN.Data;
using Sistema_CIN.Models;
using PdfSharpCore.Drawing;
using PdfSharpCore.Pdf;



namespace Sistema_CIN.Controllers
{
    [Authorize]
    public class PersonalController : Controller
	{
		private readonly CIN_pruebaContext _context;

		public PersonalController(CIN_pruebaContext context)
		{
			_context = context;
		}

		// GET: Personal
		public async Task<IActionResult> Index(string buscarEmpleado, int? page, string sortOrder)
		{
			var pageNumber = page ?? 1; // Número de página actual
			var pageSize = 10; // Número de elementos por página


			var empleado = from personal in _context.Personals select personal;
			empleado = _context.Personals.Include(p => p.IdRolNavigation);


			if (empleado.Count() < 1)
			{
				ModelState.AddModelError("", "No existen PME registrados");
			}

			if (!String.IsNullOrEmpty(buscarEmpleado))
			{
				empleado = empleado.Where(s => s.NombreP!.Contains(buscarEmpleado));
			}


			//Filtro A-Z
			// Establece el valor predeterminado para AgeSortParm
			ViewData["NameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "name_asc" : "";

			if (sortOrder == "name_asc")
			{
				empleado = empleado.OrderBy(p => p.NombreP);
			}
			if (sortOrder == "name_des")
			{
				empleado = empleado.OrderByDescending(p => p.NombreP);
			}

			//Filtro EDAD
			// Establece el valor predeterminado para AgeSortParm
			ViewData["AgeSortParm"] = String.IsNullOrEmpty(sortOrder) ? "edad_asc" : "";

			if (sortOrder == "edad_asc")
			{
				empleado = empleado.OrderBy(p => p.EdadP);
			}
			if (sortOrder == "edad_des")
			{
				empleado = empleado.OrderByDescending(p => p.EdadP);
			}



			// Paginar los resultados
			var pagedempleado = await empleado.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();

			// Calcular el número total de páginas
			var totalItems = await empleado.CountAsync();
			var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);
            // Crear un objeto de modelo para la paginación
            var pagedModel = new PagedList<Personal>(pagedempleado, pageNumber, pageSize, totalItems, totalPages);

            return View(pagedModel);
        }

        public async Task<IActionResult> Export(string buscarEmpleado, int? page, string sortOrder) {
            
			var empleado = from personal in _context.Personals select personal;
            empleado = _context.Personals.Include(p => p.IdRolNavigation);


            var document = new PdfDocument();
            var pagepdf = document.AddPage();
            var gfx = XGraphics.FromPdfPage(pagepdf);
            var fontTitle = new XFont("Arial", 16, XFontStyle.Bold);
            var fontData = new XFont("Arial", 12, XFontStyle.Regular);
            var fontBody = new XFont("Arial", 12, XFontStyle.Regular);

            // Encabezado del documento PDF
            gfx.DrawString("Lista de Personas", fontTitle, XBrushes.Black, new XRect(50, 50, pagepdf.Width, pagepdf.Height), XStringFormats.TopLeft);

            // Obtener datos de las personas filtradas y mostrar en el PDF
            int yPos = 100;
            foreach (var personal in empleado)
            {
                // Mostrar datos de cada persona
                gfx.DrawString($"Nombre: {personal.NombreP}", fontBody, XBrushes.Black, new XRect(50, yPos, pagepdf.Width, pagepdf.Height), XStringFormats.TopLeft);
                gfx.DrawString($"Edad: {personal.EdadP}", fontBody, XBrushes.Black, new XRect(yPos, yPos, pagepdf.Width, pagepdf.Height), XStringFormats.TopLeft);
                gfx.DrawString($"Rol: {personal.IdRolNavigation.NombreRol}", fontBody, XBrushes.Black, new XRect(450, yPos, pagepdf.Width, pagepdf.Height), XStringFormats.TopLeft);

                yPos += 20; // Incrementar la posición vertical para la siguiente fila
            }

            // Guardar el documento PDF en un MemoryStream
            var stream = new MemoryStream();
            document.Save(stream);
            stream.Position = 0;

            // Descargar el documento PDF como un archivo
            return File(stream, "application/pdf", "informacion_personal.pdf");



        }


        // GET: Personal/Details/5
        public async Task<IActionResult> Details(int? id)
		{
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

			ViewData["IdRol"] = new SelectList(_context.Roles, "IdRol", "NombreRol");
			return View();


		}


		// POST: Personal/Create
		// To protect from overposting attacks, enable the specific properties you want to bind to.
		// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create([Bind("IdPersonal,CedulaP,NombreP,ApellidosP,CorreoP,PuestoP,FechaNaceP,EdadP,GeneroP,ProvinciaP,CantonP,DistritoP,TelefonoP,IdRol")] Personal personal)
		{
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


			ViewData["IdRol"] = new SelectList(_context.Roles, "IdRol", "IdRol", personal.IdRol);
			return View(personal);
		}



		// GET: Personal/Edit/5
		public async Task<IActionResult> Edit(int? id)
		{
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

			ViewData["IdRol"] = new SelectList(_context.Roles, "IdRol", "NombreRol", personal.IdRol);
			return View(personal);
		}

		// POST: Personal/Edit/5

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(int id, [Bind("IdPersonal,CedulaP,NombreP,ApellidosP,CorreoP,FechaNaceP,EdadP,GeneroP,ProvinciaP,CantonP,DistritoP,TelefonoP,IdRol")] Personal personal)
		{
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
			ViewData["IdRol"] = new SelectList(_context.Roles, "IdRol", "IdRol", personal.IdRol);
			return View(personal);

		}

        // POST: Personal/Delete
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
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
