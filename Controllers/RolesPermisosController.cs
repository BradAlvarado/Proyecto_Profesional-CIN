using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sistema_CIN.Data;
using Sistema_CIN.Models;
using System.Data;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Sistema_CIN.Controllers
{
    public class RolesPermisosController : Controller
    {
        private readonly SistemaCIN_dbContext _context;

        public RolesPermisosController(SistemaCIN_dbContext context)
        {
            _context = context; // Reemplaza "TuDbContext" con el nombre de tu DbContext
        }

        // GET: RolesPermisos
        // GET: Roles
        public async Task<IActionResult> Index(string buscarRol)
        {

            var roles = from role in _context.Rols select role;

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

        // GET: RolesPermisos/Create
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        // POST: RolesPermisos/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Rol rol, Dictionary<int, Dictionary<string, bool>> operacionesPorModulo)
        {
            var existingRole = await _context.Rols.FirstOrDefaultAsync(r => r.NombreRol == rol.NombreRol);

            if (existingRole != null)
            {
                ModelState.AddModelError("", "Este rol ya existe!");

                return View(rol);
            }
            // Insertar el nuevo rol en la tabla Rol
            _context.Rols.Add(rol);
            await _context.SaveChangesAsync();

            // Obtener el ID del rol recién insertado
            int idRol = rol.IdRol;

            // Iterar sobre los módulos y sus operaciones asociadas
            foreach (var kvp in operacionesPorModulo)
            {
                int idModulo = kvp.Key;
                var operaciones = kvp.Value;

                foreach (var operacion in operaciones)
                {
                    bool isChecked = operacion.Value;

                    if (isChecked)
                    {
                        string nombreOp = operacion.Key;

                        var idOperacion = await _context.Operaciones
                            .Where(o => o.NombreOp == nombreOp && o.IdModulo == idModulo)
                            .FirstOrDefaultAsync();

                        _context.RolOperacions.Add(new RolOperacion { IdRol = idRol, IdOp = idOperacion.IdOp });
                    }
                }
            }

            await _context.SaveChangesAsync(); // Guardar los cambios una vez fuera del bucle

            return RedirectToAction("Index");
        }

        [HttpGet]
        // GET: RolesPermisos/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // Buscar el rol por ID
            Rol rol = await _context.Rols.FindAsync(id);

            if (rol == null)
            {
                return NotFound();
            }

            // Obtener las operaciones asociadas al rol
            var operacionesPorModulo = new Dictionary<int, Dictionary<string, bool>>();

            foreach (var rolOperacion in rol.RolOperacions)
            {
                int idModulo = rolOperacion.IdOpNavigation.IdModuloNavigation.IdModulo;
                string nombreOp = rolOperacion.IdOpNavigation.IdModuloNavigation.NombreModulo;

                // Inicializar el diccionario para el módulo si es necesario
                if (!operacionesPorModulo.ContainsKey(idModulo))
                {
                    operacionesPorModulo[idModulo] = new Dictionary<string, bool>();
                }

                // Marcar la operación como seleccionada en el diccionario
                operacionesPorModulo[idModulo][nombreOp] = true;
            }

            ViewBag.OperacionesPorModulo = operacionesPorModulo;
            return View(rol);
        }

        // POST: RolesPermisos/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, Rol rol, Dictionary<int, Dictionary<string, bool>> operacionesPorModulo)
        {
            if (id != rol.IdRol)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                // Eliminar las operaciones anteriores asociadas al rol
                var rolOperaciones = await _context.RolOperacions.Where(ro => ro.IdRol == id).ToListAsync();
                _context.RolOperacions.RemoveRange(rolOperaciones);

                // Actualizar el rol en la tabla Rol
                _context.Entry(rol).State = EntityState.Modified;

                // Guardar los cambios en la base de datos
                await _context.SaveChangesAsync();

                // Iterar sobre los módulos y sus operaciones asociadas
                foreach (var kvp in operacionesPorModulo)
                {
                    int idModulo = kvp.Key;
                    var operaciones = kvp.Value;

                    foreach (var operacion in operaciones)
                    {
                        bool isChecked = operacion.Value;

                        if (isChecked)
                        {
                            string nombreOp = operacion.Key;

                            var idOperacion = await _context.Operaciones
                                .Where(o => o.NombreOp == nombreOp && o.IdModulo == idModulo)
                                .Select(o => o.IdOp)
                                .FirstOrDefaultAsync();

                            _context.RolOperacions.Add(new RolOperacion { IdRol = id, IdOp = idOperacion });
                        }
                    }
                }

                // Guardar los cambios en la base de datos
                await _context.SaveChangesAsync();

                return RedirectToAction("Index");
            }

            return View(rol);
        }


        // GET: RolesPermisos/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Rol rol = await _context.Rols.FindAsync(id);

            if (rol == null)
            {
                return NotFound();
            }

            return View(rol);
        }



        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _context.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
