using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Permissions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Sistema_CIN.Data;
using Sistema_CIN.Services;

namespace Sistema_CIN.Controllers
{
    public class BitacoraIngresoSalidasController : Controller
    {
        private readonly SistemaCIN_dbContext _context;
        private readonly FiltrosPermisos _filters;

        public BitacoraIngresoSalidasController(SistemaCIN_dbContext context, FiltrosPermisos filtro)
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

        // GET: BitacoraIngresoSalidas
        public async Task<IActionResult> Index()
        {
            try
            {
                if (!await VerificarPermiso())
                {
                    return RedirectToAction("AccessDenied", "Cuenta");
                }
                var bitacoraIngresoSalidas = await _context.BitacoraIngresoSalida.ToListAsync();
                if (bitacoraIngresoSalidas != null)
                {
                    return View(bitacoraIngresoSalidas);
                }else{
                    ModelState.AddModelError("", "No existen Usuarios registrados");
                }

                return View(bitacoraIngresoSalidas);

            }
            catch (Exception ex)
            {
                // Loggear el error o manejarlo según sea necesario
                return Problem("Se produjo un error al cargar los datos: " + ex.Message);
            }
        }
    }
}

