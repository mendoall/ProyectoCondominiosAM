using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ProyectoCondominios.Business;
using ProyectoCondominios.Models;
using Proyectocondominos.Models;

namespace ProyectoCondominios.Controllers
{
    public class VehiculosPorProyectoController : Controller
    {
        private readonly IConfiguration _configuration;
        private UsuarioActivo _usuario;

        public VehiculosPorProyectoController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IActionResult Index()
        {
            try
            {
                var datos = ObtenerInformacion();
                return View(new ModeloBase { Datos = datos, Error = new ErrorViewModel() });
            }
            catch (Exception ex)
            {
                return View(new ModeloBase { Error = new ErrorViewModel { Error = ex.Message } });
            }
        }

        public IActionResult Buscar(string palabraClave)
        {
            try
            {
                var datos = ObtenerInformacion();
                if (string.IsNullOrEmpty(palabraClave))
                {
                    return View("Index", new ModeloBase { Datos = datos, Error = new ErrorViewModel() });
                }
                palabraClave = palabraClave.ToLower();
                var buscador = datos.Where(x => x.Placa.ToLower().Contains(palabraClave)).ToList();
                return View("Index", new ModeloBase { Datos = buscador, Error = new ErrorViewModel(), Buscando = true });
            }
            catch (Exception ex)
            {
                return View(new ModeloBase { Error = new ErrorViewModel { Error = ex.Message } });
            }
        }

        private List<Vehiculo> ObtenerInformacion()
        {
            ValidarUsuario();
            var bus = new VehiculosPorProyectoBusiness(_configuration);
            List<Vehiculo> vehiculos = bus.Obtener(new Vehiculo { Id = null, ProyectoId = _usuario.ProyectoId }, _usuario.Id);
            ViewBag.Usuario = _usuario;
            return vehiculos;
        }

        private void ValidarUsuario()
        {
            var sessionValue = HttpContext.Session.GetString("Usuario");

            if (string.IsNullOrEmpty(sessionValue))
            {
                throw new Exception("Por favor inicie sesión");
            }

            _usuario = JsonConvert.DeserializeObject<UsuarioActivo>(sessionValue);
        }
    }
}
