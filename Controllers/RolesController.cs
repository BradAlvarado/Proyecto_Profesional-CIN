using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Sistema_CIN.Models;

namespace Sistema_CIN.Controllers
{
    public class RolesController : Controller
    {
        private readonly CINContext _context;


        public RolesController(CINContext context)
        {
            _context = context;
        }

        // GET: Roles
        public async Task<IActionResult> Index()
        {
            return _context.Roles != null ?

                        View(await _context.Roles.ToListAsync()) :
                        Problem("Entity set 'CINContext.Roles'  is null.");

        }

        // GET: Roles/Details/5 NO 


        // GET: Roles/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Roles/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdRol,NombreRol")] Role role)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var existingRole = await _context.Roles.FirstOrDefaultAsync(r => r.NombreRol == role.NombreRol);

                    if (existingRole != null)
                    {
                        ModelState.AddModelError("", "Este rol ya existe!");

                        return View(role);
                    }

                    _context.Add(role);
                    await _context.SaveChangesAsync();

                    TempData["SuccessMessage"] = "El rol se agregó con éxito.";
                    return RedirectToAction(nameof(Index));
                }
                catch
                (Exception ex)
                {
                    ModelState.AddModelError("", "Ocurrió un error al guardar el rol: " + ex.Message);
                }
            }
            return View(role);
        }

        // GET: Roles/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Roles == null)
            {
                return NotFound();
            }

            var role = await _context.Roles.FindAsync(id);
            if (role == null)
            {
                return NotFound();
            }
            return View(role);
        }

        // POST: Roles/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdRol,NombreRol")] Role role)
        {
            if (id != role.IdRol)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var existingRole = await _context.Roles.FirstOrDefaultAsync(r => r.NombreRol == role.NombreRol);
                    if (existingRole != null)
                    {
                        ModelState.AddModelError("", "Este rol ya existe!");

                        return View(role);
                    }
                    _context.Update(role);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RoleExists(role.IdRol))
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
            return View(role);
        }

        // GET: Roles/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Roles == null)
            {
                return NotFound();
            }

            var role = await _context.Roles
                .FirstOrDefaultAsync(m => m.IdRol == id);
            if (role == null)
            {
                return NotFound();
            }

            return View(role);
        }

        // POST: Roles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Roles == null)
            {
                return Problem("Entity set 'CINContext.Roles'  is null.");
            }
            var role = await _context.Roles.FindAsync(id);
            if (role != null)
            {
                _context.Roles.Remove(role);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RoleExists(int id)
        {
            return (_context.Roles?.Any(e => e.IdRol == id)).GetValueOrDefault();
        }
    }
}
