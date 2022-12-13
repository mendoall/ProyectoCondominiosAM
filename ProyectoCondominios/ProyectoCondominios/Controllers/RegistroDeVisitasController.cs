using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ProyectoCondominios.Business;
using ProyectoCondominios.Models;
using Proyectocondominos.Models;
using Proyectocondominos.Services;

namespace ProyectoCondominios.Controllers
{
    public class RegistroDeVisitasController : Controller, IMetodosBase<Visitas>
    {
        public UsuarioActivo _usuario { get; set; }
        public IConfiguration _configuration { get;}

        public RegistroDeVisitasController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IActionResult Index()
        {
            try
            {
                var result = ObtenerDatos();

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
                var result = ObtenerDatos();

                if (string.IsNullOrEmpty(palabraClave))
                {
                    return View("Index", result);
                }
                    
                var buscador = result.Where(x => x.Nombre.ToLower().Contains(palabraClave) || x.Cedula.ToLower().Contains(palabraClave) ||
                    x.Vehiculo.ToLower().Contains(palabraClave)).ToList();
                return View("Index", new ModeloBase { Datos = buscador, Error = new ErrorViewModel() });
            }
            catch (Exception ex)
            {
                return View("Index", new ModeloBase { Error = new ErrorViewModel { Error = ex.Message } });
            }
        }

        public void ValidarUsuario()
        {
            var sessionValue = HttpContext.Session.GetString("Usuario");

            if (string.IsNullOrEmpty(sessionValue))
            {
                throw new Exception("Por favor inicie sesión");
            }

            _usuario = JsonConvert.DeserializeObject<UsuarioActivo>(sessionValue);
        }

        public List<Visitas> ObtenerDatos()
        {
            ValidarUsuario();
            var  bus = new RegistroDeVisitasBusiness(_configuration);
            var lista = bus.Obtener(new Visitas { ProyectoId = _usuario.ProyectoId }, _usuario.Id);
            ViewBag.Usuario = _usuario;
            return lista;
        }
    }
}
