using System;
using System.Collections.Generic;

namespace Sistema_CIN.Models
{
    public partial class Pme
    {
        public Pme()
        {
            Encargados = new HashSet<Encargados>();
        }

        public int IdPme { get; set; }
        public string CedulaPme { get; set; } = null!;
        public string? PolizaSeguro { get; set; }
        public string NombrePme { get; set; } = null!;
        public string ApellidosPme { get; set; } = null!;
        public DateTime FechaNacimientoPme { get; set; }
        public int EdadPme { get; set; }
        public string GeneroPme { get; set; } = null!;
        public string ProvinciaPme { get; set; } = null!;
        public string? CantonPme { get; set; }
        public string? DistritoPme { get; set; }
        public string NacionalidadPme { get; set; } = null!;
        public bool? SubvencionPme { get; set; }
        public DateTime FechaIngresoPme { get; set; }
        public DateTime? FechaEgresoPme { get; set; }
        public string? CondiciónMigratoriaPme { get; set; }
        public string? NivelEducativoPme { get; set; }
        public int? IdEncargado { get; set; }

        public virtual Encargados? IdEncargadoNavigation { get; set; }
        public virtual ICollection<Encargados> Encargados { get; set; }
    }
}
