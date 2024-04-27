using Microsoft.Build.Evaluation;
using System.Reflection;

namespace Sistema_CIN.Models.ViewModels
{
    public class RolOpViewModel
    {
        public int RoleId { get; set; }
        public List<Modulo> Modulos { get; set; }
        public List<Operaciones> Operacion { get; set; }
        public Dictionary<int, bool> ModuloChecked { get; set; } // Change to Dictionary<int, bool>
        public Dictionary<int, bool> OperacionChecked { get; set; } // Change to Dictionary<int, bool>

        public RolOpViewModel()
        {
            OperacionChecked = new Dictionary<int, bool>();
        }
    }


}
