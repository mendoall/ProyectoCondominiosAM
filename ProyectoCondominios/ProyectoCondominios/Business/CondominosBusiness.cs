using ProyectoCondominios.Models;
using Proyectocondominos.Services;
using System.Data;
using Proyectocondominos.Models;

namespace ProyectoCondominios.Business
{
    public class CondominosBusiness : IBusinessBase<Condominos>
    {
        private ServicioDB _servicioDB;
        private string _emailMessage;

        public CondominosBusiness(IConfiguration configuration)
        {
            _servicioDB = new ServicioDB(configuration);
        }

        public List<Condominos> Obtener(Condominos modelo, int usuarioId)
        {
            _servicioDB.Conectar();
            var condominos = new List<Condominos>();
            var parametros = new List<Parametros>();
            parametros.Add(new Parametros { Nombre = "condominoId", Valor = modelo.Id });
            parametros.Add(new Parametros { Nombre = "usuarioId", Valor = usuarioId });

            var data = _servicioDB.RetornaDatosDeProcedimiento("spSeleccionaCondominos", parametros);

            if (data.Columns.Count == 1)
            {
                foreach (DataRow row in data.Rows)
                {
                    throw new Exception(row[0].ToString());
                }
            }

            foreach (DataRow row in data.Rows)
            {
                var condomino = new Condominos();

                condomino.Id = Convert.ToInt32(row["Id"]);
                condomino.Cedula = row["Cedula"].ToString();
                condomino.Nombre = row["Nombre"].ToString();
                condomino.Telefonos = row["Telefonos"].ToString();
                condomino.Foto =  row["Foto"] == DBNull.Value ? null : (byte[])row["Foto"];
                condomino.Email = row["Email"].ToString();

                condominos.Add(condomino);
            }
            _servicioDB.Cerrar();
            return condominos;
        }

        public List<Condominos> ObtenerCondominosPorUsuario(int usuarioId)
        {
            _servicioDB.Conectar();
            var condominos = new List<Condominos>();
            var parametros = new List<Parametros>();
            parametros.Add(new Parametros { Nombre = "usuarioId", Valor = usuarioId });

            var data = _servicioDB.RetornaDatosDeProcedimiento("spSeleccionaCondominosPorUsuario", parametros);

            if (data.Columns.Count == 1)
            {
                foreach (DataRow row in data.Rows)
                {
                    throw new Exception(row[0].ToString());
                }
            }

            foreach (DataRow row in data.Rows)
            {
                var condomino = new Condominos();

                condomino.Id = Convert.ToInt32(row["Id"]);
                condomino.Cedula = row["Cedula"].ToString();
                condomino.Nombre = row["Nombre"].ToString();
                condomino.Telefonos = row["Telefonos"].ToString();
                condomino.Foto = row["Foto"] == DBNull.Value ? null : (byte[])row["Foto"];
                condomino.Email = row["Email"].ToString();

                condominos.Add(condomino);
            }
            _servicioDB.Cerrar();
            return condominos;
        }

        public bool Crear(Condominos modelo, int usuarioId)
        {
            _servicioDB.Conectar();
            var parametros = new List<Parametros>();
            parametros.Add(new Parametros { Nombre = "cedula", Valor = modelo.Cedula });
            parametros.Add(new Parametros { Nombre = "nombre", Valor = modelo.Nombre });
            parametros.Add(new Parametros { Nombre = "telefonos", Valor = modelo.Telefonos });
            parametros.Add(new Parametros { Nombre = "email", Valor = modelo.Email });
            parametros.Add(new Parametros { Nombre = "foto", Valor = modelo.Foto });
            parametros.Add(new Parametros { Nombre = "usuarioId", Valor = usuarioId });

            var data = _servicioDB.RetornaDatosDeProcedimiento("spCreaCondominos", parametros);
            string resultado = string.Empty;

            if (data.Columns.Count == 1)
            {
                foreach (DataRow row in data.Rows)
                {
                    resultado = row[0].ToString();
                }
            }
            _servicioDB.Cerrar();

            if (resultado == "Condómino creado exitosamente")
            {
                return true;
            }

            if (resultado.Contains("Condómino creado exitosamente"))
            {
                _emailMessage = resultado;
                return true;
            }
            else
            {
                throw new Exception(resultado);
            }
        }

        public string CrearObteniendoMensaje(Condominos modelo, int usuarioId)
        {
            Crear(modelo, usuarioId);

            return _emailMessage;
        }
        public bool Editar(Condominos modelo, int usuarioId)
        {
            _servicioDB.Conectar();
            var parametros = new List<Parametros>();
            parametros.Add(new Parametros { Nombre = "condominoId", Valor = modelo.Id });
            parametros.Add(new Parametros { Nombre = "cedula", Valor = modelo.Cedula });
            parametros.Add(new Parametros { Nombre = "nombre", Valor = modelo.Nombre });
            parametros.Add(new Parametros { Nombre = "telefonos", Valor = modelo.Telefonos });
            parametros.Add(new Parametros { Nombre = "email", Valor = modelo.Email });
            parametros.Add(new Parametros { Nombre = "foto", Valor = modelo.Foto });
            parametros.Add(new Parametros { Nombre = "usuarioId", Valor = usuarioId });

            var data = _servicioDB.RetornaDatosDeProcedimiento("spEditarCondominos", parametros);
            string resultado = string.Empty;

            if (data.Columns.Count == 1)
            {
                foreach (DataRow row in data.Rows)
                {
                    resultado = row[0].ToString();
                }
            }
            _servicioDB.Cerrar();
            if (resultado == "Condómino editado exitosamente")
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
            parametros.Add(new Parametros { Nombre = "condominoId", Valor = id });
            parametros.Add(new Parametros { Nombre = "usuarioId", Valor = usuarioId });

            var data = _servicioDB.RetornaDatosDeProcedimiento("spEliminaCondominos", parametros);
            string resultado = string.Empty;

            if (data.Columns.Count == 1)
            {
                foreach (DataRow row in data.Rows)
                {
                    resultado = row[0].ToString();
                }
            }
            _servicioDB.Cerrar();
            if (resultado == "Condómino eliminado exitosamente")
            {
                return true;
            }
            else
            {
                throw new Exception(resultado);
            }
        }
    }
}
