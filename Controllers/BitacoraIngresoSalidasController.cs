using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Sistema_CIN.Models;

namespace Sistema_CIN.Controllers
{
    public class BitacoraIngresoSalidasController : Controller
    {
        private readonly CIN_pruebaContext _context;

        public BitacoraIngresoSalidasController(CIN_pruebaContext context)
        {
            _context = context;
        }

        // GET: BitacoraIngresoSalidas
        public async Task<IActionResult> Index()
        {
              return _context.BitacoraIngresoSalida != null ? 
                          View(await _context.BitacoraIngresoSalida.ToListAsync()) :
                          Problem("Entity set 'CIN_pruebaContext.BitacoraIngresoSalida'  is null.");
        }

        private bool BitacoraIngresoSalidasExists(int id)
        {
          return (_context.BitacoraIngresoSalida?.Any(e => e.IdBitacora == id)).GetValueOrDefault();
        }
    }
}
