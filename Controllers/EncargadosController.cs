using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Rotativa.AspNetCore;
using Sistema_CIN.Data;
using Sistema_CIN.Models;
using Sistema_CIN.Services;


namespace Sistema_CIN.Controllers
{
    [Authorize]
    public class EncargadosController : Controller
    {
        private readonly SistemaCIN_dbContext _context;
        private readonly FiltrosPermisos _filters;

        public EncargadosController(SistemaCIN_dbContext context, FiltrosPermisos filtro)
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

        private void AsignarCamposVacios(Encargados encargado)
        {
            encargado.LugarTrabajoE ??= "No ingresado";

        }

        // GET: Encargados
        public async Task<IActionResult> Index(string buscarEncargado, int? page, string sortOrder)
        {
            if (!await VerificarPermiso(11))
            {
                return RedirectToAction("AccessDenied", "Cuenta");
            }
            var pageNumber = page ?? 1; // Número de página actual
            var pageSize = 10; // Número de elementos por página

            var encargado = from Encargados in _context.Encargados select Encargados;
            encargado = _context.Encargados.Include(p => p.IdPmeNavigation);


            if (encargado.Count() < 1)
            {
                ModelState.AddModelError("", "No existen Encargados registrados");
            }

            if (!String.IsNullOrEmpty(buscarEncargado))
            {
                encargado = encargado.Where(s => s.NombreE!.Contains(buscarEncargado));
            }

            switch (sortOrder)
            {
                case "name_asc":
                    encargado = encargado.OrderBy(p => p.NombreE);
                    break;
                case "name_des":
                    encargado = encargado.OrderByDescending(p => p.NombreE);
                    break;
                case "edad_asc":
                    encargado = encargado.OrderBy(p => p.Edad);
                    break;
                case "edad_des":
                    encargado = encargado.OrderByDescending(p => p.Edad);
                    break;
            }

            // Paginar los resultados
            var pagedEncargado = await encargado.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();

            // Calcular el número total de páginas
            var totalItems = await encargado.CountAsync();
            var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

            // Crear un objeto de modelo para la paginación
            var pagedModel = new PagedList<Encargados>(pagedEncargado, pageNumber, pageSize, totalItems, totalPages);
            ViewData["SortOrder"] = sortOrder;
            return View(pagedModel);
        }

        // GET: Encargados/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (!await VerificarPermiso(11))
            {
                return RedirectToAction("AccessDenied", "Cuenta");
            }


            if (id == null)
            {
                return NotFound();
            }

            var encargados = await _context.Encargados
                .Include(e => e.Pmes)
                .FirstOrDefaultAsync(m => m.IdEncargado == id);

            if (encargados == null)
            {
                return NotFound();
            }

            // Obtener los encargados registrados mediante ViewBag.IdEncargado
            var pmesRegistrados = await _context.Pmes
                .Where(e => e.IdPme == encargados.IdPme) // Filtrar por el IdEncargado del PME
                .ToListAsync();

            ViewData["PmesRegistrados"] = pmesRegistrados;

            return View(encargados);
        }

        // GET: Encargados/Create
        public async Task<IActionResult> Create()
        {
            if (!await VerificarPermiso(12))
            {
                return RedirectToAction("AccessDenied", "Cuenta");
            }
            ViewData["IdPme"] = new SelectList(_context.Pmes, "IdPme", "NombrePme");
            return View();
        }


        // POST: Encargados/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdEncargado,CedulaE,NombreE,ApellidosE,FechaNaceE,Edad,CorreoE,DireccionE,TelefonoE,LugarTrabajoE,IdPme")] Encargados encargados)
        {
            try
            {
                // Verificar si la cédula ya existe en el contexto
                var existeCedula = await _context.Encargados.FirstOrDefaultAsync(e => e.CedulaE == encargados.CedulaE);
                if (existeCedula != null)
                {
                    ModelState.AddModelError("", "Esta cédula ya está registrada.");
                    ViewData["IdPme"] = new SelectList(_context.Pmes, "IdPme", "NombrePme", encargados.IdPme);
                    return View(encargados);
                }

                AsignarCamposVacios(encargados);
                _context.Add(encargados);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Encargado registrado exitosamente!";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                // Manejo de errores aquí, como registrar el error, mostrar un mensaje al usuario, etc.
                ModelState.AddModelError("", "Ocurrió un error al procesar la solicitud: " + ex.Message);
                ViewData["IdPme"] = new SelectList(_context.Pmes, "IdPme", "NombrePme", encargados.IdPme);
                return View(encargados);
            }
        }



        // GET: Encargados/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (!await VerificarPermiso(13))
            {
                return RedirectToAction("AccessDenied", "Cuenta");
            }
            if (id == null || _context.Encargados == null)
            {
                return NotFound();
            }

            var encargados = await _context.Encargados.FindAsync(id);
            if (encargados == null)
            {
                return NotFound();
            }
            ViewData["IdPme"] = new SelectList(_context.Pmes, "IdPme", "NombrePme", encargados.IdPme);
            return View(encargados);
        }

        // POST: Encargados/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdEncargado,CedulaE,NombreE,ApellidosE,FechaNaceE,Edad,CorreoE,DireccionE,TelefonoE,LugarTrabajoE,IdPme")] Encargados encargados)
        {
            if (id != encargados.IdEncargado)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(encargados);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Encargado " + encargados.NombreE + " actualizado exitosamente!";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EncargadosExists(encargados.IdEncargado))
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
            ViewData["IdPme"] = new SelectList(_context.Pmes, "IdPme", "NombrePme", encargados.IdPme);
            return View(encargados);
        }


        // GET ReporteEncargados/4
        public async Task<ActionResult> EncargadosPDF(string sortOrder)
        {
            if (!await VerificarPermiso(14))
            {
                return RedirectToAction("AccessDenied", "Cuenta");
            }
            var encargados = _context.Encargados.AsQueryable();

            if (!string.IsNullOrEmpty(sortOrder))
            {
                if (sortOrder == "name_asc")
                {
                    encargados = encargados.OrderBy(p => p.NombreE);
                }
                if (sortOrder == "name_des")
                {
                    encargados = encargados.OrderByDescending(p => p.NombreE);
                }
                if (sortOrder == "edad_asc")
                {
                    encargados = encargados.OrderBy(p => p.Edad);
                }
                if (sortOrder == "edad_des")
                {
                    encargados = encargados.OrderByDescending(p => p.Edad);
                }
            }
            // Capturar la fecha y hora actual
            DateTime fechaActual = DateTime.Today;

            return new ViewAsPdf("EncargadosPDF", encargados.ToList())
            {
                FileName = $"Reporte_Encargados_{fechaActual}.pdf",
                PageOrientation = Rotativa.AspNetCore.Options.Orientation.Portrait,
                PageSize = Rotativa.AspNetCore.Options.Size.A4
            };
            //return View(personal);
        }

        // POST: Encargados/Delete/5
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            if (!await VerificarPermiso(15))
            {
                return RedirectToAction("AccessDenied", "Cuenta");
            }
            if (_context.Encargados == null)
            {
                return Problem("Entity set 'CINContext.Encargados'  is null.");
            }
            var encargado = await _context.Encargados.FindAsync(id);
            if (encargado == null)
            {
                return NotFound();
            }
            var pmeConEncargado = await _context.Pmes.Where(u => u.IdEncargado == id).ToListAsync();

            // Si hay pmes asociados al encargado, cambia su idEncargado a null
            if (pmeConEncargado.Any())
            {
                foreach (var pme in pmeConEncargado)
                {
                    pme.IdEncargado = null;
                    _context.Pmes.Update(pme);
                }
                
            }
            _context.Encargados.Remove(encargado);
            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = "Encargado " + encargado.NombreE + " eliminado exitosamente!";

            // Json para enviar el success del Delete del registro
            return Json(new { success = true });
        }

        private bool EncargadosExists(int id)
        {
            return (_context.Encargados?.Any(e => e.IdEncargado == id)).GetValueOrDefault();
        }
    }
}
