using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MySqlX.XDevAPI.Common;
using Newtonsoft.Json;
using ProyectoCondominios.Business;
using ProyectoCondominios.Models;
using Proyectocondominos.Business;
using Proyectocondominos.Models;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;

namespace Proyectocondominos.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IConfiguration _configuration;

        public HomeController(ILogger<HomeController> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        public IActionResult Index()
        {
            ViewBag.EsconderHeaderFooter = true;
            return View(new ErrorViewModel());
        }

        [HttpPost]
        public ActionResult Login(string usuario, string contrasena)
        {
            try
            {
                UsuarioActivo usuarioActivo = new UsuarioActivo
                {
                    Usuario = usuario,
                    Contrasena = contrasena
                };

                var bus = new UsuariosBusiness(_configuration);
                var result = bus.LoguearUsuario(usuarioActivo);

                if (result.ResetearContrasena)
                {
                    return View("ResetearContrasena", new ModeloBase { Objeto = result });
                }

                HttpContext.Session.SetString("Usuario", JsonConvert.SerializeObject(result));

                switch (result.Tipo)
                {
                    case "Administrador":
                        return RedirectToAction("Index", "ProyectosHabitacionales");
                    case "Condomino":
                        Condominos condo = seleccionaCondominos();

                        if (condo.ListaCondominos.Count() == 1)
                        {
                            result.CondominoId = Convert.ToInt32(condo.ListaCondominos.FirstOrDefault().Value);                            
                            HttpContext.Session.SetString("Usuario", JsonConvert.SerializeObject(result));
                            return RedirectToAction("Index", "Visitas");
                        }

                        return View("SeleccionarCondominos", new ModeloBase { Objeto = condo });
                    case "Oficial de seguridad":
                        ProyectosHabitacionales data = seleccionaProyectos();
                        return View("SeleccionarProyectoHabitacional", new ModeloBase { Objeto = data });
                    default:
                        return View("Index", new ErrorViewModel { Error = "Usuario Inválido" });
                }
            }
            catch (Exception ex)
            {
                return View("Index", new ErrorViewModel { Error = ex.Message });
            }
        }

        public IActionResult ResetearContraseña(UsuarioActivo usuario)
        {
            if(usuario.Contrasena != usuario.ConfirmarContrasena)
            {
                return View("ResetearContrasena", new ModeloBase { Objeto = usuario,
                    Error = new ErrorViewModel { Error = "Las contraseñas no coinciden" } });
            }
                        
            var bus = new UsuariosBusiness(_configuration);
            usuario.CambioDeContrasena = true;
            bus.Editar(usuario, usuario.Id);                
            
            return View("Index", new ErrorViewModel());
        }

        public IActionResult LogOut()
        {
            HttpContext.Session.Remove("Usuario");
            return View("Index", new ErrorViewModel());
        }

        public IActionResult SeleccionarCondominos(int? CondominoId)
        {
            if (CondominoId == null)
            {
                Condominos data = seleccionaCondominos();
                return View("SeleccionarCondominos", new ModeloBase
                {
                    Objeto = data,
                    Error = new ErrorViewModel { Error = "Debe seleccionar un proyecto." }
                });
            }
            var usuario = ValidarUsuario();
            usuario.CondominoId = (int)CondominoId;
            HttpContext.Session.SetString("Usuario", JsonConvert.SerializeObject(usuario));
            return RedirectToAction("Index", "Visitas");
        }

        public IActionResult SeleccionarProyecto(int? ProyectoId)
        {
            if (ProyectoId == null)
            {
                ProyectosHabitacionales data = seleccionaProyectos();
                return View("SeleccionarProyectoHabitacional", new ModeloBase
                {
                    Objeto = data,
                    Error = new ErrorViewModel { Error = "Debe seleccionar un proyecto." }
                });
            }
            var usuario = ValidarUsuario();
            usuario.ProyectoId = (int)ProyectoId;
            HttpContext.Session.SetString("Usuario", JsonConvert.SerializeObject(usuario));
            return RedirectToAction("Index", "VehiculosPorProyecto");
        }

        private Condominos seleccionaCondominos()
        {
            var bus = new CondominosBusiness(_configuration);
            List<Condominos> lista = bus.ObtenerCondominosPorUsuario(ValidarUsuario().Id);
            Condominos data = new Condominos();

            data.ListaCondominos = lista.Select(p => new SelectListItem
            {
                Text = p.Nombre.ToString(),
                Value = p.Id.ToString()
            });
            return data;
        }

        private ProyectosHabitacionales seleccionaProyectos()
        {
            var bus = new ProyectosHabitacionalesBusiness(_configuration);
            List<ProyectosHabitacionales> proyectos = bus.ObtenerProyectosHabitaciones(null, ValidarUsuario().Id);
            ProyectosHabitacionales data = new ProyectosHabitacionales();

            data.Proyectos = proyectos.Select(p => new SelectListItem
            {
                Text = p.Nombre.ToString(),
                Value = p.Id.ToString()
            });
            return data;
        }

        private UsuarioActivo ValidarUsuario()
        {
            var sessionValue = HttpContext.Session.GetString("Usuario");

            if (string.IsNullOrEmpty(sessionValue))
            {
                throw new Exception("Por favor inicie sesión");
            }

            return JsonConvert.DeserializeObject<UsuarioActivo>(sessionValue);
        }

    }
}