using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ProyectoCondominios.Business;
using ProyectoCondominios.Models;
using ProyectoCondominios.Utilitarios;
using Proyectocondominos.Models;

namespace ProyectoCondominios.Controllers
{
    public class OficialDeSeguridadController : Controller
    {
        private readonly IConfiguration _configuration;
        private UsuarioActivo _usuario;

        public OficialDeSeguridadController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IActionResult Index()
        {            
            try
            {
                List<OficialDeSeguridad> result = ObtenerOficiales();

                return View(new ModeloBase { Datos = result, Error = new ErrorViewModel() });
            }
            catch (Exception ex)
            {
                return View(new ModeloBase { Error = new ErrorViewModel { Error = ex.Message } });
            }
        }

        public IActionResult AgregarOEditar(int? id)
        {
            ValidarUsuario();
            var oficial = new OficialDeSeguridad();
            if (id != null)
            {
                var bus = new OficialDeSeguridadBusiness(_configuration);
                oficial = bus.Obtener(new OficialDeSeguridad { Id = (int)id, TipoUsuarioId = 3 }, _usuario.Id).FirstOrDefault();
            }
            ViewBag.Usuario = _usuario;
            return View("AgregarOEditar", oficial);
        }

        public IActionResult Guardar(OficialDeSeguridad model)
        {
            try
            {
                ValidarUsuario();                
                var bus = new OficialDeSeguridadBusiness(_configuration);
                if (model.Id == 0)
                {
                    model.TipoUsuarioId = 3;
                    model.ResetearContrasena = true;
                    model.Contrasena = Constantes.ContraseñaDefecto;
                    bus.Crear(model, _usuario.Id);
                }
                else
                {
                    bus.Editar(model, _usuario.Id);
                }
                List<OficialDeSeguridad> result = ObtenerOficiales();
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
                var bus = new OficialDeSeguridadBusiness(_configuration);
                bus.Eliminar(id, _usuario.Id);

                List<OficialDeSeguridad> result = ObtenerOficiales();
                return View("Index", new ModeloBase { Datos = result, Error = new ErrorViewModel() });
            }
            catch (Exception ex)
            {
                return View("Index", new ModeloBase { Error = new ErrorViewModel { Error = ex.Message } });
            }
        }

        private List<OficialDeSeguridad> ObtenerOficiales()
        {
            ValidarUsuario();
            var bus = new OficialDeSeguridadBusiness(_configuration);
            var result = bus.Obtener(new OficialDeSeguridad { TipoUsuarioId = 3 }, _usuario.Id);
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
