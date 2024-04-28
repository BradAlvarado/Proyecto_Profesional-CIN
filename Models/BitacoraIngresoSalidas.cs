namespace Sistema_CIN.Models
{
    public partial class BitacoraIngresoSalidas
    {
        public int IdBitacora { get; set; }
        public string? UsuarioB { get; set; }
        public DateTime? FechaIngreso { get; set; }
        public DateTime? FechaSalida { get; set; }
        public int? EstadoActual { get; set; }
    }
}
