using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sistema_CIN.Models
{
    public partial class Usuarios
    {
        [Key]
        public int id_usuario { get; set; }
        public string? nombre_u { get; set; }
        public string? clave { get; set; }
        public string? correo_u { get; set; }

        [ForeignKey("Roles")]
        public int? id_rol { get; set; }

        public Roles Roles { get; set; }
    }
}
