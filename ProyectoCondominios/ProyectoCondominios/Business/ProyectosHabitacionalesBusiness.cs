using Proyectocondominos.Models;
using Proyectocondominos.Services;
using System.Data;
using System.Reflection.Metadata;

namespace Proyectocondominos.Business
{
    public class ProyectosHabitacionalesBusiness
    {
        private ServicioDB _servicioDB;

        public ProyectosHabitacionalesBusiness(IConfiguration configuration)
        {
            _servicioDB = new ServicioDB(configuration);
        }

        public List<ProyectosHabitacionales> ObtenerProyectosHabitaciones(int? id, int usuarioId)
        {
            _servicioDB.Conectar();
            var proyectos = new List<ProyectosHabitacionales>();
            var parametros = new List<Parametros>();
            parametros.Add(new Parametros { Nombre = "proyectoId", Valor = id });
            parametros.Add(new Parametros { Nombre = "usuarioId", Valor = usuarioId });

            var data = _servicioDB.RetornaDatosDeProcedimiento("spSeleccionaProyectos", parametros);

            if (data.Columns.Count == 1)
            {
                foreach (DataRow row in data.Rows)
                {
                    throw new Exception(row[0].ToString());
                }
            }

            foreach (DataRow row in data.Rows)
            {
                var proyecto = new ProyectosHabitacionales();

                proyecto.Id = Convert.ToInt32(row["Id"]);
                proyecto.Codigo = row["Codigo"].ToString();
                proyecto.Nombre = row["Nombre"].ToString();
                proyecto.Direccion = row["Direccion"].ToString();
                proyecto.Logo = row["Logo"] == DBNull.Value ? null : (byte[])row["Logo"];
                proyecto.TelefonoOficina = row["TelefonoOficina"].ToString();

                proyectos.Add(proyecto);
            }
            _servicioDB.Cerrar();
            return proyectos;
        }

        public bool CrearProyectosHabitacionales(ProyectosHabitacionales proyecto, int usuarioId)
        {
            _servicioDB.Conectar();
            var parametros = new List<Parametros>();
            parametros.Add(new Parametros { Nombre = "logo", Valor = proyecto.Logo });
            parametros.Add(new Parametros { Nombre = "codigo", Valor = proyecto.Codigo });
            parametros.Add(new Parametros { Nombre = "nombre", Valor = proyecto.Nombre });
            parametros.Add(new Parametros { Nombre = "direccion", Valor = proyecto.Direccion });
            parametros.Add(new Parametros { Nombre = "telefonoOficina", Valor = proyecto.TelefonoOficina });
            parametros.Add(new Parametros { Nombre = "usuarioId", Valor = usuarioId });

            var data = _servicioDB.RetornaDatosDeProcedimiento("spCreaProyectosHabitacionales", parametros);
            string resultado = string.Empty;

            if (data.Columns.Count == 1)
            {
                foreach (DataRow row in data.Rows)
                {
                    resultado = row[0].ToString();
                }
            }
            _servicioDB.Cerrar();
            if (resultado == "Proyecto creado exitosamente")
            {
                return true;
            }
            else
            {
                throw new Exception(resultado);
            }
        }

        public bool EditarProyectosHabitacionales(ProyectosHabitacionales proyecto, int usuarioId)
        {
            _servicioDB.Conectar();
            var parametros = new List<Parametros>();
            parametros.Add(new Parametros { Nombre = "proyectoId", Valor = proyecto.Id });
            parametros.Add(new Parametros { Nombre = "logo", Valor = proyecto.Logo });
            parametros.Add(new Parametros { Nombre = "codigo", Valor = proyecto.Codigo });
            parametros.Add(new Parametros { Nombre = "nombre", Valor = proyecto.Nombre });
            parametros.Add(new Parametros { Nombre = "direccion", Valor = proyecto.Direccion });
            parametros.Add(new Parametros { Nombre = "telefonoOficina", Valor = proyecto.TelefonoOficina });
            parametros.Add(new Parametros { Nombre = "usuarioId", Valor = usuarioId });

            var data = _servicioDB.RetornaDatosDeProcedimiento("spEditarProyectosHabitacionales", parametros);
            string resultado = string.Empty;

            if (data.Columns.Count == 1)
            {
                foreach (DataRow row in data.Rows)
                {
                    resultado = row[0].ToString();
                }
            }
            _servicioDB.Cerrar();
            if (resultado == "Proyecto editado exitosamente")
            {
                return true;
            }
            else
            {
                throw new Exception(resultado);
            }
        }

        public bool EliminarProyectosHabitacionales(int id, int usuarioId)
        {
            _servicioDB.Conectar();
            var parametros = new List<Parametros>();
            parametros.Add(new Parametros { Nombre = "proyectoId", Valor = id });
            parametros.Add(new Parametros { Nombre = "usuarioId", Valor = usuarioId });

            var data = _servicioDB.RetornaDatosDeProcedimiento("spEliminaProyectosHabitacionales", parametros);
            string resultado = string.Empty;

            if (data.Columns.Count == 1)
            {
                foreach (DataRow row in data.Rows)
                {
                    resultado = row[0].ToString();
                }
            }
            _servicioDB.Cerrar();
            if (resultado == "Proyecto eliminado exitosamente")
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
