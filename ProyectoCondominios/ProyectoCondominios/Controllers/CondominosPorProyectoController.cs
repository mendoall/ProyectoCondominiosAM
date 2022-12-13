using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Newtonsoft.Json;
using ProyectoCondominios.Business;
using ProyectoCondominios.Models;
using Proyectocondominos.Business;
using Proyectocondominos.Models;
using System.Collections.Generic;
using System.Linq;

namespace ProyectoCondominios.Controllers
{
    public class CondominosPorProyectoController : Controller
    {
        private readonly IConfiguration _configuration;
        private UsuarioActivo _usuario;

        public CondominosPorProyectoController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IActionResult Index()
        {
            ValidarUsuario();
            ModeloBase modeloBase = ObtenerCondominosPorProyecto();
            return View(modeloBase);
        }

        private ModeloBase ObtenerCondominosPorProyecto()
        {
            ModeloBase modeloBase = new ModeloBase();
            CondominosPorProyecto data = new CondominosPorProyecto();
            var condominosbus = new CondominosBusiness(_configuration);
            var proyectosbus = new ProyectosHabitacionalesBusiness(_configuration);
            var condominosPorProyecto = new CondominosPorProyectoBusiness(_configuration);

            var proyectos = proyectosbus.ObtenerProyectosHabitaciones(null, _usuario.Id);
            var condominos = condominosbus.Obtener(new Condominos(), _usuario.Id);
            var lista = condominosPorProyecto.Obtener(new CondominosPorProyecto(), _usuario.Id);

            data.Proyectos = proyectos.Select(p => new SelectListItem
            {
                Text = p.Nombre.ToString(),
                Value = p.Id.ToString()
            });
            data.Condominos = condominos.Select(p => new SelectListItem
            {
                Text = p.Nombre.ToString(),
                Value = p.Id.ToString()
            });

            modeloBase.Error = new ErrorViewModel();
            modeloBase.Datos = lista;
            modeloBase.Objeto = data;
            ViewBag.Usuario = _usuario;
            return modeloBase;
        }

        public IActionResult Guardar(int CondominoId, int ProyectoId)
        {
            ValidarUsuario();
            var bus = new CondominosPorProyectoBusiness(_configuration);
            var resultado = bus.Obtener(new CondominosPorProyecto { CondominoId = CondominoId, ProyectoId = ProyectoId }, _usuario.Id);

            if (resultado != null && resultado.Any())
            {
                ModeloBase modeloBase = ObtenerCondominosPorProyecto();
                modeloBase.Error = new ErrorViewModel
                {
                    Error = String.Format("El Condómino {0} ya esta registrado al Proyecto {1}", resultado.FirstOrDefault()?.Condomino, resultado.FirstOrDefault()?.Proyecto)
                };
                return View("Index", modeloBase);
            }
            else
            {
                bus.Crear(new CondominosPorProyecto { CondominoId = CondominoId, ProyectoId = ProyectoId }, _usuario.Id);
                ModeloBase modeloBase = ObtenerCondominosPorProyecto();
                return View("Index", modeloBase);
            }
        }

        public IActionResult Eliminar(int id)
        {
            try
            {
                ValidarUsuario();
                var bus = new CondominosPorProyectoBusiness(_configuration);
                bus.Eliminar(id, _usuario.Id);

                ModeloBase modeloBase = ObtenerCondominosPorProyecto();
                return View("Index", modeloBase);
            }
            catch (Exception ex)
            {
                ModeloBase modeloBase = ObtenerCondominosPorProyecto();
                modeloBase.Error = new ErrorViewModel { Error = ex.Message };
                return View("Index", modeloBase);
            }
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
