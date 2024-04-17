using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Sistema_CIN.Data;
using Sistema_CIN.Models;

namespace Sistema_CIN.Controllers
{
    [Authorize(Policy = "RequireAdmin")]
    public class PermisosController : Controller
    {
        private readonly CIN_pruebaContext _context;


        public PermisosController(CIN_pruebaContext context)
        {
            _context = context;
        }

        // GET: PermisosController
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

        // GET: PermisosController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

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

        // GET: PermisosController/Edit/5
        public async Task<ActionResult> Edit(int id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var permisoRol = await _context.Permisos
                .Include(u => u.IdRolNavigation)
                .FirstOrDefaultAsync(m => m.IdRol == id);

            if (permisoRol == null)
            {
                return NotFound();
            }

            ViewData["IdRol"] = new SelectList(_context.Roles, "IdRol", "NombreRol", permisoRol.IdRol);
            return View(permisoRol);
        }

        // POST: PermisosController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: PermisosController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: PermisosController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
