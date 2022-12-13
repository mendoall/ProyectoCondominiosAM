using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ProyectoCondominios.Business;
using ProyectoCondominios.Models;
using Proyectocondominos.Models;

namespace ProyectoCondominios.Controllers
{
    public class VehiculoController : Controller
    {
        private readonly IConfiguration _configuration;
        private UsuarioActivo _usuario;

        public VehiculoController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IActionResult Index()
        {
            try
            {
                List<Vehiculo> result = CargarVehiculo();

                return View(new ModeloBase { Datos = result, Error = new ErrorViewModel() });
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
                List<Vehiculo> result = CargarVehiculo();

                if (string.IsNullOrEmpty(palabraClave))
                {
                    return RedirectToAction("Index", new ModeloBase { Datos = result, Error = new ErrorViewModel() });
                }
                palabraClave = palabraClave.ToLower();
                var buscador = result.Where(x => x.Marca.ToLower().Contains(palabraClave) || x.Placa.ToLower().Contains(palabraClave) ||
                    x.Color.ToLower().Contains(palabraClave) || x.Modelo.ToLower().Contains(palabraClave)).ToList();
                return RedirectToAction("Index", new ModeloBase { Datos = buscador, Error = new ErrorViewModel(), Buscando = true });
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index", new ModeloBase { Error = new ErrorViewModel { Error = ex.Message } });
            }
        }

        public IActionResult AgregarOEditar(int? id)
        {
            ValidarUsuario();
            var vehiculo = new Vehiculo();
            if (id != null)
            {
                var bus = new VehiculoBusiness(_configuration);
                vehiculo = bus.Obtener(new Vehiculo { Id = id }, _usuario.Id).FirstOrDefault();
            }
            ViewBag.Usuario = _usuario;
            return View("AgregarOEditar", vehiculo);
        }

        public IActionResult Guardar(Vehiculo model)
        {
            try
            {
                ValidarUsuario();
                var bus = new VehiculoBusiness(_configuration);
                if (model.Id == 0 || model.Id == null)
                {
                    bus.Crear(model, _usuario.Id); 
                }
                else
                {
                    bus.Editar(model, _usuario.Id);
                }
                List<Vehiculo> result = CargarVehiculo();
                return RedirectToAction("Index", new ModeloBase { Datos = result, Error = new ErrorViewModel() });
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index", new ModeloBase { Error = new ErrorViewModel { Error = ex.Message } });
            }
        }

        private List<Vehiculo> CargarVehiculo()
        {
            ValidarUsuario();
            var bus = new VehiculoBusiness(_configuration);
            var result = bus.Obtener(new Vehiculo(), _usuario.Id);
            ViewBag.Usuario = _usuario;
            return result;
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
