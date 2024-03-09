using System;
using System.Collections.Generic;

namespace Sistema_CIN.Models
{
    public partial class Personal
    {
        public string? CedulaPersonal { get; set; }
        public string? NombrePersonal { get; set; }
        public string? ApellidosPersonal { get; set; }
        public string? DireccionProvinciaPersonal { get; set; }
        public string? DireccionCantonPersonal { get; set; }
        public string? DireccionDistritoPersonal { get; set; }
        public string? OtrasSenasPersonal { get; set; }
        public string? TelefonoPersonal { get; set; }
        public string? CorreoPersonal { get; set; }
        public string? GeneroPersonal { get; set; }
        public string? FechaNacimientoPersonal { get; set; }
        public string? PuestoPersonal { get; set; }
    }
}
