using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Sistema_CIN.Models
{
    public partial class Encargado
    {
        [Key]
        public string cedula_e { get; set; } = null!;
        public string? responsable_de { get; set; }
        public string? nombre_e { get; set; }
        public string? apellidos_e { get; set; }
        public int? edad_e { get; set; }
        public string? direccion_e { get; set; }
        public string? telefono_e { get; set; }
        public string? correo_e { get; set; }
        public string? lugar_trabajo_e { get; set; }
    }
}
