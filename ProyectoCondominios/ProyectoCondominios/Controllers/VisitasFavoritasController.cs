using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ProyectoCondominios.Business;
using ProyectoCondominios.Models;
using Proyectocondominos.Models;

namespace ProyectoCondominios.Controllers
{
    public class VisitasFavoritasController : Controller
    {
        private readonly IConfiguration _configuration;
        private UsuarioActivo _usuario;

        public VisitasFavoritasController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IActionResult Index()
        {
            try
            {
                List<Visitas> result = CargarVisitas();

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
                List<Visitas> result = CargarVisitas();

                if (string.IsNullOrEmpty(palabraClave))
                {
                    return View("Index", new ModeloBase { Datos = result, Error = new ErrorViewModel() });
                }
                palabraClave = palabraClave.ToLower();
                var buscador = result.Where(x => x.Nombre.ToLower().Contains(palabraClave) || x.Cedula.ToLower().Contains(palabraClave) ||
                    x.Vehiculo.ToLower().Contains(palabraClave)).ToList();
                return View("Index", new ModeloBase { Datos = buscador, Error = new ErrorViewModel(), Buscando = true });
            }
            catch (Exception ex)
            {
                return View(new ModeloBase { Error = new ErrorViewModel { Error = ex.Message } });
            }
        }

        public IActionResult AgregarOEditar(int? id)
        {
            ValidarUsuario();
            var visita = new Visitas();
            if (id != null)
            {
                var bus = new VisitasFavoritasBusiness(_configuration);
                visita = bus.Obtener(new Visitas { Id = id }, _usuario.Id).FirstOrDefault();
            }
            ViewBag.Usuario = _usuario;
            return View("AgregarOEditar", visita);
        }

        public IActionResult Guardar(Visitas model)
        {
            try
            {
                ValidarUsuario();
                var bus = new VisitasFavoritasBusiness(_configuration);
                if (model.Id == 0 || model.Id == null)
                {
                    bus.Crear(model, _usuario.Id);
                }
                else
                {
                    bus.Editar(model, _usuario.Id);
                }
                List<Visitas> result = CargarVisitas();
                return View("Index", new ModeloBase { Datos = result, Error = new ErrorViewModel() });
            }
            catch (Exception ex)
            {
                return View("Index", new ModeloBase { Error = new ErrorViewModel { Error = ex.Message } });
            }
        }

        public IActionResult Eliminar(int id)
        {
            try
            {
                ValidarUsuario();
                var bus = new VisitasFavoritasBusiness(_configuration);
                bus.Eliminar(id, _usuario.Id);

                List<Visitas> result = CargarVisitas();
                return View("Index", new ModeloBase { Datos = result, Error = new ErrorViewModel() });
            }
            catch (Exception ex)
            {
                return View("Index", new ModeloBase { Error = new ErrorViewModel { Error = ex.Message } });
            }
        }

        private List<Visitas> CargarVisitas()
        {
            ValidarUsuario();
            var bus = new VisitasFavoritasBusiness(_configuration);
            var result = bus.Obtener(new Visitas(), _usuario.Id);
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
