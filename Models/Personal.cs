using System;
using System.Collections.Generic;

namespace Sistema_CIN.Models
{
    public partial class Personal
    {
        public int IdP { get; set; }
        public string? CedulaP { get; set; }
        public string? NombreP { get; set; }
        public string? ApellidosP { get; set; }
        public int? EdadP { get; set; }
        public string? ProvinciaP { get; set; }
        public string? CantonP { get; set; }
        public string? DistritoP { get; set; }
        public string? GeneroP { get; set; }
        public string? IdRol { get; set; }
        public string? CorreoP { get; set; }
    }
}
