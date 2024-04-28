using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sistema_CIN.Models
{
    public partial class Usuario
    {
        public int IdUsuario { get; set; }
        public string? FotoU { get; set; }
        [Required(ErrorMessage = "Este campo es obligatorio.")]
        public string NombreU { get; set; } = null!;

        [Required(ErrorMessage = "Este campo es obligatorio.")]
        [EmailAddress(ErrorMessage = "Este campo debe ser de tipo Correo")]
        public string? CorreoU { get; set; }

        [Required(ErrorMessage = "Este campo es obligatorio.")]
        [DataType(DataType.Password)]
        [MinLength(6, ErrorMessage = "Tu contraseña debe de ser de 6 caracteres como mínimo")]
        public string Clave { get; set; } = null!;

        [NotMapped]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Este campo es obligatorio.")]
        [Compare("Clave", ErrorMessage = "Las contraseñas no coinciden.")]
        public string? ConfirmarClave { get; set; } 
        public bool? EstadoU { get; set; }
        public bool? AccesoU { get; set; }
        public int? IdRol { get; set; }
        [NotMapped]
        public IFormFile FrontImage { get; set; }   

        public virtual Rol? IdRolNavigation { get; set; }
    }
}
