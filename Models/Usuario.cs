using System;
using System.Collections.Generic;

namespace Sistema_CIN.Models
{
    public partial class Usuario
    {
        public Usuario()
        {
            Permisos = new HashSet<Permiso>();
        }

        public int IdUsuario { get; set; }
        public string? NombreU { get; set; }
        public string? Clave { get; set; }
        public string CorreoU { get; set; } = null!;
        public byte[]? ImagenU { get; set; }
        public int? IdRol { get; set; }

        public virtual Role? IdRolNavigation { get; set; }
        public virtual ICollection<Permiso> Permisos { get; set; }
    }
}
