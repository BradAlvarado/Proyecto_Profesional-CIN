using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Sistema_CIN.Models
{
    public partial class Personal
    {
        public int IdPersonal { get; set; }
        [Required(ErrorMessage = "Este campo es obligatorio.")]
        public string CedulaP { get; set; } = null!;
        [Required(ErrorMessage = "Este campo es obligatorio.")]
        public string NombreP { get; set; } = null!;
        [Required(ErrorMessage = "Este campo es obligatorio.")]
        public string ApellidosP { get; set; } = null!;
        [Required(ErrorMessage = "Este campo es obligatorio.")]
        public string CorreoP { get; set; } = null!;
        [Required(ErrorMessage = "Este campo es obligatorio.")]
        public string TelefonoP { get; set; } = null!;
        [Required(ErrorMessage = "Este campo es obligatorio.")]
        public DateTime? FechaNaceP { get; set; }
        public int EdadP { get; set; }
        [Required(ErrorMessage = "Este campo es obligatorio.")]
        public string GeneroP { get; set; } = null!;
        [Required(ErrorMessage = "Este campo es obligatorio.")]
        public string ProvinciaP { get; set; } = null!;
        public string? CantonP { get; set; }
        public string? DistritoP { get; set; }
        public int? IdRol { get; set; }

        public virtual Rol? IdRolNavigation { get; set; }
    }
}
