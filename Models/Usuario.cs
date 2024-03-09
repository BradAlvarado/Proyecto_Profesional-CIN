using System;
using System.Collections.Generic;

namespace Sistema_CIN.Models
{
    public partial class Usuario
    {
        public string NombreUsuario { get; set; } = null!;
        public string Correo { get; set; } = null!;
        public string Clave { get; set; } = null!;
    }
}
