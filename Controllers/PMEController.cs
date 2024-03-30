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
    public class PmeController : Controller
    {
        private readonly CINContext _context;

        public PmeController(CINContext context)
        {
            _context = context;
        }

        // GET: Pme
        public async Task<IActionResult> Index(string buscarPme)
        {
            var cINContext = _context.Pmes.Include(u => u.IdEncargadoNavigation);
            return View(await cINContext.ToListAsync());

        }

        // GET: Pme/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Pmes == null)
            {
                return NotFound();
            }

            
            var pme = await _context.Pmes
                .Include(e => e.IdEncargadoNavigation)
                .FirstOrDefaultAsync(m => m.IdPme == id);

            if (pme == null)
            {
                return NotFound();
            }

            return View(pme);
        }

        // GET: Pme/Contacto Familiar/
        public async Task<IActionResult> ContactoFamiliar(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pme = await _context.Pmes
                .Include(p => p.Encargados)
                .FirstOrDefaultAsync(m => m.IdPme == id);

            if (pme == null)
            {
                return NotFound();
            }

            var viewModel = new PmeViewModel
            {
                Pme = pme
            };

            return View(viewModel);
        }

        // GET: Pme/Create
        public IActionResult Create()
        {
            ViewData["IdEncargado"] = new SelectList(_context.Pmes, "IdEncargado", "NombreE");
            return View();
        }

        // POST: Pme/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Pme, Encargado")] PmeViewModel viewModel)
        {

            if (ModelState.IsValid)
            {
                // Verificar si se proporcionaron datos del Encargado
                if (viewModel.Encargado != null && !string.IsNullOrEmpty(viewModel.Encargado.NombreE))
                {
                    // Registrar el Encargado solo si se proporcionaron los datos
                    _context.Encargados.Add(viewModel.Encargado);
                    await _context.SaveChangesAsync();

                    // Asignar el ID del Encargado al Pme
                    viewModel.Pme.IdEncargado = viewModel.Encargado.IdEncargado;
                }

                // Guardar el Pme
                _context.Pmes.Add(viewModel.Pme);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            return View(viewModel);


        }

        // GET: Pme/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pme = await _context.Pmes.FindAsync(id);
            if (pme == null)
            {
                return NotFound();
            }

            var encargado = await _context.Encargados.FindAsync(pme.IdEncargado);
            var viewModel = new PmeViewModel
            {
                Pme = pme,
                Encargado = encargado
            };
            ViewData["IdEncargado"] = new SelectList(_context.Encargados, "IdEncargado", "NombreE", pme.IdEncargado);
            return View(viewModel);
        }

        // POST: Pme/Edit/5
       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Pme, Encargado")] PmeViewModel viewModel)
        {
            if (id != viewModel.Pme.IdPme)
            {
                return NotFound();
            }

            try
            {
                _context.Update(viewModel.Pme);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PmeExists(viewModel.Pme.IdPme))
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

        // GET: Pme/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Pmes == null)
            {
                return NotFound();
            }

            var pme = await _context.Pmes
                .FirstOrDefaultAsync(m => m.IdPme == id);
            if (pme == null)
            {
                return NotFound();
            }

            return View(pme);
        }

        // POST: Pme/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
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

            // Desvincular al alumno de los registros relacionados en la tabla de padres
            var padresConPme = await _context.Encargados.Where(p => p.IdPme == id).ToListAsync();
            foreach (var padre in padresConPme)
            {
                padre.IdPme = null;
            }
            await _context.SaveChangesAsync();

            // Eliminar al alumno
            _context.Pmes.Remove(pme);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        private bool PmeExists(int id)
        {
            return (_context.Pmes?.Any(e => e.IdPme == id)).GetValueOrDefault();
        }
    }
}
