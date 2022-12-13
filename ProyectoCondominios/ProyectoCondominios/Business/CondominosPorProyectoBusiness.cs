using ProyectoCondominios.Models;
using Proyectocondominos.Models;
using Proyectocondominos.Services;
using System.Data;
using System.Reflection;

namespace ProyectoCondominios.Business
{
    public class CondominosPorProyectoBusiness : IBusinessBase<CondominosPorProyecto>
    {
        private ServicioDB _servicioDB;

        public CondominosPorProyectoBusiness(IConfiguration configuration)
        {
            _servicioDB = new ServicioDB(configuration);
        }

        public bool Crear(CondominosPorProyecto modelo, int usuarioId)
        {
            _servicioDB.Conectar();
            var parametros = new List<Parametros>();
            parametros.Add(new Parametros { Nombre = "pProyectoId", Valor = modelo.ProyectoId });
            parametros.Add(new Parametros { Nombre = "pCondominoId", Valor = modelo.CondominoId });
            parametros.Add(new Parametros { Nombre = "usuarioId", Valor = usuarioId });

            var data = _servicioDB.RetornaDatosDeProcedimiento("spAsignaCondominosAProyecto", parametros);
            string resultado = string.Empty;

            if (data.Columns.Count == 1)
            {
                foreach (DataRow row in data.Rows)
                {
                    resultado = row[0].ToString();
                }
            }
            _servicioDB.Cerrar();
            if (resultado == "Condómino asignado a proyecto exitosamente")
            {
                return true;
            }
            else
            {
                throw new Exception(resultado);
            }
        }

        public bool Editar(CondominosPorProyecto modelo, int usuarioId)
        {
            throw new NotImplementedException();
        }

        public bool Eliminar(int id, int usuarioId)
        {
            _servicioDB.Conectar();
            var parametros = new List<Parametros>();
            parametros.Add(new Parametros { Nombre = "condominoPorProyectoId", Valor = id });
            parametros.Add(new Parametros { Nombre = "usuarioId", Valor = usuarioId });

            var data = _servicioDB.RetornaDatosDeProcedimiento("spEliminaCondominosPorProyecto", parametros);
            string resultado = string.Empty;

            if (data.Columns.Count == 1)
            {
                foreach (DataRow row in data.Rows)
                {
                    resultado = row[0].ToString();
                }
            }

            if (resultado == "Condomino por Proyecto eliminado exitosamente")
            {
                return true;
            }
            else
            {
                throw new Exception(resultado);
            }
        }

        public List<CondominosPorProyecto> Obtener(CondominosPorProyecto modelo, int usuarioId)
        {
            _servicioDB.Conectar();
            var parametros = new List<Parametros>();
            var condominosPorProyecto = new List<CondominosPorProyecto>();
            parametros.Add(new Parametros { Nombre = "pProyectoId", Valor = modelo.ProyectoId });
            parametros.Add(new Parametros { Nombre = "pCondominoId", Valor = modelo.CondominoId });
            parametros.Add(new Parametros { Nombre = "usuarioId", Valor = usuarioId });

            var data = _servicioDB.RetornaDatosDeProcedimiento("spSeleccionaCondominosPorProyecto", parametros);

            if (data.Columns.Count == 1)
            {
                foreach (DataRow row in data.Rows)
                {
                    throw new Exception(row[0].ToString());
                }
            }

            foreach (DataRow row in data.Rows)
            {
                var result = new CondominosPorProyecto();

                result.Id = Convert.ToInt32(row["Id"]);
                result.ProyectoId = Convert.ToInt32(row["ProyectoId"]);
                result.Proyecto = row["Proyecto"].ToString();
                result.CondominoId = Convert.ToInt32(row["CondominoId"]);
                result.Condomino = row["Condomino"].ToString();

                condominosPorProyecto.Add(result);
            }
            _servicioDB.Cerrar();
            return condominosPorProyecto;
        }
    }
}
