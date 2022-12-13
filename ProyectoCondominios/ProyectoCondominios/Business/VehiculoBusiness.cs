using ProyectoCondominios.Models;
using Proyectocondominos.Models;
using Proyectocondominos.Services;
using System.Data;

namespace ProyectoCondominios.Business
{
    public class VehiculoBusiness : IBusinessBase<Vehiculo>
    {
        private ServicioDB _servicioDB;
        public VehiculoBusiness(IConfiguration configuration)
        {
            _servicioDB = new ServicioDB(configuration);
        }

        public bool Crear(Vehiculo modelo, int usuarioId)
        {
            _servicioDB.Conectar();
            var parametros = new List<Parametros>();
            parametros.Add(new Parametros { Nombre = "nuevoPlaca", Valor = modelo.Placa });
            parametros.Add(new Parametros { Nombre = "nuevoMarca", Valor = modelo.Marca });
            parametros.Add(new Parametros { Nombre = "nuevoModelo", Valor = modelo.Modelo });
            parametros.Add(new Parametros { Nombre = "nuevoColor", Valor = modelo.Color });
            parametros.Add(new Parametros { Nombre = "usuarioId", Valor = usuarioId });

            var data = _servicioDB.RetornaDatosDeProcedimiento("spCreaVehiculos", parametros);
            string resultado = string.Empty;

            if (data.Columns.Count == 1)
            {
                foreach (DataRow row in data.Rows)
                {
                    resultado = row[0].ToString();
                }
            }
            _servicioDB.Cerrar();

            if (resultado == "Vehiculo creado satisfactoriamente")
            {
                return true;
            }
            else
            {
                throw new Exception(resultado);
            }
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
            var vehiculos = new List<Vehiculo>();
            var parametros = new List<Parametros>();
            parametros.Add(new Parametros { Nombre = "seleccionaVehiculoId", Valor = modelo.Id });
            parametros.Add(new Parametros { Nombre = "usuarioId", Valor = usuarioId });

            var data = _servicioDB.RetornaDatosDeProcedimiento("spSeleccionaVehiculos", parametros);

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
