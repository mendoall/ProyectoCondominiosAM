using Proyectocondominos.Models;

namespace ProyectoCondominios.Models
{
    public class OficialDeSeguridad : UsuarioActivo
    {
        public OficialDeSeguridad()
        {

        }

        public OficialDeSeguridad(UsuarioActivo usuario)
        {
            Id = usuario.Id;
            Nombre = usuario.Nombre;
            Usuario = usuario.Usuario; 
            Contrasena = usuario.Contrasena;
            ResetearContrasena = usuario.ResetearContrasena;
            TipoUsuarioId = usuario.TipoUsuarioId;
        }
    }
}
