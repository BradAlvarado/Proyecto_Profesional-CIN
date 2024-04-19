using System;
using System.Collections.Generic;

namespace Sistema_CIN.Models
{
    public partial class Modulo
    {
        public Modulo()
        {
            Operaciones = new HashSet<Operaciones>();
        }

        public int IdModulo { get; set; }
        public string? NombreModulo { get; set; }

        public virtual ICollection<Operaciones> Operaciones { get; set; }
    }
}
