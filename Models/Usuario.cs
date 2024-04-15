﻿using Microsoft.AspNetCore.Identity;
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
        public string NombreU { get; set; } = null!;

        [Required(ErrorMessage = "El campo Correo es obligatorio.")]
        [DataType(DataType.EmailAddress)]
        public string CorreoU { get; set; } = null!;

        [Required(ErrorMessage = "El campo Contraseña es obligatorio.")]
        [DataType(DataType.Password)]
        public string Clave { get; set; } = null!;

        [NotMapped]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "El campo Cédula es obligatorio.")]
        [Compare("Clave", ErrorMessage = "Las contraseñas no coinciden.")]
        public string? ConfirmarClave { get; set; }

        public string? Token { get; set; }
        public bool? EstadoU { get; set; }
        public bool? AccesoU { get; set; }
        public int? IdRol { get; set; }

        public virtual Roles? IdRolNavigation { get; set; }
    }
}
