using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Sistema_CIN.Data;
using Sistema_CIN.Models;
using System.Data;


namespace Sistema_CIN.Controllers
{
    [Authorize(Policy = "RequireAdmin")]

    public class RolesPermisosController : Controller
    {
        private readonly SistemaCIN_dbContext _context;

        public RolesPermisosController(SistemaCIN_dbContext context)
        {
            _context = context; // Reemplaza "TuDbContext" con el nombre de tu DbContext
        }

        // GET: RolesPermisos

        public async Task<IActionResult> Index(string buscarRol)
        {

            var roles = from role in _context.Rols select role;

            if (!String.IsNullOrEmpty(buscarRol))
            {
                roles = roles.Where(s => s.NombreRol!.Contains(buscarRol));
            }
            else
            {
                ModelState.AddModelError("", "No existen Roles registrados");
                return View(await roles.ToListAsync());
            }
            return View(await roles.ToListAsync());

        }

        public ActionResult Create()
        {
            return View();
        }


        //POST: RolesPermisos/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Rol rol, Dictionary<int, Dictionary<string, bool>> operacionesPorModulo)
        {
            var existingRole = await _context.Rols.FirstOrDefaultAsync(r => r.NombreRol == rol.NombreRol);

            if (existingRole != null)
            {
                ModelState.AddModelError("", "Este rol ya existe!");

                var rolesPermisos = new RolesPermisos
                {
                    Rol = rol,
                    // Otras asignaciones necesarias para el modelo RolesPermisos
                };

                return View(rolesPermisos);
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
            TempData["SuccessMessage"] = "Rol registrado exitosamente!";
            return RedirectToAction("Index");
            // Insertar el nuevo rol en la tabla             
        }


        // GET RolesPermisos Details
        public async Task<IActionResult> Details(int id)
        {
            var rol = await _context.Rols
                .Include(r => r.RolOperacions)
                .ThenInclude(ro => ro.IdOpNavigation) // Cargar las operaciones asociadas a través de RolOperacion
                .ThenInclude(op => op.IdModuloNavigation) // Cargar la navegación del módulo para cada operación
                .FirstOrDefaultAsync(r => r.IdRol == id);

            if (rol == null)
            {
                return NotFound();
            }

            return View(rol);
        }



        // GET: Roles/Edit/5
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var rol = await _context.Rols
                .Include(r => r.RolOperacions)
                .ThenInclude(ro => ro.IdOpNavigation) // Cargar las operaciones asociadas a través de RolOperacion
               .ThenInclude(op => op.IdModuloNavigation) // Cargar la navegación del módulo para cada operación
                .FirstOrDefaultAsync(r => r.IdRol == id);

            if (rol == null)
            {
                return NotFound();
            }

            var modulos = await _context.Modulos.ToListAsync(); // Obtener todos los módulos
            var operaciones = await _context.Operaciones.ToListAsync();

            var rolesPermisos = new RolesPermisos
            {
                Rol = rol,
                Modulos = modulos,
                Operaciones = operaciones,
                RolOperaciones = rol.RolOperacions.ToList(), // Asegúrate de cargar las relaciones de RolOperaciones
                NombreRol = rol.NombreRol // Asignar el nombre del rol
            };

            return View(rolesPermisos);
        }

        // POST/ EDIT
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, Rol rol, Dictionary<int, Dictionary<string, bool>> operacionesPorModulo)
        {
            try
            {
                // Buscar el rol existente en la base de datos
                var existingRol = await _context.Rols
                    .Include(r => r.RolOperacions)
                    .FirstOrDefaultAsync(r => r.IdRol == id);

                if (existingRol == null)
                {
                    return NotFound();
                }

                // Actualizar las propiedades del rol existente con los datos del formulario
                existingRol.NombreRol = rol.NombreRol;

                // Limpiar las operaciones existentes asociadas al rol
                _context.RolOperacions.RemoveRange(existingRol.RolOperacions);

                // Iterar sobre los módulos y sus operaciones asociadas en el formulario
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

                            // Buscar la operación correspondiente en la base de datos
                            var idOperacion = await _context.Operaciones
                                .Where(o => o.NombreOp == nombreOp && o.IdModulo == idModulo)
                                .Select(o => o.IdOp)
                                .FirstOrDefaultAsync();

                            // Agregar la relación de rol-operación al rol existente
                            existingRol.RolOperacions.Add(new RolOperacion { IdOp = idOperacion });
                        }
                    }
                }

                // Guardar los cambios en la base de datos
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Rol " + rol.NombreRol +" actualizado exitosamente!";
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                ModelState.AddModelError("", "Este rol ya existe!");
                return View();
            }
        }

        // POST: RolesPermisos/Delete/5
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                // Buscar el rol por su ID
                var rol = await _context.Rols.FindAsync(id);

                // Si el rol no se encuentra, devolver un JSON indicando que no se encontró el rol
                if (rol == null)
                {
                    return Json(new { success = false });
                }

                // Verificar si hay usuarios asociados a este rol
                var usuariosConRol = await _context.Usuarios.Where(u => u.IdRol == id).ToListAsync();

                var personalConRol = await _context.Personals.Where(u => u.IdRol == id).ToListAsync();

                int idRolInvitado = await _context.Rols
                .Where(r => r.NombreRol == "Invitado")
                    .Select(r => r.IdRol)
                    .FirstOrDefaultAsync();

                // Si hay usuarios asociados al rol, cambia su rol a "Invitado"
                if (usuariosConRol.Any())
                {
                    foreach (var usuario in usuariosConRol)
                    {
                        usuario.IdRol = idRolInvitado; // Cambiar el rol del usuario a "Invitado"
                    }
                }

                if (personalConRol.Any())
                {
                    foreach (var empleado in personalConRol)
                    {
                        empleado.IdRol = idRolInvitado; // Cambiar el rol del usuario a "Invitado"
                    }
                }

                // Eliminar todas las relaciones de RolOperaciones asociadas a este rol

                var rolOperaciones = await _context.RolOperacions.Where(ro => ro.IdRol == id).ToListAsync();
                _context.RolOperacions.RemoveRange(rolOperaciones);

                // Eliminar el rol
                _context.Rols.Remove(rol);

                // Guardar los cambios en la base de datos
                await _context.SaveChangesAsync();

                // Establecer un mensaje de éxito en TempData
                TempData["SuccessMessage"] = "Rol " + rol.NombreRol + " eliminado exitosamente!";

                // Devolver un JSON indicando el éxito de la operación de eliminación
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                // Manejar cualquier excepción que pueda ocurrir durante el proceso de eliminación
                return Json(new { success = false, message = "Ocurrió un error al eliminar el rol: " + ex.Message });
            }
        }




        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _context.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool RolExists(int id)
        {
            return (_context.Rols?.Any(e => e.IdRol == id)).GetValueOrDefault();
        }
    }
}
