namespace Proyectocondominos.Models
{
    public class UsuarioActivo
    {
        public int Id { get; set; }
        public string Usuario { get; set; }
        public string Contrasena { get; set; }
        public int TipoUsuarioId { get; set; }
        public string Tipo { get; set; }
        public bool ResetearContrasena { get; set; }
        public string Nombre { get; set; }
        public int ProyectoId { get; set; }
        public int CondominoId { get; set; }
        public bool CambioDeContrasena { get; set; }
        public string ConfirmarContrasena { get; set; }
    }
}
