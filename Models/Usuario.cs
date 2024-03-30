using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sistema_CIN.Models
{
    public partial class Usuario
    {
        public int IdUsuario { get; set; }

        public string? FotoU { get; set; }
        [NotMapped]
        public IFormFile File { get; set; } // Propiedad para el archivo de imagen enviado desde el formulario
        public string NombreU { get; set; } = null!;
        public string CorreoU { get; set; } = null!;
        public string Clave { get; set; } = null!;
        public string? Token { get; set; }
        public bool? EstadoU { get; set; }
        public bool? AccesoU { get; set; }
        public int? IdRol { get; set; }

        public virtual Roles? IdRolNavigation { get; set; }
    }
}
