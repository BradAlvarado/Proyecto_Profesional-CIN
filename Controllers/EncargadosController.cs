using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
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
        public async Task<IActionResult> Index(string buscarEncargado, int? page)
        {
            if (!await VerificarPermiso(11))
            {
                return RedirectToAction("AccessDenied", "Cuenta");
            }
            var pageNumber = page ?? 1; // Número de página actual
            var pageSize = 10; // Número de elementos por página

            var encargados = from encargado in _context.Encargados select encargado;
            encargados = _context.Encargados.Include(p => p.IdPmeNavigation);

            if (encargados.Count() < 1)
            {
                ModelState.AddModelError("", "No existen Encargados registrados");
            }

            if (!String.IsNullOrEmpty(buscarEncargado))
            {
                encargados = encargados.Where(s => s.NombreE!.Contains(buscarEncargado));
            }

            // Paginar los resultados
            var pagedEnc = await encargados.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();

            // Calcular el número total de páginas
            var totalItems = await encargados.CountAsync();
            var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

            // Crear un objeto de modelo para la paginación
            var pagedModel = new PagedList<Encargados>(pagedEnc, pageNumber, pageSize, totalItems, totalPages);

            return View(pagedModel);
        }

        // GET: Encargados/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (!await VerificarPermiso(11))
            {
                return RedirectToAction("AccessDenied", "Cuenta");
            }
            if (id == null || _context.Encargados == null)
            {
                return NotFound();
            }

            var encargados = await _context.Encargados
                .Include(e => e.IdPmeNavigation)
                .FirstOrDefaultAsync(m => m.IdEncargado == id);
            if (encargados == null)
            {
                return NotFound();
            }

            return View(encargados);
        }

        // GET: Encargados/Create
        public IActionResult Create()
        {
            
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
                if (!await VerificarPermiso(12))
                {
                    return RedirectToAction("AccessDenied", "Cuenta");
                }
                // Verificar si la cédula ya existe en el contexto
                var existeCedula = await _context.Encargados.FirstOrDefaultAsync(e => e.CedulaE == encargados.CedulaE);
                if (existeCedula != null)
                {
                    ModelState.AddModelError("", "Esta cédula ya está registrada.");
                    ViewData["IdPme"] = new SelectList(_context.Pmes, "IdPme", "NombrePme", encargados.IdPme);
                    return View(encargados);
                }

                if (ModelState.IsValid)
                {
                    AsignarCamposVacios(encargados);
                    _context.Add(encargados);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Encargado registrado exitosamente!";
                    return RedirectToAction(nameof(Index));
                }
                ViewData["IdPme"] = new SelectList(_context.Pmes, "IdPme", "NombrePme", encargados.IdPme);
                return View(encargados);
            }
            catch (Exception)
            {
                // Manejo de errores aquí, como registrar el error, mostrar un mensaje al usuario, etc.
                ModelState.AddModelError("", "Ocurrió un error al procesar la solicitud.");
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
            if (!await VerificarPermiso(13))
            {
                return RedirectToAction("AccessDenied", "Cuenta");
            }
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
