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
        public byte[]? Imagen_U { get; set; }

        public int Id_Usuario { get; set; }
        public string? Nombre_U { get; set; }
        public string? Clave { get; set; }
        public string Correo_U { get; set; } = null!;
        
        public int? Id_Rol { get; set; }

        public virtual Role? IdRolNavigation { get; set; }
        public virtual ICollection<Permiso> Permisos { get; set; }
    }
}
