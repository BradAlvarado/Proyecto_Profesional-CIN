using System;
using System.Collections.Generic;

namespace Sistema_CIN.Models
{
    public partial class Pme
    {
        public Pme()
        {
            Encargados = new HashSet<Encargado>();
        }

        public int IdPme { get; set; }
        public string? CedulaPme { get; set; }
        public string? PolizaSeguro { get; set; }
        public string? NombrePme { get; set; }
        public string? ApellidosPme { get; set; }
        public DateTime? FechaNacimientoPme { get; set; }
        public int? EdadPme { get; set; }
        public string? GeneroPme { get; set; }
        public string? ProvinciaPme { get; set; }
        public string? CantonPme { get; set; }
        public string? DistritoPme { get; set; }
        public string? NacionalidadPme { get; set; }
        public bool? SubvencionPme { get; set; }
        public DateTime? FechaIngresoPme { get; set; }
        public DateTime? FechaEgresoPme { get; set; }
        public string? CondiciónMigratoriaPme { get; set; }
        public string? NivelEducativoPme { get; set; }
        public string? EncargadoPme { get; set; }
        public int? IdEncargado { get; set; }

        public virtual Encargado? IdEncargadoNavigation { get; set; }
        public virtual ICollection<Encargado> Encargados { get; set; }
    }
}
