using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sistema_CIN.Models
{
    public partial class Usuario : IdentityUser
    {
        public int IdUsuario { get; set; }
        public string? FotoU { get; set; }
        public string NombreU { get; set; } = null!;
        [Required]
        [DataType(DataType.EmailAddress)]
        public string CorreoU { get; set; } = null!;
        [Required]
        [DataType(DataType.Password)]
        public string? Clave { get; set; }

        [NotMapped]
        [Compare("Clave", ErrorMessage = "Las contraseñas no coinciden")]
        [Display(Name = "Confirmar contraseña")]
        [DataType(DataType.Password)]
        public string? ConfirmarClave { get; set; }
        public string? Token { get; set; }
        public bool? EstadoU { get; set; }
        public bool? AccesoU { get; set; }
        public int? IdRol { get; set; }

        public virtual Roles? IdRolNavigation { get; set; }
    }
}
