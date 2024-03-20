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
    public class EncargadoController : Controller
    {
        private readonly CINContext _context;

        public EncargadoController(CINContext context)
        {
            _context = context;
        }

        // GET: Encargado
        public async Task<IActionResult> Index()
        {
            var cINContext = _context.Encargados.Include(e => e.IdPmeNavigation);
            return View(await cINContext.ToListAsync());
        }

        // GET: Encargado/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Encargados == null)
            {
                return NotFound();
            }

            var encargado = await _context.Encargados
                .Include(e => e.IdPmeNavigation)
                .FirstOrDefaultAsync(m => m.IdEncargado == id);
            if (encargado == null)
            {
                return NotFound();
            }

            return View(encargado);
        }

        // GET: Encargado/Create
        public IActionResult Create()
        {
            ViewData["IdPme"] = new SelectList(_context.Pmes, "CedulaPme", "CedulaPme");
			return View();
        }

        // POST: Encargado/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdEncargado,CedulaE,ResponsableDe,NombreE,ApellidosE,FechaNaceE,Edad,CorreoE,DireccionE,TelefonoE,LugarTrabajoE,IdPme")] Encargado encargado)
        {
            if (ModelState.IsValid)
            {
                _context.Add(encargado);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdPme"] = new SelectList(_context.Pmes, "CedulaPme", "NombrePme", encargado.ResponsableDe);
            return View(encargado); 
        }

        // GET: Encargado/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Encargados == null)
            {
                return NotFound();
            }

            var encargado = await _context.Encargados.FindAsync(id);
            if (encargado == null)
            {
                return NotFound();
            }
            ViewData["IdPme"] = new SelectList(_context.Pmes, "IdPme", "IdPme", encargado.IdPme);
            return View(encargado);
        }

        // POST: Encargado/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdEncargado,CedulaE,ResponsableDe,NombreE,ApellidosE,FechaNaceE,Edad,CorreoE,DireccionE,TelefonoE,LugarTrabajoE,IdPme")] Encargado encargado)
        {
            if (id != encargado.IdEncargado)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(encargado);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EncargadoExists(encargado.IdEncargado))
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
            ViewData["IdPme"] = new SelectList(_context.Pmes, "IdPme", "IdPme", encargado.IdPme);
            return View(encargado);
        }

        // GET: Encargado/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Encargados == null)
            {
                return NotFound();
            }

            var encargado = await _context.Encargados
                .Include(e => e.IdPmeNavigation)
                .FirstOrDefaultAsync(m => m.IdEncargado == id);
            if (encargado == null)
            {
                return NotFound();
            }

            return View(encargado);
        }

        // POST: Encargado/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Encargados == null)
            {
                return Problem("Entity set 'CINContext.Encargados'  is null.");
            }
            var encargado = await _context.Encargados.FindAsync(id);
            if (encargado != null)
            {
                _context.Encargados.Remove(encargado);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EncargadoExists(int id)
        {
          return (_context.Encargados?.Any(e => e.IdEncargado == id)).GetValueOrDefault();
        }
    }
}
