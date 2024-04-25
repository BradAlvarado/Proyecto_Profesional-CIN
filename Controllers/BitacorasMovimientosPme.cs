using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Sistema_CIN.Data;
using Sistema_CIN.Models;
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

      
                var pageNumber = page ?? 1; // Número de página actual
                var pageSize = 15; // Número de elementos por página

                var bitacoraMovimientos = from BitacoraMovimiento in _context.BitacoraMovimientos select BitacoraMovimiento;


                if (bitacoraMovimientos != null)
                {
                    return View(bitacoraMovimientos);
                }
                else
                {
                    ModelState.AddModelError("", "No existen Movimientos registrados");
                }
                var pagedBitacora = await bitacoraMovimientos.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();

                // Calcular el número total de páginas
                var totalItems = await bitacoraMovimientos.CountAsync();
                var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

                // Crear un objeto de modelo para la paginación
                var pagedModel = new PagedList<BitacoraMovimiento>(pagedBitacora, pageNumber, pageSize, totalItems, totalPages);




                return View(pagedModel);

            }
            catch (Exception ex)
            {
                // Loggear el error o manejarlo según sea necesario
                return Problem("Se produjo un error al cargar los datos: " + ex.Message);
            }
        }


    }
}
