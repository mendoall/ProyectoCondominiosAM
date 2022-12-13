using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using ProyectoCondominios.Models;
using ProyectoCondominios.Utilitarios;
using Proyectocondominos.Business;
using Proyectocondominos.Models;

namespace Proyectocondominos.Controllers
{
    public class ProyectosHabitacionalesController : Controller
    {
        private readonly IConfiguration _configuration;
        private UsuarioActivo _usuario;

        public ProyectosHabitacionalesController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IActionResult Index()
        {
            try
            {
                List<ProyectosHabitacionales> result = CargarProyectos();

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
                var result = CargarProyectos();

                if (string.IsNullOrEmpty(palabraClave))
                {
                    return View("Index", new ModeloBase { Datos = result, Error = new ErrorViewModel() });
                }
                palabraClave = palabraClave.ToLower();
                var buscador = result.Where(x => x.Nombre.ToLower().Contains(palabraClave) || x.Codigo.ToLower().Contains(palabraClave) ||
                    x.Direccion.ToLower().Contains(palabraClave) || x.TelefonoOficina.ToLower().Contains(palabraClave)).ToList();
                return View("Index", new ModeloBase { Datos = buscador, Error = new ErrorViewModel(), Buscando = true  });
            }
            catch (Exception ex)
            {
                return View(new ModeloBase { Error = new ErrorViewModel { Error = ex.Message } });
            }
        }

        public IActionResult AgregarOEditar(int? id)
        {
            ValidarUsuario();
            var proyecto = new ProyectosHabitacionales();
            if (id != null)
            {
                var bus = new ProyectosHabitacionalesBusiness(_configuration);
                proyecto = bus.ObtenerProyectosHabitaciones(id, _usuario.Id).FirstOrDefault();
            }
            ViewBag.Usuario = _usuario;
            return View("AgregarOEditar", proyecto);
        }

        public IActionResult Guardar(ProyectosHabitacionales model)
        {
            try
            {
                ValidarUsuario();
                var bus = new ProyectosHabitacionalesBusiness(_configuration);

                //Convierte la imagen
                if (model.Archivo != null)
                {
                    var convertidor = new TransformadorDeImagenes();
                    model.Logo = convertidor.ConvertirImagenABlob(model.Archivo);
                }

                if (model.Id == 0)
                {
                    bus.CrearProyectosHabitacionales(model, _usuario.Id);
                }
                else
                {
                    bus.EditarProyectosHabitacionales(model, _usuario.Id);
                }
                List<ProyectosHabitacionales> result = CargarProyectos();
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
                var bus = new ProyectosHabitacionalesBusiness(_configuration);
                bus.EliminarProyectosHabitacionales(id, _usuario.Id);
               
                List<ProyectosHabitacionales> result = CargarProyectos();
                return View("Index", new ModeloBase { Datos = result, Error = new ErrorViewModel() });
            }
            catch (Exception ex)
            {
                return View("Index", new ModeloBase { Error = new ErrorViewModel { Error = ex.Message } });
            }
        }

        private List<ProyectosHabitacionales> CargarProyectos()
        {
            ValidarUsuario();
            var bus = new ProyectosHabitacionalesBusiness(_configuration);
            var result = bus.ObtenerProyectosHabitaciones(null, _usuario.Id);
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
