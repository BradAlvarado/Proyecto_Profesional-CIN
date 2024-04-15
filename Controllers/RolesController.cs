using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Sistema_CIN.Data;
using Sistema_CIN.Models;

namespace Sistema_CIN.Controllers
{
    [Authorize]
    public class RolesController : Controller
    {
        private readonly CIN_pruebaContext _context;


        public RolesController(CIN_pruebaContext context)
        {
            _context = context;
        }

        // GET: Roles
        public async Task<IActionResult> Index(string buscarRol)
        {

            var roles = from role in _context.Roles select role;

            if (!String.IsNullOrEmpty(buscarRol))
            {
                roles = roles.Where(s => s.NombreRol!.Contains(buscarRol));
            }
            else
            {
                ModelState.AddModelError("", "No existen Roles");
                return View(await roles.ToListAsync());
            }
            return View(await roles.ToListAsync());


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
        public async Task<IActionResult> Create([Bind("IdRol,NombreRol")] Roles role)
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
        public async Task<IActionResult> Edit(int id, [Bind("IdRol,NombreRol")] Roles role)
        {
            if (id != role.IdRol)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Verificar si el nombre de rol ya existe en la base de datos
                    var existingRole = await _context.Roles.AsNoTracking().FirstOrDefaultAsync(r => r.NombreRol == role.NombreRol);

                    if (existingRole != null)
                    {
                        // Si el nombre de rol ya existe y no es el mismo que el original, mostrar un error
                        if (existingRole.IdRol != role.IdRol)
                        {
                            ModelState.AddModelError("", "El nombre de rol ya está en uso.");
                            return View(role);
                        }
                    }

                    // Actualizar el rol en la base de datos
                    _context.Update(role);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Rol modificado exitosamente!";
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

            // Si el modelo no es válido, vuelve a la vista de edición con los errores
            return View(role);
        }

     
        private bool RoleExists(int id)
        {
            return (_context.Roles?.Any(e => e.IdRol == id)).GetValueOrDefault();
        }
    }
}
