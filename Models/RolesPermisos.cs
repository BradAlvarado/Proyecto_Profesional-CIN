namespace Sistema_CIN.Models
{
    public class RolesPermisos
    { 
        public Rol Rol { get; set; }
        public List<Modulo> Modulos { get; set; }
        public List<Operaciones> Operaciones { get; set; }
        public List<RolOperacion> RolOperaciones { get; set; }

    }
}
