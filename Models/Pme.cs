using System.ComponentModel.DataAnnotations;

namespace Sistema_CIN.Models
{
    public partial class Pme
    {
        public Pme()
        {
            Encargados = new HashSet<Encargados>();
        }

        public int IdPme { get; set; }
        public int? IdEncargado { get; set; }

        [Required(ErrorMessage = "Este campo es obligatorio.")]
        public string CedulaPme { get; set; } = null!;
        public string? PolizaSeguro { get; set; }
        [Required(ErrorMessage = "Este campo es obligatorio.")]
        public string NombrePme { get; set; } = null!;
        [Required(ErrorMessage = "Este campo es obligatorio.")]
        public string ApellidosPme { get; set; } = null!;

        [Required(ErrorMessage = "Este campo es obligatorio.")]
        [DataType(DataType.Date)]
        public DateTime FechaNacimientoPme { get; set; }
        public int EdadPme { get; set; }
        [Required(ErrorMessage = "Este campo es obligatorio.")]
        public string GeneroPme { get; set; } = null!;
        [Required(ErrorMessage = "Este campo es obligatorio.")]
        public string ProvinciaPme { get; set; } = null!;
        public string? CantonPme { get; set; }
        public string? DistritoPme { get; set; }
        [Required(ErrorMessage = "Este campo es obligatorio.")]
        public string NacionalidadPme { get; set; } = null!;
        public bool? SubvencionPme { get; set; }
        [Required(ErrorMessage = "Este campo es obligatorio.")]
        [DataType(DataType.Date)]
        public DateTime FechaIngresoPme { get; set; }
		
		public DateTime? FechaEgresoPme { get; set; }
        public string? CondiciónMigratoriaPme { get; set; }
        public string? NivelEducativoPme { get; set; }
		public virtual Encargados? IdEncargadoNavigation { get; set; }
        public virtual ICollection<Encargados> Encargados { get; set; }
    }
}
