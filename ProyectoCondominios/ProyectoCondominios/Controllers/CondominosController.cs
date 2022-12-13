using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using NuGet.Protocol.Plugins;
using ProyectoCondominios.Business;
using ProyectoCondominios.Models;
using Proyectocondominos.Business;
using Proyectocondominos.Models;
using System.Net.Mail;
using System.Net;
using ProyectoCondominios.Utilitarios;
using System.Drawing;
using System.Drawing.Imaging;

namespace ProyectoCondominios.Controllers
{
    public class CondominosController : Controller
    {
        private readonly IConfiguration _configuration;
        private UsuarioActivo _usuario;

        public CondominosController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IActionResult Index()
        {
            try
            {
                List<Condominos> result = CargarCondominos();

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
                List<Condominos> result = CargarCondominos();

                if (string.IsNullOrEmpty(palabraClave))
                {
                    return View("Index", new ModeloBase { Datos = result, Error = new ErrorViewModel() });
                }
                palabraClave = palabraClave.ToLower();
                var buscador = result.Where(x => x.Nombre.ToLower().Contains(palabraClave) || x.Cedula.ToLower().Contains(palabraClave) ||
                    x.Telefonos.ToLower().Contains(palabraClave) || x.Email.ToLower().Contains(palabraClave)).ToList();
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
            var condomino = new Condominos();
            if (id != null)
            {
                var bus = new CondominosBusiness(_configuration);
                condomino = bus.Obtener(new Condominos { Id = id }, _usuario.Id).FirstOrDefault();
            }
            ViewBag.Usuario = _usuario;
            return View("AgregarOEditar", condomino);
        }

        public IActionResult Guardar(Condominos model)
        {
            try
            {
                ValidarUsuario();
                var bus = new CondominosBusiness(_configuration);

                //Convierte la imagen
                if (model.Archivo != null)
                {
                    var convertidor = new TransformadorDeImagenes();
                    model.Foto = convertidor.ConvertirImagenABlob(model.Archivo);
                }  
                
                if (model.Id == 0 || model.Id == null)
                {
                    string message = bus.CrearObteniendoMensaje(model, _usuario.Id);

                    if (!string.IsNullOrEmpty(message))
                    {
                        EnvioDeCorreoElectronico(message, model);
                    }                    
                }
                else
                {
                    bus.Editar(model, _usuario.Id);
                }
                List<Condominos> result = CargarCondominos();
                return View("Index", new ModeloBase { Datos = result, Error = new ErrorViewModel() });
            }
            catch (Exception ex)
            {
                return View("Index", new ModeloBase { Error = new ErrorViewModel { Error = ex.Message } });
            }
        }

        private static void EnvioDeCorreoElectronico(string message, Condominos modelo)
        {
            var senderEmail = new MailAddress(Constantes.EmailFrom, Constantes.EmailName);
            var receiverEmail = new MailAddress(modelo.Email, modelo.Nombre);
            var password = Constantes.EmailPass;
            string sub = "Registro de Condómino. Usuario";
            var body = message;
            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(senderEmail.Address, password)
            };
            using (var mess = new MailMessage(senderEmail, receiverEmail)
            {
                Subject = sub,
                Body = body
            })
            {
                smtp.Send(mess);
            }
        }

        public IActionResult Eliminar(int id)
        {
            try
            {
                ValidarUsuario();
                var bus = new CondominosBusiness(_configuration);
                bus.Eliminar(id, _usuario.Id);

                List<Condominos> result = CargarCondominos();
                return View("Index", new ModeloBase { Datos = result, Error = new ErrorViewModel() });
            }
            catch (Exception ex)
            {
                return View("Index", new ModeloBase { Error = new ErrorViewModel { Error = ex.Message } });
            }
        }

        private List<Condominos> CargarCondominos()
        {
            ValidarUsuario();
            var bus = new CondominosBusiness(_configuration);
            var result = bus.Obtener(new Condominos(), _usuario.Id);
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
