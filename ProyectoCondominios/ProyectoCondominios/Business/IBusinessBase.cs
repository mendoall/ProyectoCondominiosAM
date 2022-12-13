using ProyectoCondominios.Models;
using Proyectocondominos.Models;

namespace ProyectoCondominios.Business
{
    public interface IBusinessBase<T>
    {
        public List<T> Obtener(T modelo, int usuarioId);

        public bool Crear(T modelo, int usuarioId);

        public bool Editar(T modelo, int usuarioId);

        public bool Eliminar(int id, int usuarioId);
    }
}
