using System;
using System.Collections.Generic;

namespace Sistema_CIN.Models
{
    public partial class Permisos
    {
        public int IdPermiso { get; set; }
        public int? IdRol { get; set; }
        public int? IdModulo { get; set; }
        public bool? Permitido { get; set; }

        public virtual Modulos? IdModuloNavigation { get; set; }
        public virtual Roles? IdRolNavigation { get; set; }
    }
}
