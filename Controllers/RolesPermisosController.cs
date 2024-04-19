using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sistema_CIN.Data;
using Sistema_CIN.Models;
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
        public ActionResult Create()
        {
            return View();
        }

        // POST: RolesPermisos/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Rol rol, bool verChecked, bool crearChecked, bool editarChecked, bool reportarChecked, bool eliminarChecked)
        {
            // Primero, insertamos el nuevo rol en la tabla Rol
            _context.Rols.Add(rol);
            await _context.SaveChangesAsync();

            // Luego, obtenemos el ID del rol recién insertado
            int idRol = rol.IdRol;

           
            if (verChecked)
            {
                Operaciones verOperacion = new Operaciones { NombreOp = "Ver" ,IdModulo = 0};
                _context.Operaciones.Add(verOperacion);
                await _context.SaveChangesAsync();

                _context.RolOperacions.Add(new RolOperacion { IdRol = idRol, IdOp = verOperacion.IdOp });
                await _context.SaveChangesAsync();
            }

            if (crearChecked)
            {
                Operaciones crearOperacion = new Operaciones { NombreOp = "Crear" };
                _context.Operaciones.Add(crearOperacion);
                await _context.SaveChangesAsync();

                _context.RolOperacions.Add(new RolOperacion { IdRol = idRol, IdOp = crearOperacion.IdOp });
                await _context.SaveChangesAsync();
            }

            if (editarChecked)
            {
                Operaciones editarOperacion = new Operaciones { NombreOp = "Editar" };
                _context.Operaciones.Add(editarOperacion);
                await _context.SaveChangesAsync();

                _context.RolOperacions.Add(new RolOperacion { IdRol = idRol, IdOp = editarOperacion.IdOp });
                await _context.SaveChangesAsync();
            }

            if (reportarChecked)
            {
                Operaciones reportarOperacion = new Operaciones { NombreOp = "Reportar" };
                _context.Operaciones.Add(reportarOperacion);
                await _context.SaveChangesAsync();

                _context.RolOperacions.Add(new RolOperacion { IdRol = idRol, IdOp = reportarOperacion.IdOp });
                await _context.SaveChangesAsync();
            }

            if (eliminarChecked)
            {
                Operaciones eliminarOperacion = new Operaciones { NombreOp = "Eliminar" };
                _context.Operaciones.Add(eliminarOperacion);
                await _context.SaveChangesAsync();

                _context.RolOperacions.Add(new RolOperacion { IdRol = idRol, IdOp = eliminarOperacion.IdOp });
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Index");
        }


        // GET: RolesPermisos/Edit/5


        // POST: RolesPermisos/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(Rol rol)
        {
            if (ModelState.IsValid)
            {
                _context.Entry(rol).State = EntityState.Modified;
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
