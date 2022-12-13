using ProyectoCondominios.Models;
using Proyectocondominos.Models;
using Proyectocondominos.Services;
using System.Data;

namespace ProyectoCondominios.Business
{
    public class RegistroDeVisitasBusiness : IBusinessBase<Visitas>
    {
        private ServicioDB _servicioDB;

        public RegistroDeVisitasBusiness(IConfiguration configuration)
        {
            _servicioDB = new ServicioDB(configuration);
        }

        public bool Crear(Visitas modelo, int usuarioId)
        {
            throw new NotImplementedException();
        }

        public bool Editar(Visitas modelo, int usuarioId)
        {
            throw new NotImplementedException();
        }

        public bool Eliminar(int id, int usuarioId)
        {
            throw new NotImplementedException();
        }

        public List<Visitas> Obtener(Visitas modelo, int usuarioId)
        {
            _servicioDB.Conectar();
            var visitas = new List<Visitas>();
            var parametros = new List<Parametros>();
            parametros.Add(new Parametros { Nombre = "seleccionVisitaId", Valor = modelo.Id });
            parametros.Add(new Parametros { Nombre = "seleccionProyectoId", Valor = modelo.ProyectoId });             
            parametros.Add(new Parametros { Nombre = "usuarioId", Valor = usuarioId });

            var data = _servicioDB.RetornaDatosDeProcedimiento("spSeleccionaVisitasPorProyecto", parametros);

            if (data.Columns.Count == 1)
            {
                foreach (DataRow row in data.Rows)
                {
                    throw new Exception(row[0].ToString());
                }
            }

            foreach (DataRow row in data.Rows)
            {
                var visita = new Visitas();

                visita.Id = Convert.ToInt32(row["Id"]);
                visita.Cedula = row["Cedula"].ToString();
                visita.Nombre = row["Nombre"].ToString();
                visita.Vehiculo = row["Vehiculo"].ToString();
                visita.FechaVisita = row["FechaVisita"] == DBNull.Value ? DateTime.Now : Convert.ToDateTime(row["FechaVisita"]);

                visitas.Add(visita);
            }
            _servicioDB.Cerrar();
            return visitas;
        }
    }
}