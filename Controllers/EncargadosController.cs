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
    public class EncargadosController : Controller
    {
        private readonly CINContext _context;

        public EncargadosController(CINContext context)
        {
            _context = context;
        }

        // GET: Encargados
        public async Task<IActionResult> Index()
        {
            var cINContext = _context.Encargados.Include(e => e.IdPmeNavigation);
            return View(await cINContext.ToListAsync());
        }

        // GET: Encargados/Details/5
        public async Task<IActionResult> Details(int? id)
        {
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
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdEncargado,CedulaE,NombreE,ApellidosE,FechaNaceE,Edad,CorreoE,DireccionE,TelefonoE,LugarTrabajoE,IdPme")] Encargados encargados)
        {
            if (ModelState.IsValid)
            {
                _context.Add(encargados);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdPme"] = new SelectList(_context.Pmes, "IdPme", "NombrePme", encargados.IdPme);
            return View(encargados);
        }

        // GET: Encargados/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
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
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
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

        // GET: Encargados/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
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

        // POST: Encargados/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Encargados == null)
            {
                return Problem("Entity set 'CINContext.Encargados'  is null.");
            }
            var encargados = await _context.Encargados.FindAsync(id);
            if (encargados != null)
            {
                _context.Encargados.Remove(encargados);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EncargadosExists(int id)
        {
          return (_context.Encargados?.Any(e => e.IdEncargado == id)).GetValueOrDefault();
        }
    }
}
