namespace Sistema_CIN.Models
{
    public partial class RolOperacion
    {
        public int IdRolOp { get; set; }
        public int? IdRol { get; set; }
        public int? IdOp { get; set; }

        public virtual Operaciones? IdOpNavigation { get; set; }
        public virtual Rol? IdRolNavigation { get; set; }

    }
}
