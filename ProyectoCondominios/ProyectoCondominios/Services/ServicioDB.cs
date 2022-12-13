using MySql.Data.MySqlClient;
using Proyectocondominos.Models;
using System.Data;

namespace Proyectocondominos.Services
{
    public class ServicioDB
    {
        private readonly IConfiguration _configuration;
        private MySqlConnection _conn;

        public ServicioDB(IConfiguration configuration)
        {
            _configuration = configuration;
            _conn = new MySqlConnection();
        }

        public void Conectar()
        {
            string constr = _configuration.GetConnectionString("Default");
            _conn.ConnectionString = constr;
            _conn.Open();
        }

        public void Cerrar()
        {
            _conn.Close();
        }

        public DataTable RetornaDatosDeProcedimiento(string storeProcedure, List<Parametros> parametros)
        {
            DataTable dt = new DataTable();
            using (MySqlCommand cmd = new MySqlCommand(storeProcedure, _conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                foreach (var item in parametros)
                {
                    cmd.Parameters.AddWithValue(item.Nombre, item.Valor);
                }
                
                using (MySqlDataAdapter sda = new MySqlDataAdapter(cmd))
                {                    
                    sda.Fill(dt);
                }
            }

            return dt;
        }

    }
}
