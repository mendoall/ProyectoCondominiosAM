using Microsoft.AspNetCore.Mvc.Rendering;
using Proyectocondominos.Models;

namespace ProyectoCondominios.Models
{
    public class CondominosPorProyecto
    {
        public int Id { get; set; }
        public ErrorViewModel Error { get; set; }
        public IEnumerable<SelectListItem> Proyectos { get; set; }
        public IEnumerable<SelectListItem> Condominos { get; set; }
        public int? ProyectoId { get; set; }
        public int? CondominoId { get; set; }
        public string Proyecto { get; set; }
        public string Condomino { get; set; }

    }
}
