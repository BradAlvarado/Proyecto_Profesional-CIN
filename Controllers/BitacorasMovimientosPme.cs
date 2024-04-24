using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        private async Task<bool> VerificarPermiso()
        {
            string emailUser = User.FindFirst(ClaimTypes.Email)?.Value ?? "desconocido";
            int cantidadOperaciones = await _filters.VerificarPermiso(emailUser, 16);
            return cantidadOperaciones > 0;
        }
        // GET: BitacorasMovimientosPme

        public async Task<IActionResult> Index()
        {
            try
            {
                if (!await VerificarPermiso())
                {
                    return RedirectToAction("AccessDenied", "Cuenta");
                }

                var usuario = User.FindFirstValue(ClaimTypes.Name);
                

                var bitacoraMovimientos = await _context.BitacoraMovimientos.ToListAsync();
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
