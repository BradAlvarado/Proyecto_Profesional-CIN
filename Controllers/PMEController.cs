using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Sistema_CIN.Models;

namespace Sistema_CIN.Controllers
{
    public class PMEController : Controller
    {
        private readonly CIN_pruebaContext _context;

        public PMEController(CIN_pruebaContext context)
        {
            _context = context;
        }

        // GET: PME
        public async Task<IActionResult> Index()
        {
            var cINContext = _context.Pmes.Include(p => p.IdEncargadoNavigation);
            return View(await cINContext.ToListAsync());
        }

        // GET: PME/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Pmes == null)
            {
                return NotFound();
            }

            var pme = await _context.Pmes
                .Include(p => p.IdEncargadoNavigation)
                .FirstOrDefaultAsync(m => m.IdPme == id);
            if (pme == null)
            {
                return NotFound();
            }

            return View(pme);
        }

        // GET: PME/Create
        public IActionResult Create()
        {
            ViewData["IdEncargado"] = new SelectList(_context.Encargados, "IdEncargado", "NombreE");
            return View();
        }

        // POST: PME/Create
       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Encargados, Pme")] PmeViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                _context.Add(viewModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdEncargado"] = new SelectList(_context.Encargados, "IdEncargado", "IdEncargado", viewModel.Pme.IdEncargado);
            return View(viewModel);
        }

        // GET: PME/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Pmes == null)
            {
                return NotFound();
            }

            var pme = await _context.Pmes.FindAsync(id);
            if (pme == null)
            {
                return NotFound();
            }
            ViewData["IdEncargado"] = new SelectList(_context.Encargados, "IdEncargado", "IdEncargado", pme.IdEncargado);
            return View(pme);
        }

        // POST: PME/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdPme,CedulaPme,PolizaSeguro,NombrePme,ApellidosPme,FechaNacimientoPme,EdadPme,GeneroPme,ProvinciaPme,CantonPme,DistritoPme,NacionalidadPme,SubvencionPme,FechaIngresoPme,FechaEgresoPme,CondiciónMigratoriaPme,NivelEducativoPme,EncargadoPme,IdEncargado")] Pme pme)
        {
            if (id != pme.IdPme)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(pme);
                    await _context.SaveChangesAsync();
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
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdEncargado"] = new SelectList(_context.Encargados, "IdEncargado", "IdEncargado", pme.IdEncargado);
            return View(pme);
        }

        // GET: PME/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Pmes == null)
            {
                return NotFound();
            }

            var pme = await _context.Pmes
                .Include(p => p.IdEncargadoNavigation)
                .FirstOrDefaultAsync(m => m.IdPme == id);
            if (pme == null)
            {
                return NotFound();
            }

            return View(pme);
        }

        // POST: PME/Delete/5
  
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
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
