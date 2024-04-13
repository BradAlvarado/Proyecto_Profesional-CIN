using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace Sistema_CIN.Models
{
    public partial class Usuario : IdentityUser
    {
        public int IdUsuario { get; set; }
        public string? FotoU { get; set; }
        public string NombreU { get; set; } = null!;
        public string CorreoU { get; set; } = null!;
        public string Clave { get; set; } = null!;
        public string? Token { get; set; }
        public bool? EstadoU { get; set; }
        public bool? AccesoU { get; set; }
        public int? IdRol { get; set; }

        public virtual Roles? IdRolNavigation { get; set; }
    }
}
