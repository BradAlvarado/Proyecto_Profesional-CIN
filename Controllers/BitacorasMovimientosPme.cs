using Microsoft.AspNetCore.Mvc;
using Sistema_CIN.Data;
using Sistema_CIN.Services;
using System.Security.Claims;

namespace Sistema_CIN.Controllers
{
    public class BitacorasMovimientosPme : Controller
    {
        private readonly SistemaCIN_dbContext _context;
        private readonly FiltrosPermisos _filters;

        public BitacorasMovimientosPme(SistemaCIN_dbContext context, FiltrosPermisos filtro)
        {
            _context = context;
            _filters = filtro;
        }

        // Funcion para verificar si el usuario en sesion tiene permiso para acceder a los modulos
        private async Task<bool> VerificarPermiso(int idOp)
        {
            string emailUser = User.FindFirst(ClaimTypes.Email)?.Value ?? "desconocido";
            int cantidadOperaciones = await _filters.VerificarPermiso(emailUser, idOp);
            return cantidadOperaciones > 0;
        }
        // GET: BitacorasMovimientosPme

        public async Task<IActionResult> Index(int? page)
        {
            try
            {
                if (!await VerificarPermiso(17))
                {
                    return RedirectToAction("AccessDenied", "Cuenta");
                }


                var bitacoraMovimientos = _context.BitacoraMovimientos.AsQueryable();


                if (bitacoraMovimientos != null)
                {
                    return View(bitacoraMovimientos);
                }
                else
                {
                    ModelState.AddModelError("", "No existen Movimientos registrados");
                }
               

                return View(bitacoraMovimientos);

            }
            catch (Exception ex)
            {
                // Loggear el error o manejarlo según sea necesario
                return Problem("Se produjo un error al cargar los datos: " + ex.Message);
            }
        }


    }
}
