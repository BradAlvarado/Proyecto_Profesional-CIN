namespace Sistema_CIN.Models
{
    public partial class BitacoraMovimiento
    {
        public int IdBitacora { get; set; }
        public string? UsuarioB { get; set; }
        public DateTime? FechaMovimiento { get; set; }
        public string? TipoMovimiento { get; set; }
        public string? Detalle { get; set; }
    }
}
