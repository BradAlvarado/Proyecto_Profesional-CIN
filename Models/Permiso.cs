using System;
using System.Collections.Generic;

namespace Sistema_CIN.Models
{
    public partial class Permiso
    {
        public int IdPermiso { get; set; }
        public int? IdUsuario { get; set; }
        public int? IdModulo { get; set; }
        public bool? Permitido { get; set; }

        public virtual Modulo? IdModuloNavigation { get; set; }
        public virtual Usuario? IdUsuarioNavigation { get; set; }
    }
}
