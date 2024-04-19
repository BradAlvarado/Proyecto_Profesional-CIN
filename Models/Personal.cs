using System;
using System.Collections.Generic;

namespace Sistema_CIN.Models
{
    public partial class Personal
    {
        public int IdPersonal { get; set; }
        public string CedulaP { get; set; } = null!;
        public string NombreP { get; set; } = null!;
        public string ApellidosP { get; set; } = null!;
        public string CorreoP { get; set; } = null!;
        public string? TelefonoP { get; set; }
        public DateTime? FechaNaceP { get; set; }
        public int EdadP { get; set; }
        public string GeneroP { get; set; } = null!;
        public string ProvinciaP { get; set; } = null!;
        public string? CantonP { get; set; }
        public string? DistritoP { get; set; }
        public int? IdRol { get; set; }

        public virtual Rol? IdRolNavigation { get; set; }
    }
}
