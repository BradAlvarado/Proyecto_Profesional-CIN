using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Sistema_CIN.Models
{
    public partial class Rol
    {
        public Rol()
        {
            Personals = new HashSet<Personal>();
            RolOperacions = new HashSet<RolOperacion>();
            Usuarios = new HashSet<Usuario>();
        }

        public int IdRol { get; set; }

        [Required(ErrorMessage = "Este campo es obligatorio.")]
        public string NombreRol { get; set; } = null!;

        public virtual ICollection<Personal> Personals { get; set; }
        public virtual ICollection<RolOperacion> RolOperacions { get; set; }
        public virtual ICollection<Usuario> Usuarios { get; set; }
    }
}
