using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Sistema_CIN.Models
{
    public partial class Encargados
    {
        public Encargados()
        {
            Pmes = new HashSet<Pme>();
        }

        public int IdEncargado { get; set; }

        [Required(ErrorMessage = "El campo Cédula es obligatorio.")]
        public string CedulaE { get; set; } = null!;
        [Required(ErrorMessage = "El campo Nombre es obligatorio.")]
        public string NombreE { get; set; } = null!;
        [Required(ErrorMessage = "El campo Apellidos es obligatorio.")]
        public string ApellidosE { get; set; } = null!;
        [Required(ErrorMessage = "El campo Fecha de nacimiento es obligatorio para completar la Edad.")]
        public DateTime FechaNaceE { get; set; }
        [Required(ErrorMessage = "El campo Edad es obligatorio.")]
        public int Edad { get; set; }

        public string? CorreoE { get; set; }
        [Required(ErrorMessage = "El campo Dirección es obligatorio.")]
        public string DireccionE { get; set; } = null!;
        [Required(ErrorMessage = "El campo Teléfono es obligatorio.")]
        public string TelefonoE { get; set; } = null!;
        public string? LugarTrabajoE { get; set; }
        public int? IdPme { get; set; }

        public virtual Pme? IdPmeNavigation { get; set; }
        public virtual ICollection<Pme> Pmes { get; set; }
    }
}
