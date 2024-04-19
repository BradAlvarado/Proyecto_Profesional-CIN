using System;
using System.Collections.Generic;

namespace Sistema_CIN.Models
{
    public partial class Operaciones
    {
        public Operaciones()
        {
            RolOperacions = new HashSet<RolOperacion>();
        }

        public int IdOp { get; set; }
        public string NombreOp { get; set; } = null!;
        public int? IdModulo { get; set; }

        public virtual Modulo? IdModuloNavigation { get; set; }
        public virtual ICollection<RolOperacion> RolOperacions { get; set; }
    }
}
