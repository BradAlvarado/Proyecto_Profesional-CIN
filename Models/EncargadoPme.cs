using System;
using System.Collections.Generic;

namespace Sistema_CIN.Models
{
    public partial class EncargadoPme
    {
        public EncargadoPme()
        {
            Pmes = new HashSet<Pme>();
        }

        public string CedulaE { get; set; } = null!;
        public string? NombreE { get; set; }
        public string? ApellidosE { get; set; }
        public DateTime? FechaNaceE { get; set; }
        public string? DireccionE { get; set; }
        public string? TelefonoE { get; set; }
        public string? CorreoE { get; set; }
        public string? LugarTrabajoE { get; set; }
        public string? EncargadoDeE { get; set; }

        public virtual Pme? EncargadoDeENavigation { get; set; }
        public virtual ICollection<Pme> Pmes { get; set; }
    }
}
