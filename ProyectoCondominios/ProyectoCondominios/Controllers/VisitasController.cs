using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using ProyectoCondominios.Business;
using ProyectoCondominios.Models;
using Proyectocondominos.Models;
using ProyectoCondominios.Utilitarios;
using System.Net.Mail;
using System.Net;
using System.Net.Mime;
using System.Text;

namespace ProyectoCondominios.Controllers
{
    public class VisitasController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _environment;
        private UsuarioActivo _usuario;

        public VisitasController(IConfiguration configuration, IWebHostEnvironment environment)
        {
            _configuration = configuration;
            _environment = environment;
        }

        public IActionResult Index()
        {
            try
            {
                ModeloBase result = CargarVisitas();
                return View(result);
            }
            catch (Exception ex)
            {
                return View(new ModeloBase { Error = new ErrorViewModel { Error = ex.Message } });
            }
        }

        public IActionResult GenerarEasyPass()
        {
            try
            {
                ValidarUsuario();
                var bus = new EasyPassBusiness(_configuration);
                var generador = new GeneradorCodigoQR();
                var easypass = bus.CrearYRetorna(_usuario.CondominoId, _usuario.Id);
                easypass.Foto = generador.GenerarCodigoQR(easypass.Codigo, _usuario.Nombre.Replace(" ", ""), _environment);
                return View("GenerarEasyPass", easypass);
            }
            catch (Exception ex)
            {
                return View("Index", new ModeloBase { Error = new ErrorViewModel { Error = ex.Message } });
            }
        }

        public IActionResult GuardarVisitaFavorita(int VisitaFavoritaId)
        {
            if (VisitaFavoritaId > 0)
            {
                ValidarUsuario();
                var bus = new VisitasFavoritasBusiness(_configuration);
                var visitaFavorita = bus.Obtener(new Visitas { Id = VisitaFavoritaId }, _usuario.Id).FirstOrDefault();
                visitaFavorita.Id = null;
                return View("AgregarOEditar", visitaFavorita);
            }

            ModeloBase result = CargarVisitas();
            return View("Index", result);
        }

        public IActionResult CompartirEasyPass(EasyPass model)
        {
            if (!string.IsNullOrEmpty(model.Email))
            {
                ValidarUsuario();
                EnvioDeCorreoElectronico(model);
                ModeloBase result = CargarVisitas();
                return View("Index", result);
            }
            model.Error = new ErrorViewModel { Error = "Digite el email" };
            return View("GenerarEasyPass", model);
        }

        private void EnvioDeCorreoElectronico(EasyPass model)
        {
            var htmlContent = string.Format("Hola, \n Adjunto su codigo QR para Easy Pass para su visita a: {0}. \n Su código de 4 dígitos es: {1}." +
                " \n El código tiene validez hasta: {2}.\n Gracias por su visita.", _usuario.Nombre, model.Codigo, model.FechaExpiracion);

            var image = new LinkedResource(new MemoryStream(model.Foto), MediaTypeNames.Image.Jpeg);
            var alternateView = AlternateView.CreateAlternateViewFromString(htmlContent, Encoding.UTF8, "text/html");
            alternateView.LinkedResources.Add(image);

            var senderEmail = new MailAddress(Constantes.EmailFrom, Constantes.EmailName);
            var receiverEmail = new MailAddress(model.Email, model.Email);
            var password = Constantes.EmailPass;
            string sub = "EasyPass para visita";
            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(senderEmail.Address, password)
            };
            var mailMessage = new MailMessage
            {
                From = new MailAddress(Constantes.EmailFrom, Constantes.EmailName, Encoding.UTF8),
                Subject = sub,
                SubjectEncoding = Encoding.UTF8,
                IsBodyHtml = true,
                Body = htmlContent,
                Priority = MailPriority.Normal,
                BodyEncoding = Encoding.UTF8
            };

            mailMessage.To.Add(receiverEmail);
            mailMessage.AlternateViews.Add(alternateView);
            smtp.Send(mailMessage);
        }

        public IActionResult Buscar(string palabraClave)
        {
            try
            {
                ModeloBase result = CargarVisitas();

                if (string.IsNullOrEmpty(palabraClave))
                {
                    return View("Index", result);
                }
                palabraClave = palabraClave.ToLower();

                var lista = (List<Visitas>)result.Datos;
                var buscador = lista.Where(x => x.Nombre.ToLower().Contains(palabraClave) || x.Cedula.ToLower().Contains(palabraClave) ||
                    x.Vehiculo.ToLower().Contains(palabraClave)).ToList();

                result.Datos = buscador;
                result.Buscando = true;
                return View("Index", result);
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
                var bus = new VisitasBusiness(_configuration);
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
                var bus = new VisitasBusiness(_configuration);
                if (model.Id == 0 || model.Id == null)
                {
                    bus.Crear(model, _usuario.Id);
                }
                else
                {
                    bus.Editar(model, _usuario.Id);
                }
                ModeloBase result = CargarVisitas();
                return View("Index", result);
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
                var bus = new VisitasBusiness(_configuration);
                bus.Eliminar(id, _usuario.Id);

                ModeloBase result = CargarVisitas();
                return View("Index", result);
            }
            catch (Exception ex)
            {
                return View("Index", new ModeloBase { Error = new ErrorViewModel { Error = ex.Message } });
            }
        }

        private ModeloBase CargarVisitas()
        {
            ValidarUsuario();
            var bus = new VisitasBusiness(_configuration);
            var busFavoritas = new VisitasFavoritasBusiness(_configuration);
            var data = new Visitas();
            var modelo = new ModeloBase();

            var lista = bus.Obtener(new Visitas(), _usuario.Id);
            var listaFavoritas = busFavoritas.Obtener(new Visitas(), _usuario.Id);

            data.VisitasFavoritas = listaFavoritas.Select(p => new SelectListItem
            {
                Text = String.Format("{0} - {1} - {2}", p.Nombre, p.Cedula, p.Vehiculo),
                Value = p.Id.ToString()
            });

            modelo.Objeto = data;
            modelo.Datos = lista;
            modelo.Error = new ErrorViewModel();

            ViewBag.Usuario = _usuario;
            return modelo;
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
