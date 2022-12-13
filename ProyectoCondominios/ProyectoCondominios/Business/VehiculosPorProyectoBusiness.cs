using ProyectoCondominios.Models;
using Proyectocondominos.Models;
using Proyectocondominos.Services;
using System.Data;

namespace ProyectoCondominios.Business
{
    public class VehiculosPorProyectoBusiness : IBusinessBase<Vehiculo>

    {
        private ServicioDB _servicioDB;
        public VehiculosPorProyectoBusiness(IConfiguration configuration)
        {
            _servicioDB = new ServicioDB(configuration);
        }
        public bool Crear(Vehiculo modelo, int usuarioId)
        {
            throw new NotImplementedException();
        }

        public bool Editar(Vehiculo modelo, int usuarioId)
        {
            throw new NotImplementedException();
        }

        public bool Eliminar(int id, int usuarioId)
        {
            throw new NotImplementedException();
        }

        public List<Vehiculo> Obtener(Vehiculo modelo, int usuarioId)
        {
            _servicioDB.Conectar();
            var parametros = new List<Parametros>();
            var vehiculos = new List<Vehiculo>();
            parametros.Add(new Parametros { Nombre = "seleccionaVehiculoId", Valor = modelo.Id });
            parametros.Add(new Parametros { Nombre = "seleccionProyectoId", Valor = modelo.ProyectoId });
            parametros.Add(new Parametros { Nombre = "usuarioId", Valor = usuarioId });

            var data = _servicioDB.RetornaDatosDeProcedimiento("spSeleccionaVehiculosPorProyecto", parametros);

            if (data.Columns.Count == 1)
            {
                foreach (DataRow row in data.Rows)
                {
                    throw new Exception(row[0].ToString());
                }
            }

            foreach (DataRow row in data.Rows)
            {
                var vehiculo = new Vehiculo();

                vehiculo.Id = Convert.ToInt32(row["Id"]);
                vehiculo.Placa = row["Placa"].ToString();
                vehiculo.Marca = row["Marca"].ToString();
                vehiculo.Modelo = row["Modelo"].ToString();
                vehiculo.Color = row["Color"].ToString();

                vehiculos.Add(vehiculo);
            }
            _servicioDB.Cerrar();
            return vehiculos;
        }
    }

}
    
