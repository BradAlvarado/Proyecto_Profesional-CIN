﻿using System;
using System.Collections.Generic;

namespace Sistema_CIN.Models
{
    public partial class Roles
    {
        public int id_rol { get; set; }
        public string? nombre_Rol { get; set; }

        public ICollection<Usuarios> Usuarios { get; set; }
    }
}
