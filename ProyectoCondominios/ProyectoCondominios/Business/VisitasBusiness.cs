using ProyectoCondominios.Models;
using Proyectocondominos.Models;
using Proyectocondominos.Services;
using System.Data;
using System.Net.Mail;

namespace ProyectoCondominios.Business
{
    public class VisitasBusiness : IBusinessBase<Visitas>
    {
        private ServicioDB _servicioDB;
        public VisitasBusiness(IConfiguration configuration)
        {
            _servicioDB = new ServicioDB(configuration);
        }

        public bool Crear(Visitas modelo, int usuarioId)
        {
            _servicioDB.Conectar();
            var parametros = new List<Parametros>();
            parametros.Add(new Parametros { Nombre = "cedulaNueva", Valor = modelo.Cedula });
            parametros.Add(new Parametros { Nombre = "nombreNuevo", Valor = modelo.Nombre });
            parametros.Add(new Parametros { Nombre = "vehiculoNuevo", Valor = modelo.Vehiculo });
            parametros.Add(new Parametros { Nombre = "fechaVisitaNuevo", Valor = modelo.FechaVisita });
            parametros.Add(new Parametros { Nombre = "usuarioId", Valor = usuarioId });

            var data = _servicioDB.RetornaDatosDeProcedimiento("spCreaVisita", parametros);
            string resultado = string.Empty;

            if (data.Columns.Count == 1)
            {
                foreach (DataRow row in data.Rows)
                {
                    resultado = row[0].ToString();
                }
            }
            _servicioDB.Cerrar();

            if (resultado == "Visita creada satisfactoriamente")
            {
                return true;
            }
            else
            {
                throw new Exception(resultado);
            }
        }

        public bool Editar(Visitas modelo, int usuarioId)
        {
            _servicioDB.Conectar();
            var parametros = new List<Parametros>();
            parametros.Add(new Parametros { Nombre = "visitaEditaId", Valor = modelo.Id });
            parametros.Add(new Parametros { Nombre = "cedulaEdita", Valor = modelo.Cedula });
            parametros.Add(new Parametros { Nombre = "nombreEdita", Valor = modelo.Nombre });
            parametros.Add(new Parametros { Nombre = "vehiculoEdita", Valor = modelo.Vehiculo });
            parametros.Add(new Parametros { Nombre = "fechaVisitaEdita", Valor = modelo.FechaVisita });
            parametros.Add(new Parametros { Nombre = "usuarioId", Valor = usuarioId });

            var data = _servicioDB.RetornaDatosDeProcedimiento("spEditarVisita", parametros);
            string resultado = string.Empty;

            if (data.Columns.Count == 1)
            {
                foreach (DataRow row in data.Rows)
                {
                    resultado = row[0].ToString();
                }
            }
            _servicioDB.Cerrar();
            if (resultado == "Visita editada exitosamente")
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
            parametros.Add(new Parametros { Nombre = "eliminaVisitaId", Valor = id });
            parametros.Add(new Parametros { Nombre = "usuarioId", Valor = usuarioId });

            var data = _servicioDB.RetornaDatosDeProcedimiento("spEliminaVisitas", parametros);
            string resultado = string.Empty;

            if (data.Columns.Count == 1)
            {
                foreach (DataRow row in data.Rows)
                {
                    resultado = row[0].ToString();
                }
            }
            _servicioDB.Cerrar();
            if (resultado == "Visita eliminada exitosamente")
            {
                return true;
            }
            else
            {
                throw new Exception(resultado);
            }
        }

        public List<Visitas> Obtener(Visitas modelo, int usuarioId)
        {
            _servicioDB.Conectar();
            var visitas = new List<Visitas>();
            var parametros = new List<Parametros>();
            parametros.Add(new Parametros { Nombre = "seleccionVisitaId", Valor = modelo.Id });
            parametros.Add(new Parametros { Nombre = "usuarioId", Valor = usuarioId });

            var data = _servicioDB.RetornaDatosDeProcedimiento("spSeleccionaVisitas", parametros);

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
