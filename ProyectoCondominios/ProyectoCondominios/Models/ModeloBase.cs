namespace Proyectocondominos.Models
{
    public class ModeloBase
    {
        public ErrorViewModel Error { get; set; }
        public dynamic Datos { get; set; }
        public dynamic Objeto { get; set; }
        public bool Buscando { get; internal set; }
    }
}
