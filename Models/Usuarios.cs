using System;
using System.Collections.Generic;

namespace Sistema_CIN.Models
{
    public partial class Usuario
    {
        public int id_usuario { get; set; }
        public string? nombre_u { get; set; }
        public string? clave { get; set; }
        public string? correo_u { get; set; }
        public int? id_rol { get; set; }
    }
}
