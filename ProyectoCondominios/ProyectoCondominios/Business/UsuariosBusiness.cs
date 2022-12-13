using Proyectocondominos.Services;
using Proyectocondominos.Models;
using System.Data;
using ProyectoCondominios.Business;

namespace Proyectocondominos.Business
{
    public class UsuariosBusiness : IBusinessBase<UsuarioActivo>
    {
        private ServicioDB _servicioDB;

        public UsuariosBusiness(IConfiguration configuration)
        {
            _servicioDB = new ServicioDB(configuration);
        }

        public bool Crear(UsuarioActivo modelo, int usuarioId)
        {
            _servicioDB.Conectar();
            var parametros = new List<Parametros>();
            parametros.Add(new Parametros { Nombre = "usuarioNuevo", Valor = modelo.Usuario });
            parametros.Add(new Parametros { Nombre = "nombreNuevo", Valor = modelo.Nombre });
            parametros.Add(new Parametros { Nombre = "contrasenaNueva", Valor = modelo.Contrasena });
            parametros.Add(new Parametros { Nombre = "resetearLaContrasena", Valor = modelo.ResetearContrasena });
            parametros.Add(new Parametros { Nombre = "tipoDeUsuarioId", Valor = modelo.TipoUsuarioId });

            var data = _servicioDB.RetornaDatosDeProcedimiento("spRegistrarUsuario", parametros);

            string resultado = string.Empty;

            if (data.Columns.Count == 1)
            {
                foreach (DataRow row in data.Rows)
                {
                    resultado = row[0].ToString();
                }
            }
            _servicioDB.Cerrar();
            if (resultado == "Usuario creado correctamente")
            {
                return true;
            }
            else
            {
                throw new Exception(resultado);
            }
        }

        public bool Editar(UsuarioActivo modelo, int usuarioId)
        {
            _servicioDB.Conectar();
            var parametros = new List<Parametros>();
            parametros.Add(new Parametros { Nombre = "usuarioEditaId", Valor = modelo.Id });
            parametros.Add(new Parametros { Nombre = "nombreEdita", Valor = modelo.Nombre });
            parametros.Add(new Parametros { Nombre = "usuarioEdita", Valor = modelo.Usuario });
            parametros.Add(new Parametros { Nombre = "cambioContrasena", Valor = modelo.CambioDeContrasena });
            parametros.Add(new Parametros { Nombre = "usuarioContrasena", Valor = modelo.Contrasena });
            parametros.Add(new Parametros { Nombre = "reseteaContrasena", Valor = modelo.ResetearContrasena });
            parametros.Add(new Parametros { Nombre = "usuarioId", Valor = usuarioId });

            var data = _servicioDB.RetornaDatosDeProcedimiento("spEditarUsuario", parametros);
            string resultado = string.Empty;

            if (data.Columns.Count == 1)
            {
                foreach (DataRow row in data.Rows)
                {
                    resultado = row[0].ToString();
                }
            }
            _servicioDB.Cerrar();
            if (resultado == "Usuario editado exitosamente")
            {
                return true;
            }
            else
            {
                throw new Exception(resultado);
            }
        }

        public bool Eliminar(int id, int usuarioId)
        {
            _servicioDB.Conectar();
            var parametros = new List<Parametros>();
            parametros.Add(new Parametros { Nombre = "eliminaUsuarioId", Valor = id });
            parametros.Add(new Parametros { Nombre = "usuarioId", Valor = usuarioId });

            var data = _servicioDB.RetornaDatosDeProcedimiento("spEliminaUsuarios", parametros);

            string resultado = string.Empty;

            if (data.Columns.Count == 1)
            {
                foreach (DataRow row in data.Rows)
                {
                    resultado = row[0].ToString();
                }
            }
            _servicioDB.Cerrar();
            if (resultado == "Usuario eliminado exitosamente")
            {
                return true;
            }
            else
            {
                throw new Exception(resultado);
            }
        }

        public UsuarioActivo LoguearUsuario(UsuarioActivo usuario)
        {
            _servicioDB.Conectar();
            var usuarioActivo = new UsuarioActivo();
            var parametros = new List<Parametros>();
            parametros.Add(new Parametros { Nombre = "usuarioLoguea", Valor = usuario.Usuario });
            parametros.Add(new Parametros { Nombre = "contrasenaUsuario", Valor = usuario.Contrasena });

            var data = _servicioDB.RetornaDatosDeProcedimiento("spValidarUsuario", parametros);

            if (data.Columns.Count == 1)
            {
                foreach (DataRow row in data.Rows)
                {
                    throw new Exception(row[0].ToString());
                }
            }

            foreach (DataRow row in data.Rows)
            {
                usuarioActivo.Id = Convert.ToInt32(row["Id"]);
                usuarioActivo.Usuario = row["Usuario"].ToString();
                usuarioActivo.Nombre = row["Nombre"].ToString();
                usuarioActivo.TipoUsuarioId = Convert.ToInt32(row["TipoUsuarioId"]);
                usuarioActivo.ResetearContrasena = row["ResetearContrasena"] == DBNull.Value ? true : Convert.ToBoolean(row["ResetearContrasena"]);
                usuarioActivo.Tipo = row["Tipo"].ToString();
            }
            _servicioDB.Cerrar();
            return usuarioActivo;
        }

        public List<UsuarioActivo> Obtener(UsuarioActivo modelo, int usuarioId)
        {
            _servicioDB.Conectar();
            var usuarios = new List<UsuarioActivo>();
            var parametros = new List<Parametros>();
            parametros.Add(new Parametros { Nombre = "seleccionUsuarioId", Valor = modelo.Id == 0 ? null : modelo.Id });
            parametros.Add(new Parametros { Nombre = "tipoDeUsuarioId", Valor = modelo.TipoUsuarioId });
            parametros.Add(new Parametros { Nombre = "usuarioId", Valor = usuarioId });

            var data = _servicioDB.RetornaDatosDeProcedimiento("spSeleccionaUsuarios", parametros);

            if (data.Columns.Count == 1)
            {
                foreach (DataRow row in data.Rows)
                {
                    throw new Exception(row[0].ToString());
                }
            }

            foreach (DataRow row in data.Rows)
            {
                var usuario = new UsuarioActivo();

                usuario.Id = Convert.ToInt32(row["Id"]);
                usuario.Usuario = row["Usuario"].ToString();
                usuario.Nombre = row["Nombre"].ToString();
                usuario.TipoUsuarioId = Convert.ToInt32(row["TipoUsuarioId"]);
                usuario.ResetearContrasena = row["ResetearContrasena"] == DBNull.Value ? true : Convert.ToBoolean(row["ResetearContrasena"]);

                usuarios.Add(usuario);
            }
            _servicioDB.Cerrar();
            return usuarios;
        }
    }
}
