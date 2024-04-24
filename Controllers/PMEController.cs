using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Rotativa.AspNetCore;
using Sistema_CIN.Data;
using Sistema_CIN.Models;
using Sistema_CIN.Services;
using System.Security.Claims;

namespace Sistema_CIN.Controllers
{
    [Authorize]
    public class PMEController : Controller
    {
        private readonly SistemaCIN_dbContext _context;
        private readonly FiltrosPermisos _filters;


        public PMEController(SistemaCIN_dbContext context, FiltrosPermisos filtro)
        {
            _context = context;
            _filters = filtro;
        }

        private async Task<bool> VerificarPermiso(int idOp)
        {
            string emailUser = User.FindFirst(ClaimTypes.Email)?.Value ?? "desconocido";
            int cantidadOperaciones = await _filters.VerificarPermiso(emailUser, idOp);
            return cantidadOperaciones > 0;
        }

        private void AsignarCamposVacios(Pme pme)
        {
            pme.PolizaSeguro ??= "Póliza no registrada";
            pme.CantonPme ??= "No registrado";
            pme.DistritoPme ??= "No registrado";
            pme.SubvencionPme ??= false;
            pme.FechaEgresoPme ??= null;
            pme.CondiciónMigratoriaPme ??= "No registrado";
            pme.NivelEducativoPme ??= "No registrado";
        }

        public async Task<IActionResult> Index(string buscarPME, int? page, string sortOrder)
        {
            if (!await VerificarPermiso(6))
            {
                return RedirectToAction("AccessDenied", "Cuenta");
            }

            var pageNumber = page ?? 1; // Número de página actual
            var pageSize = 10; // Número de elementos por página


            var pmes = _context.Pmes.AsQueryable();
            pmes = _context.Pmes.Include(p => p.IdEncargadoNavigation);


            if (pmes.Count() < 1)
            {
                ModelState.AddModelError("", "No existen PME registrados");
            }

            if (!String.IsNullOrEmpty(buscarPME))
            {
                pmes = pmes.Where(s => s.NombrePme!.Contains(buscarPME));
            }

            switch (sortOrder)
            {
                case "name_asc":
                    pmes = pmes.OrderBy(p => p.NombrePme);
                    break;
                case "name_des":
                    pmes = pmes.OrderByDescending(p => p.NombrePme);
                    break;
                case "edad_asc":
                    pmes = pmes.OrderBy(p => p.EdadPme);
                    break;
                case "edad_des":
                    pmes = pmes.OrderByDescending(p => p.EdadPme);
                    break;
                default:
                    // Ordenamiento predeterminado
                    pmes = pmes.OrderBy(p => p.NombrePme);
                    break;
            }

            // Paginar los resultados
            var pagedPmes = await pmes.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();

            // Calcular el número total de páginas
            var totalItems = await pmes.CountAsync();
            var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

            // Crear un objeto de modelo para la paginación
            var pagedModel = new PagedList<Pme>(pagedPmes, pageNumber, pageSize, totalItems, totalPages);
            ViewData["SortOrder"] = sortOrder;

            return View(pagedModel);


        }

        // GET: PME/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (!await VerificarPermiso(6))
            {
                return RedirectToAction("AccessDenied", "Cuenta");
            }

            if (id == null)
            {
                return NotFound();
            }

            var pme = await _context.Pmes
                .Include(p => p.Encargados) // Incluir todos los encargados asociados al PME
                .FirstOrDefaultAsync(p => p.IdPme == id);

            if (pme == null)
            {
                return NotFound();
            }

            // Obtener los encargados registrados mediante ViewBag.IdEncargado
            var encargadosRegistrados = await _context.Encargados
                .Where(e => e.IdEncargado == pme.IdEncargado) // Filtrar por el IdEncargado del PME
                .ToListAsync();

            ViewData["EncargadosRegistrados"] = encargadosRegistrados;

            return View(pme);
        }

        // GET: PME/Create
        public async Task<IActionResult> Create()
        {
            if (!await VerificarPermiso(7))
            {
                return RedirectToAction("AccessDenied", "Cuenta");
            }

            ViewData["IdEncargado"] = new SelectList(_context.Encargados, "IdEncargado", "NombreE");
            return View();

        }
        // POST: PME/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdPme,CedulaPme,PolizaSeguro,NombrePme,ApellidosPme,FechaNacimientoPme,EdadPme,GeneroPme,ProvinciaPme,CantonPme,DistritoPme,NacionalidadPme,SubvencionPme,FechaIngresoPme,FechaEgresoPme,CondiciónMigratoriaPme,NivelEducativoPme,EncargadoPme,IdEncargado")] Pme pme)
        {
            try
            {
                if (!await VerificarPermiso(6))
                {
                    return RedirectToAction("AccessDenied", "Cuenta");
                }
                //Verificar si la póliza existe
                var existePoliza = await _context.Pmes.FirstOrDefaultAsync(r => r.PolizaSeguro == pme.PolizaSeguro);
                var user = User.Identity.Name;

                if (existePoliza != null)
                {
                    ModelState.AddModelError("", "Este número de póliza ya está en uso.");
                    return View(pme);
                }

                // Verificar si la cédula existe
                var existeCedula = await _context.Pmes.FirstOrDefaultAsync(r => r.CedulaPme == pme.CedulaPme);

                if (existeCedula != null)
                {
                    ModelState.AddModelError("", "La cédula ingresada ya existe");
                    return View(pme);
                }

                // Función para asignar valores en los campos vacíos
                AsignarCamposVacios(pme);

                // Agregar el nuevo PME 
                _context.Add(pme);
                await _context.SaveChangesAsync();

                await CrearBitacora("registró", pme.NombrePme);

                // Mostrar un mensaje de éxito al usuario
                TempData["SuccessMessage"] = "PME registrado exitosamente!";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                // Manejar cualquier excepción que ocurra durante la inserción del 
                // Si hay un error de validación o una excepción, devolver el formulario de creación con los datos proporcionados
                ModelState.AddModelError("", "Error: " + ex.Message);
                ViewData["IdEncargado"] = new SelectList(_context.Encargados, "IdEncargado", "IdEncargado", pme.IdEncargado);
                return View(pme);
            }
        }

        public async Task CrearBitacora(string movimiento, string nombrePme)
        {
            var user = User.Identity.Name;
            var bitacoraMovimiento = new BitacoraMovimiento
            {
                UsuarioB = user,
                FechaMovimiento = DateTime.Now,
                TipoMovimiento = movimiento,
                Detalle = user + " " + movimiento + " a " + nombrePme
            };

            _context.BitacoraMovimientos.Add(bitacoraMovimiento);
            await _context.SaveChangesAsync();
        }



        // GET: PME/Edit/5

        public async Task<IActionResult> Edit(int? id)
        {
            if (!await VerificarPermiso(8))
            {
                return RedirectToAction("AccessDenied", "Cuenta");
            }

            if (id == null || _context.Pmes == null)
            {
                return NotFound();
            }

            var pme = await _context.Pmes.FindAsync(id);
            if (pme == null)
            {
                return NotFound();
            }
            ViewData["IdEncargado"] = new SelectList(_context.Encargados, "IdEncargado", "NombreE", pme.IdEncargado);
            return View(pme);
        }

        // POST: PME/Edit/
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdPme,CedulaPme,PolizaSeguro,NombrePme,ApellidosPme,FechaNacimientoPme,EdadPme,GeneroPme,ProvinciaPme,CantonPme,DistritoPme,NacionalidadPme,SubvencionPme,FechaIngresoPme,FechaEgresoPme,CondiciónMigratoriaPme,NivelEducativoPme,EncargadoPme,IdEncargado")] Pme pme)
        {

            if (id != pme.IdPme)
            {
                return NotFound();
            }

            try
            {
                if (ModelState.IsValid)
                {
                    var existeCedula = await _context.Pmes.AsNoTracking().FirstOrDefaultAsync(r => r.CedulaPme == pme.CedulaPme && r.IdPme != pme.IdPme);

                    if (existeCedula != null)
                    {
                        ModelState.AddModelError("", "Este número de cédula ya está en uso.");
                        return View(pme);
                    }

                    _context.Update(pme);
                    await _context.SaveChangesAsync();
                    await CrearBitacora("actualizó", pme.NombrePme);

                    TempData["SuccessMessage"] = "Menor " + pme.NombrePme + " actualizado exitosamente!";
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PmeExists(pme.IdPme))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            ViewData["IdEncargado"] = new SelectList(_context.Encargados, "IdEncargado", "IdEncargado", pme.IdEncargado);
            return View(pme);
        }

        // GET ReportePMEs
        public async Task<ActionResult> ReportePMEs(string sortOrder)
        {
            if (!await VerificarPermiso(9))
            {
                return RedirectToAction("AccessDenied", "Cuenta");
            }
            var pmes = _context.Pmes.AsQueryable();

            if (!string.IsNullOrEmpty(sortOrder))
            {
                if (sortOrder == "name_asc")
                {
                    pmes = pmes.OrderBy(p => p.NombrePme);
                }
                if (sortOrder == "name_des")
                {
                    pmes = pmes.OrderByDescending(p => p.NombrePme);
                }
                if (sortOrder == "edad_asc")
                {
                    pmes = pmes.OrderBy(p => p.EdadPme);
                }
                if (sortOrder == "edad_des")
                {
                    pmes = pmes.OrderByDescending(p => p.EdadPme);
                }
            }
            // Capturar la fecha y hora actual
            DateTime fechaActual = DateTime.Now;

            return new ViewAsPdf("ReportePMEs", pmes.ToList())
            {
                FileName = $"Reporte_Pmes_{fechaActual}.pdf",
                PageOrientation = Rotativa.AspNetCore.Options.Orientation.Portrait,
                PageSize = Rotativa.AspNetCore.Options.Size.A4
            };
            //return View(pme);
        }

        // POST: PME/Delete/5
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            if (!await VerificarPermiso(10))
            {
                return RedirectToAction("AccessDenied", "Cuenta");
            }
            if (_context.Pmes == null)
            {
                return Problem("Entity set 'CINContext.Pmes'  is null.");
            }
            var pme = await _context.Pmes.FindAsync(id);
            if (pme == null)
            {
                return NotFound();
            }

            _context.Pmes.Remove(pme);
            await _context.SaveChangesAsync();
            await CrearBitacora("eliminó", pme.NombrePme);
            TempData["SuccessMessage"] = "Menor " + pme.NombrePme + " eliminado exitosamente!";

            // Json para enviar el success del Delete del registro
            return Json(new { success = true });
        }


        private bool PmeExists(int id)
        {
            return (_context.Pmes?.Any(e => e.IdPme == id)).GetValueOrDefault();
        }
    }
}
