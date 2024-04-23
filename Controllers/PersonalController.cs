using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing.Printing;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Sistema_CIN.Data;
using Sistema_CIN.Models;
using Sistema_CIN.Services;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Hosting.Internal;




namespace Sistema_CIN.Controllers
{

    [Authorize]
    public class PersonalController : Controller
    {
        private readonly SistemaCIN_dbContext _context;
        private readonly FiltrosPermisos _filters;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public PersonalController(SistemaCIN_dbContext context, FiltrosPermisos filtro, IWebHostEnvironment hostingEnvironment)
        {
            _context = context;
            _filters = filtro;
            _hostingEnvironment = hostingEnvironment;
        }

        // Funcion para verificar si el usuario en sesion tiene permiso para acceder a los modulos
        private async Task<bool> VerificarPermiso(int idOp)
        {
            string emailUser = User.FindFirst(ClaimTypes.Email)?.Value ?? "desconocido";
            int cantidadOperaciones = await _filters.VerificarPermiso(emailUser, idOp);
            return cantidadOperaciones > 0;
        }

        // GET: Personal

        public async Task<IActionResult> Index(string buscarEmpleado, int? page)
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

            // Paginar los resultados
            var pagedempleado = await empleado.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();
            // Calcular el número total de páginas
            var totalItems = await empleado.CountAsync();
            var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

            // Crear un objeto de modelo para la paginación
            var pagedModel = new PagedList<Personal>(pagedempleado, pageNumber, pageSize, totalItems, totalPages);

            return View(pagedModel);
        }

        public ActionResult IndexPDF(string sortOrder)
        {
            var empleados = from personal in _context.Personals select personal;
            empleados = _context.Personals.Include(p => p.IdRolNavigation);

            if (empleados.Count() < 1)
            {
                ModelState.AddModelError("", "No existen PME registrados");
            }

            // Ordenar según el valor de sortOrder
            switch (sortOrder)
            {
                case "name_asc":
                    empleados = empleados.OrderBy(p => p.NombreP);
                    break;
                case "name_des":
                    empleados = empleados.OrderByDescending(p => p.NombreP);
                    break;
                case "edad_asc":
                    empleados = empleados.OrderBy(p => p.EdadP);
                    break;
                case "edad_des":
                    empleados = empleados.OrderByDescending(p => p.EdadP);
                    break;
                default:
                    // Orden predeterminado (por nombre ascendente)
                    empleados = empleados.OrderBy(p => p.NombreP);
                    break;
            }

            return View(empleados);
        }

        // POST Reporte
       

        // GET
        [HttpGet]
        public ActionResult ReportePersonal(string sortOrder)
        {
            var empleados = from personals in _context.Personals select personals;
            empleados = _context.Personals.Include(p => p.IdRolNavigation);

            if (empleados.Count() < 1)
            {
                ModelState.AddModelError("", "No existen PME registrados");
            }

            // Ordenar según el valor de sortOrder
            switch (sortOrder)
            {
                case "name_asc":
                    empleados = empleados.OrderBy(p => p.NombreP);
                    break;
                case "name_des":
                    empleados = empleados.OrderByDescending(p => p.NombreP);
                    break;
                case "edad_asc":
                    empleados = empleados.OrderBy(p => p.EdadP);
                    break;
                case "edad_des":
                    empleados = empleados.OrderByDescending(p => p.EdadP);
                    break;
                default:
                    // Orden predeterminado (por nombre ascendente)
                    empleados = empleados.OrderBy(p => p.NombreP);
                    break;
            }

            return View(empleados);
            var personal = PersonalServices.GetPersonal();
            PersonalServices.AddPersonal(personal);
            return View(personal);
        }

        [HttpPost]
        public ActionResult ReportePersonal(Personal personal)
        {
            IronPdf.Installation.TempFolderPath = $@"{AppDomain.CurrentDomain.BaseDirectory}/irontemp/";
            IronPdf.Installation.LinuxAndDockerDependenciesAutoConfig = true;

            var html = this.RenderViewAsync("_TicketPdf", personal);
            var ironPdfRender = new IronPdf.ChromePdfRenderer();
            using var pdfDoc = ironPdfRender.RenderHtmlAsPdf(html.Result);
            return File(pdfDoc.Stream.ToArray(), "application/pdf");
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
        public async Task<IActionResult> Create()
        {
            if (!await VerificarPermiso(2))
            {
                return RedirectToAction("AccessDenied", "Cuenta");
            }


            ViewData["IdRol"] = new SelectList(_context.Rols, "IdRol", "NombreRol");
            return View();
        }

        // POST: Personal/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdPersonal,CedulaP,NombreP,ApellidosP,CorreoP,PuestoP,FechaNaceP,EdadP,GeneroP,ProvinciaP,CantonP,DistritoP,TelefonoP,IdRol")] Personal personal)
        {
            try
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

            }
            catch (Exception ex)
            {
                // Manejo de la excepción aquí
                ModelState.AddModelError("", "Se produjo un error al intentar registrar el personal. Error: " + ex);
                // Puedes agregar registro de errores aquí si lo necesitas (por ejemplo, con loggers)
                ViewData["IdRol"] = new SelectList(_context.Rols, "IdRol", "IdRol", personal.IdRol);
                return View(personal); // Redirige a una vista de error o a donde sea apropiado en tu aplicación.
            }
            return RedirectToAction(nameof(Index));
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
            if (id != personal.IdPersonal)
            {
                return NotFound();
            }

            try
            {
                if (ModelState.IsValid)
                {
                    var existeCelular = await _context.Personals.AsNoTracking().FirstOrDefaultAsync(r => r.TelefonoP == personal.TelefonoP && r.IdPersonal != personal.IdPersonal);

                    if (existeCelular != null)
                    {
                        ModelState.AddModelError("", "Este número de celular ya está en uso.");
                        return View(personal);
                    }

                    _context.Update(personal);
                    await _context.SaveChangesAsync();

                    TempData["SuccessMessage"] = "Personal " + personal.NombreP + " actualizado exitosamente!";
                    return RedirectToAction(nameof(Index));
                }
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
