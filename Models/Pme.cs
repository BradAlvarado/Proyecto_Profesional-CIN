using System;
using System.Collections.Generic;

namespace Sistema_CIN.Models
{
    public partial class Pme
    {
        public Pme()
        {
            EncargadoPmes = new HashSet<EncargadoPme>();
        }

        public string CedulaPme { get; set; } = null!;
        public string? PolizaSeguro { get; set; }
        public string? NombrePme { get; set; }
        public string? ApellidosPme { get; set; }
        public int? EdadPme { get; set; }
        public DateTime? FechaNacimientoPme { get; set; }
        public string? ProvinciaPme { get; set; }
        public string? CantonPme { get; set; }
        public string? DistritoPme { get; set; }
        public string? NacionalidadPme { get; set; }
        public bool? SubvencionPme { get; set; }
        public DateTime? FechaIngresoPme { get; set; }
        public DateTime? FechaEgresoPme { get; set; }
        public string? CondiciónMigratoriaPme { get; set; }
        public string? GeneroPme { get; set; }
        public string? NivelEducativoPme { get; set; }
        public string? EncargadoPme { get; set; }
        public string? CedulaE { get; set; }

        public virtual EncargadoPme? EncargadoPmeNavigation { get; set; }
        public virtual ICollection<EncargadoPme> EncargadoPmes { get; set; }
    }
}
