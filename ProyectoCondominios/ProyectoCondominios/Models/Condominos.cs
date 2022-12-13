using Microsoft.AspNetCore.Mvc.Rendering;
using System.Web;
namespace ProyectoCondominios.Models
{
    public class Condominos
    {
        public int? Id { get; set; }
        public string Cedula { get; set; }
        public string Nombre { get; set; }
        public string Telefonos { get; set; }
        public string Email { get; set; }
        public byte[] Foto { get; set; }
        public IFormFile Archivo { get; set; }
        public IEnumerable<SelectListItem> ListaCondominos { get; set; }
    }
}
