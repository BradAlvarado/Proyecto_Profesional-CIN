using System;
using System.Collections.Generic;

namespace Sistema_CIN.Models
{
    public partial class Personal
    {
        public int IdPersonal { get; set; }
        public string? CedulaP { get; set; }
        public string? NombreP { get; set; }
        public string? ApellidosP { get; set; }
        public string? CorreoP { get; set; }
        public string? PuestoP { get; set; }
        public DateTime? FechaNaceP { get; set; }
        public int? EdadP { get; set; }
        public string? GeneroP { get; set; }
        public string? ProvinciaP { get; set; }
        public string? CantonP { get; set; }
        public string? DistritoP { get; set; }
        public int? IdRol { get; set; }

        public virtual Role? IdRolNavigation { get; set; }
    }
}
