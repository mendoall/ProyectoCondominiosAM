using Proyectocondominos.Models;

namespace ProyectoCondominios.Controllers
{
    public interface IMetodosBase<T>
    {
         UsuarioActivo _usuario { get; set; }

        IConfiguration _configuration { get; }

        public void ValidarUsuario();

        public List<T> ObtenerDatos();
    }
}
