using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace Sistema_CIN.Models
{
    public partial class Personal
    {
        public int IdPersonal { get; set; }

        [Required(ErrorMessage = "El campo Cédula es obligatorio.")]
        public string CedulaP { get; set; } = null!;
        [Required(ErrorMessage = "El campo Nombre es obligatorio.")]
        public string NombreP { get; set; } = null!;
        [Required(ErrorMessage = "El campo de Apellidos es obligatorio.")]
        public string ApellidosP { get; set; } = null!;
        [Required(ErrorMessage = "El campo Correo es obligatorio.")]
        public string CorreoP { get; set; } = null!;
        [Required(ErrorMessage = "El campo Teléfono es obligatorio.")]
        public string TelefonoP { get; set; }
        public DateTime? FechaNaceP { get; set; }
        public int EdadP { get; set; }
        public string GeneroP { get; set; } = null!;
        public string ProvinciaP { get; set; } = null!;
        public string? CantonP { get; set; }
        public string? DistritoP { get; set; } 
        public int? IdRol { get; set; }
  
        public virtual Roles? IdRolNavigation { get; set; }
    }
}
