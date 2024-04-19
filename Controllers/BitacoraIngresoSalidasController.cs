using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Permissions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Sistema_CIN.Data;

namespace Sistema_CIN.Controllers
{
    [Authorize, ]
    public class BitacoraIngresoSalidasController : Controller
    {
        private readonly SistemaCIN_dbContext _context;

        public BitacoraIngresoSalidasController(SistemaCIN_dbContext context)
        {
            _context = context;
        }

        // GET: BitacoraIngresoSalidas
        public async Task<IActionResult> Index()
        {
            try
            {
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

