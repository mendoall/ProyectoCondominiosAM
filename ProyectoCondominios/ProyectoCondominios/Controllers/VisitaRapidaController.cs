using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using ProyectoCondominios.Business;
using ProyectoCondominios.Models;
using Proyectocondominos.Models;

namespace ProyectoCondominios.Controllers
{
    public class VisitaRapidaController : Controller
    {
        private readonly IConfiguration _configuration;
        private UsuarioActivo _usuario;

        public VisitaRapidaController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IActionResult Index()
        {
            try
            {
                ModeloBase model = CargarVisitasRapidas();

                return View(model);
            }
            catch (Exception ex)
            {
                return View(new ModeloBase { Error = new ErrorViewModel { Error = ex.Message } });
            }
        }

        private ModeloBase CargarVisitasRapidas()
        {
            ValidarUsuario();
            var bus = new VisitaRapidaBusiness(_configuration);
            var data = new VisitaRapida();
            var modelo = new ModeloBase();

            var tipos = bus.ObtenerTipoDeVisitaRapida(_usuario.Id);
            var lista = bus.Obtener(new VisitaRapida(), _usuario.Id);

            data.TipoDeVisitaRapida = tipos.Select(p => new SelectListItem
            {
                Text = p.Nombre.ToString(),
                Value = p.Id.ToString()
            });

            modelo.Objeto = data;
            modelo.Datos = lista;
            modelo.Error = new ErrorViewModel();

            ViewBag.Usuario = _usuario;
            return modelo;
        }

        public IActionResult Guardar(int TipoVisitaId)
        {
            if (TipoVisitaId > 0)
            {
                ValidarUsuario();
                var bus = new VisitaRapidaBusiness(_configuration);

                bus.Crear(new VisitaRapida { TipoVisitaRapidaId = TipoVisitaId }, _usuario.Id);
            }
            
            ModeloBase modeloBase = CargarVisitasRapidas();
            return View("Index", modeloBase);
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
