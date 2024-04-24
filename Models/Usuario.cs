using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sistema_CIN.Models
{
    public partial class Usuario
    {
        public int IdUsuario { get; set; }
        public string? FotoU { get; set; }
        [Required(ErrorMessage = "El campo Nombre de usuario es obligatorio.")]
        public string NombreU { get; set; } = null!;

        [Required(ErrorMessage = "El campo Correo es obligatorio.")]
        [DataType(DataType.EmailAddress, ErrorMessage ="El campo no es tipo email")]
        public string? CorreoU { get; set; }

        [Required(ErrorMessage = "El campo Contraseña es obligatorio.")]
        [DataType(DataType.Password)]
        [MinLength(6, ErrorMessage = "Tu contraseña es muy corta")]
        public string Clave { get; set; } = null!;

        [NotMapped]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "El campo Confirmar clave es obligatorio.")]
        [Compare("Clave", ErrorMessage = "Las contraseñas no coinciden.")]
        public string? ConfirmarClave { get; set; } 
        public bool? EstadoU { get; set; }
        public bool? AccesoU { get; set; }
        public int? IdRol { get; set; }

        public virtual Rol? IdRolNavigation { get; set; }
    }
}
