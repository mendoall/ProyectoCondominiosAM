using Microsoft.AspNetCore.Mvc.Rendering;
using Proyectocondominos.Models;

namespace ProyectoCondominios.Models
{
    public class VisitaRapida
    {
        public int? Id { get; set; }
        public string Nombre { get; set; }
        public DateTime FechaVisita { get; set; }
        public int TipoVisitaRapidaId { get; set; }
        public IEnumerable<SelectListItem> TipoDeVisitaRapida { get; set; }
    }
}
