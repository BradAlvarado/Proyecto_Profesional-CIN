﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Sistema_CIN.Data;
using Sistema_CIN.Models;
using Microsoft.AspNetCore.Identity;

namespace Sistema_CIN.Controllers
{
    [Authorize(Policy = "RequireAdmin")]
    public class UsuariosController : Controller
    {
        private readonly SistemaCIN_dbContext _context;


        public UsuariosController(SistemaCIN_dbContext context)
        {
            _context = context;

        }

        // GET: Usuarios
        public async Task<IActionResult> Index() //no tiene Search User
        {
            var cINContext = _context.Usuarios.Include(u => u.IdRolNavigation);
            return View(await cINContext.ToListAsync());
        }

        // GET: Usuarios/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Usuarios == null)
            {
                return NotFound();
            }

            var usuario = await _context.Usuarios
                .Include(u => u.IdRolNavigation)
                .FirstOrDefaultAsync(m => m.IdUsuario == id);
            if (usuario == null)
            {
                return NotFound();
            }

            return View(usuario);
        }

        // GET: Usuarios/Create
        public IActionResult Create()
        {
            ViewData["IdRol"] = new SelectList(_context.Rols, "IdRol", "NombreRol");
            return View();
        }

        // GET: Usuarios/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Usuarios == null)
            {
                return NotFound();
            }

            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null)
            {
                return NotFound();
            }
            ViewData["IdRol"] = new SelectList(_context.Rols, "IdRol", "NombreRol", usuario.IdRol);
            return View(usuario);
        }

        // POST: Usuarios/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdUsuario,NombreU,CorreoU,Clave,AccesoU,IdRol")] Usuario usuario)
        {
            if (id != usuario.IdUsuario)
            {
                return NotFound();
            }

            try
            {
                _context.Update(usuario);
                await _context.SaveChangesAsync();

                if(usuario.AccesoU != true)
                {
                    // Obtener el ID del usuario de la sesión actual
                    var userId = HttpContext.Session.GetString("IdUsuario");
                    
                    // Verificar si el usuario editado es el mismo usuario que está en sesión
                    if (userId != null && userId == usuario.IdUsuario.ToString())
                    {
                        // Cerrar la sesión actual del usuario
                        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                    }
                }
                TempData["SuccessMessage"] = "Usuario " + usuario.NombreU + " actualizado exitosamente!";
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UsuarioExists(usuario.IdUsuario))
                {
                    ModelState.AddModelError("", "Error al obtener el Usuario");
                    return View(usuario);
                }
                else
                {
                    throw;
                }
            }
        }


        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            if (_context.Usuarios == null)
            {
                return Problem("Entity set 'CINContext.Pme'  is null.");
            }
            var user = await _context.Usuarios.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            _context.Usuarios.Remove(user);
            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = "Usuario " + user.NombreU + " eliminado exitosamente!";


            return Json(new { success = true });
        }

        private bool UsuarioExists(int id)
        {
            return (_context.Usuarios?.Any(e => e.IdUsuario == id)).GetValueOrDefault();
        }
    }
}
