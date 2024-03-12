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
        private readonly CIN_dbContext _context;

        public EncargadoController(CIN_dbContext context)
        {
            _context = context;
        }

        // GET: Encargadoes
        public async Task<IActionResult> Index()
        {
              return _context.Encargados != null ? 
                          View(await _context.Encargados.ToListAsync()) :
                          Problem("Entity set 'CIN_dbContext.Encargados'  is null.");
        }

        // GET: Encargadoes/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null || _context.Encargados == null)
            {
                return NotFound();
            }

            var encargado = await _context.Encargados
                .FirstOrDefaultAsync(m => m.cedula_e == id);
            if (encargado == null)
            {
                return NotFound();
            }

            return View(encargado);
        }

        // GET: Encargadoes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Encargadoes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("cedula_e,responsable_de,nombre_e,apellidos_e,edad_e,direccion_e,telefono_e,correo_e,lugar_trabajo_e")] Encargado encargado)
        {
            if (ModelState.IsValid)
            {
                _context.Add(encargado);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(encargado);
        }

        // GET: Encargadoes/Edit/5
        public async Task<IActionResult> Edit(string id)
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
            return View(encargado);
        }

        // POST: Encargadoes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("cedula_e,responsable_de,nombre_e,apellidos_e,edad_e,direccion_e,telefono_e,correo_e,lugar_trabajo_e")] Encargado encargado)
        {
            if (id != encargado.cedula_e)
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
                    if (!EncargadoExists(encargado.cedula_e))
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
            return View(encargado);
        }

        // GET: Encargadoes/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null || _context.Encargados == null)
            {
                return NotFound();
            }

            var encargado = await _context.Encargados
                .FirstOrDefaultAsync(m => m.cedula_e == id);
            if (encargado == null)
            {
                return NotFound();
            }

            return View(encargado);
        }

        // POST: Encargadoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (_context.Encargados == null)
            {
                return Problem("Entity set 'CIN_dbContext.Encargados'  is null.");
            }
            var encargado = await _context.Encargados.FindAsync(id);
            if (encargado != null)
            {
                _context.Encargados.Remove(encargado);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EncargadoExists(string id)
        {
          return (_context.Encargados?.Any(e => e.cedula_e == id)).GetValueOrDefault();
        }
    }
}
