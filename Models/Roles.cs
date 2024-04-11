using System;
using System.Collections.Generic;

namespace Sistema_CIN.Models
{
    public partial class Roles
    {
        public Roles()
        {
            Personals = new HashSet<Personal>();
            Usuarios = new HashSet<Usuario>();
        }

        public int IdRol { get; set; }
        public string NombreRol { get; set; } = null!;

        public virtual ICollection<Personal> Personals { get; set; }
        public virtual ICollection<Usuario> Usuarios { get; set; }
    }
}
