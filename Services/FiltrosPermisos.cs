using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sistema_CIN.Data;
using Sistema_CIN.Models;
using System.Security.Claims;

namespace Sistema_CIN.Services
{
    public class FiltrosPermisos
    {
        private readonly SistemaCIN_dbContext _context;
        public FiltrosPermisos(SistemaCIN_dbContext context)
        {
            _context = context;
        }

        public async Task<int> VerificarPermiso(string correo, int idOperacion)
        {
            // Obtener el usuario por correo electrónico
            var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.CorreoU == correo);

            if (usuario == null)
            {
                throw new InvalidOperationException($"El usuario con el correo {correo} no existe.");
            }

            // Realizar las operaciones de autorización y contar la cantidad de operaciones
            var misOperaciones = await _context.RolOperacions
                .Where(m => m.IdRol == usuario.IdRol && m.IdOp == idOperacion)
                .ToListAsync();

            return misOperaciones.Count;
        }
    }
}
