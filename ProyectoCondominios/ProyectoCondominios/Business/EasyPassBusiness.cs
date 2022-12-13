using ProyectoCondominios.Models;
using Proyectocondominos.Models;
using Proyectocondominos.Services;
using System.Data;
using System.Net.Mail;

namespace ProyectoCondominios.Business
{
    public class EasyPassBusiness : IBusinessBase<EasyPass>
    {
        private ServicioDB _servicioDB;

        public EasyPassBusiness(IConfiguration configuration)
        {
            _servicioDB = new ServicioDB(configuration);
        }

        public bool Crear(EasyPass modelo, int usuarioId)
        {
            throw new NotImplementedException();
        }

        public EasyPass CrearYRetorna(int condominoId, int usuarioId)
        {
            _servicioDB.Conectar();
            var parametros = new List<Parametros>();
            parametros.Add(new Parametros { Nombre = "nCondominoId", Valor = condominoId });
            parametros.Add(new Parametros { Nombre = "usuarioId", Valor = usuarioId });

            var data = _servicioDB.RetornaDatosDeProcedimiento("spCreaEasyPass", parametros);

            if (data.Columns.Count == 1)
            {
                foreach (DataRow row in data.Rows)
                {
                    throw new Exception(row[0].ToString());
                }
            }

            var dato = new EasyPass();
            foreach (DataRow row in data.Rows)
            {
                dato.Id = Convert.ToInt32(row["Id"]);
                dato.CondominoId = Convert.ToInt32(row["CondominoId"]);
                dato.Codigo = row["Codigo"].ToString();
                dato.FechaExpiracion = Convert.ToDateTime(row["FechaExpiracion"]);
            }
            _servicioDB.Cerrar();
            return dato;
        }

        public bool Editar(EasyPass modelo, int usuarioId)
        {
            throw new NotImplementedException();
        }

        public bool Eliminar(int id, int usuarioId)
        {
            throw new NotImplementedException();
        }

        public List<EasyPass> Obtener(EasyPass modelo, int usuarioId)
        {
            _servicioDB.Conectar();
            var lista = new List<EasyPass>();
            var parametros = new List<Parametros>();
            parametros.Add(new Parametros { Nombre = "seleccionaId", Valor = modelo.CondominoId });
            parametros.Add(new Parametros { Nombre = "condominoId", Valor = modelo.CondominoId });
            parametros.Add(new Parametros { Nombre = "usuarioId", Valor = usuarioId });

            var data = _servicioDB.RetornaDatosDeProcedimiento("spSeleccionaEasyPass", parametros);

            if (data.Columns.Count == 1)
            {
                foreach (DataRow row in data.Rows)
                {
                    throw new Exception(row[0].ToString());
                }
            }

            foreach (DataRow row in data.Rows)
            {
                var dato = new EasyPass();

                dato.Id = Convert.ToInt32(row["Id"]);
                dato.CondominoId = Convert.ToInt32(row["CondominoId"]);
                dato.Codigo = row["Codigo"].ToString();
                dato.FechaExpiracion = Convert.ToDateTime( row["FechaExpiracion"]);

                lista.Add(dato);
            }
            _servicioDB.Cerrar();
            return lista;
        }
    }
}
