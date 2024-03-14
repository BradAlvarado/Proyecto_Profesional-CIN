using System;
using System.Collections.Generic;

namespace Sistema_CIN.Models
{
    public partial class Modulo
    {
        public Modulo()
        {
            Permisos = new HashSet<Permiso>();
        }

        public int IdModulo { get; set; }
        public string? NombreModulo { get; set; }

        public virtual ICollection<Permiso> Permisos { get; set; }
    }
}
