using ProyectoCondominios.Models;
using Proyectocondominos.Models;
using Proyectocondominos.Services;
using System.Data;

namespace ProyectoCondominios.Business
{
    public class VisitaRapidaBusiness : IBusinessBase<VisitaRapida>
    {
        private ServicioDB _servicioDB;
        public VisitaRapidaBusiness(IConfiguration configuration)
        {
            _servicioDB = new ServicioDB(configuration);
        }

        public bool Crear(VisitaRapida modelo, int usuarioId)
        {
            _servicioDB.Conectar();
            var parametros = new List<Parametros>();
            parametros.Add(new Parametros { Nombre = "tipoDeVisitaId", Valor = modelo.TipoVisitaRapidaId });
            parametros.Add(new Parametros { Nombre = "usuarioId", Valor = usuarioId });

            var data = _servicioDB.RetornaDatosDeProcedimiento("spCreaVisitaRapida", parametros);
            string resultado = string.Empty;

            if (data.Columns.Count == 1)
            {
                foreach (DataRow row in data.Rows)
                {
                    resultado = row[0].ToString();
                }
            }
            _servicioDB.Cerrar();

            if (resultado == "Visitas Rapida creada satisfactoriamente")
            {
                return true;
            }
            else
            {
                throw new Exception(resultado);
            }
        }

        public bool Editar(VisitaRapida modelo, int usuarioId)
        {
            throw new NotImplementedException();
        }

        public bool Eliminar(int id, int usuarioId)
        {
            throw new NotImplementedException();
        }

        public List<VisitaRapida> Obtener(VisitaRapida modelo, int usuarioId)
        {
            _servicioDB.Conectar();
            var visitas = new List<VisitaRapida>();
            var parametros = new List<Parametros>();
            parametros.Add(new Parametros { Nombre = "seleccionVisitaId", Valor = modelo.Id });
            parametros.Add(new Parametros { Nombre = "usuarioId", Valor = usuarioId });

            var data = _servicioDB.RetornaDatosDeProcedimiento("spSeleccionaVisitaRapida", parametros);

            if (data.Columns.Count == 1)
            {
                foreach (DataRow row in data.Rows)
                {
                    throw new Exception(row[0].ToString());
                }
            }

            foreach (DataRow row in data.Rows)
            {
                var visita = new VisitaRapida();

                visita.Id = Convert.ToInt32(row["Id"]);
                visita.Nombre = row["Nombre"].ToString();
                visita.FechaVisita = Convert.ToDateTime(row["FechaVisita"]);

                visitas.Add(visita);
            }
            _servicioDB.Cerrar();
            return visitas;
        }

        public List<TipoVisitaRapida> ObtenerTipoDeVisitaRapida(int usuarioId)
        {
            _servicioDB.Conectar();
            var visitas = new List<TipoVisitaRapida>();
            var parametros = new List<Parametros>();
            parametros.Add(new Parametros { Nombre = "usuarioId", Valor = usuarioId });

            var data = _servicioDB.RetornaDatosDeProcedimiento("spSeleccionaTipoVisitaRapida", parametros);

            if (data.Columns.Count == 1)
            {
                foreach (DataRow row in data.Rows)
                {
                    throw new Exception(row[0].ToString());
                }
            }

            foreach (DataRow row in data.Rows)
            {
                var visita = new TipoVisitaRapida();

                visita.Id = Convert.ToInt32(row["Id"]);
                visita.Nombre = row["Nombre"].ToString();

                visitas.Add(visita);
            }
            _servicioDB.Cerrar();
            return visitas;
        }
    }
}
