using System;
using System.Collections.Generic;

namespace Sistema_CIN.Models
{
    public partial class Encargados
    {
        public Encargados()
        {
            Pmes = new HashSet<Pme>();
        }

        public int IdEncargado { get; set; }
        public string? CedulaE { get; set; }
        public string? NombreE { get; set; }
        public string? ApellidosE { get; set; } 
        public DateTime? FechaNaceE { get; set; }
        public int? Edad { get; set; }
        public string? CorreoE { get; set; } 
        public string? DireccionE { get; set; }
        public string? TelefonoE { get; set; } 
        public string? LugarTrabajoE { get; set; }
        public int? IdPme { get; set; }

        public virtual Pme? IdPmeNavigation { get; set; }
        public virtual ICollection<Pme> Pmes { get; set; }
    }
}
