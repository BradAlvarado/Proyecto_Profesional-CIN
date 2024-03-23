using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sistema_CIN.Models
{
    public partial class Usuario
    {
        public Usuario()
        {
            Permisos = new HashSet<Permiso>();
        }

        public int IdUsuario { get; set; }
        public byte[]? ImagenU { get; set; }
        public string NombreU { get; set; } = null!;
        public string CorreoU { get; set; } = null!;


        public string Clave { get; set; } = null!;


        [NotMapped]
        public string? ConfirmacionClave { get; set; }// Confirmación de contraseña (no mapeada a la base de datos)
        public string? NombreRolU { get; set; }
        public bool? EstadoU { get; set; }
        public int? IdRol { get; set; }

        public virtual Roles? IdRolNavigation { get; set; }
        public virtual ICollection<Permiso> Permisos { get; set; }

   
    }
}
