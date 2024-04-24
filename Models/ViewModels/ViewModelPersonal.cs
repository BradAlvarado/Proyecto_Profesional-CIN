namespace Sistema_CIN.Models.ViewModels
{
    public class ViewModelPersonal
    {
        public string cedulaP { get; set; }
        public string nombreP { get; set; }
        public string apellidosP { get; set; }
        public string correoP { get; set; }
        public string telefonoP { get; set; }
        public DateTime? fechaNaceP { get; set; }
        public int edadP { get; set; }
        public string generoP { get; set; }
        public string provinciaP { get; set; }
        public string cantonP { get; set; }
        public string distritoP { get; set; }
    }

    public class ViewModelRol
    {

        public string nombreRol { get; set; } = null!;
        public List<ViewModelPersonal> personal { get; set; }

    }
}
