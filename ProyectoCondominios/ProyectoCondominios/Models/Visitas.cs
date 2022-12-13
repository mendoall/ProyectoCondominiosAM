using Microsoft.AspNetCore.Mvc.Rendering;

namespace ProyectoCondominios.Models
{
    public class Visitas
    {
        public int? Id { get; set; }
        public string Nombre { get; set; }
        public string Cedula { get; set; }
        public string Vehiculo { get; set; }
        public IEnumerable<SelectListItem> VisitasFavoritas { get; set; }
        public DateTime FechaVisita { get; set; } = DateTime.Now;
        public int ProyectoId { get; internal set; }
    }
}
