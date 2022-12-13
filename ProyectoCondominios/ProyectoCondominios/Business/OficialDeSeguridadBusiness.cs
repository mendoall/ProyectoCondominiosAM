using ProyectoCondominios.Models;
using Proyectocondominos.Business;
using Proyectocondominos.Models;
using Proyectocondominos.Services;
using System.Data;
using System.Reflection;

namespace ProyectoCondominios.Business
{
    public class OficialDeSeguridadBusiness : IBusinessBase<OficialDeSeguridad>
    {
        private UsuariosBusiness _usuariosBusiness;

        public OficialDeSeguridadBusiness(IConfiguration configuration)
        {
            _usuariosBusiness = new UsuariosBusiness(configuration);
        }

        public bool Crear(OficialDeSeguridad modelo, int usuarioId)
        {
            return _usuariosBusiness.Crear(modelo, usuarioId);
        }

        public bool Editar(OficialDeSeguridad modelo, int usuarioId)
        {
            return _usuariosBusiness.Editar(modelo, usuarioId);
        }

        public bool Eliminar(int id, int usuarioId)
        {
            return _usuariosBusiness.Eliminar(id, usuarioId);
        }

        public List<OficialDeSeguridad> Obtener(OficialDeSeguridad modelo, int usuarioId)
        {
            var list = _usuariosBusiness.Obtener(modelo, usuarioId);
            var resultado = new List<OficialDeSeguridad>();
            if (list != null && list.Any())
            {
                foreach (var item in list)
                {
                    resultado.Add(new OficialDeSeguridad(item));
                }
            }
            return resultado;
        }
    }
}
