using System;
using System.Collections.Generic;

namespace Sistema_CIN.Models
{
    public partial class Role
    {
        public Role()
        {
            Personals = new HashSet<Personal>();
            Usuarios = new HashSet<Usuario>();
        }

        public int IdRol { get; set; }
        public string? NombreRol { get; set; }

        public virtual ICollection<Personal> Personals { get; set; }
        public virtual ICollection<Usuario> Usuarios { get; set; }
    }
}
