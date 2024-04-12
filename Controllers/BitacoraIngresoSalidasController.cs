using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
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
			try
			{
				var bitacoraIngresoSalidas = await _context.BitacoraIngresoSalida.ToListAsync();
				if (bitacoraIngresoSalidas != null)
				{
					return View(bitacoraIngresoSalidas);
				}
				else
				{
					return Problem("No se encontraron usuarios en la base de datos.");
				}
			}
			catch (Exception ex)
			{
				// Loggear el error o manejarlo según sea necesario
				return Problem("Se produjo un error al cargar los datos: " + ex.Message);
			}
		}
	}
}

