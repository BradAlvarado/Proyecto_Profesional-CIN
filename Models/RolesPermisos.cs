using System.ComponentModel.DataAnnotations.Schema;

namespace Sistema_CIN.Models
{
    public class RolesPermisos
    {
        public Rol Rol { get; set; }
        public List<Modulo> Modulos { get; set; }
        public List<Operaciones> Operaciones { get; set; }
        public List<RolOperacion> RolOperaciones { get; set; }
        public Dictionary<string, Dictionary<string, bool>> OperacionesPorModulo { get; set; }
        public string NombreRol { get; set; } // Propiedad para almacenar el nombre del rol
    }
    // Método para verificar si una operación está marcada para un rol dado
    //public bool IsOperacionChecked(int idRol, int idOperacion)
    //{
    //    return RolOperaciones.Any(ro => ro.IdRol == idRol && ro.IdOp == idOperacion);
    //}

}
