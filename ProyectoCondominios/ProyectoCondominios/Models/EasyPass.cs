using Proyectocondominos.Models;

namespace ProyectoCondominios.Models
{
    public class EasyPass
    {
        public int Id { get; set; }
        public int CondominoId { get; set; }
        public string Codigo { get; set; }
        public DateTime FechaExpiracion { get; set; }
        public byte[] Foto { get; set; }
        public string Email { get; set; }
        public ErrorViewModel Error { get; set; }
    }
}
