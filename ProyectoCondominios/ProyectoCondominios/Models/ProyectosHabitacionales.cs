using Microsoft.AspNetCore.Mvc.Rendering;

namespace Proyectocondominos.Models
{
    public class ProyectosHabitacionales
    {
        public int Id { get; set; }
        public byte[] Logo { get; set; }
        public string Codigo { get; set; }
        public string Nombre { get; set; }
        public string Direccion { get; set; }
        public string TelefonoOficina { get; set; }
        public IEnumerable<SelectListItem> Proyectos { get; set; }
        public IFormFile Archivo { get; set; }
    }
}
