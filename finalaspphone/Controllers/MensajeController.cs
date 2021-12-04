using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using finalaspphone.Models;

namespace finalaspphone.Controllers
{
    public class MensajeController : Controller
    {
        private BasesitaEntities db = new BasesitaEntities();


        // GET: Mensaje
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult EnviarMensaje(int telefono)
        {

            Telefono telefono1 = db.Telefono.Find(telefono);
            ViewBag.remitente = telefono1.nroTelefono;
            ViewBag.codRemitente = telefono;
            List<Telefono> destinatarios = (from d in db.Telefono where d.codPersona != telefono1.codPersona select d).ToList();
            ViewBag.destinatario = new SelectList(destinatarios, "codTelefono", "nroTelefono");
            return View();
        }

        public ActionResult Enviar(string codRemitente, string destinatario, string mensaje)
        {
            Random random = new Random();
            int remitente = int.Parse(codRemitente);
            int codDestinatario = int.Parse(destinatario);
            Telefono telefono1 = db.Telefono.Find(remitente);
            Telefono telefono2 = db.Telefono.Find(codDestinatario);
            Mensaje mensaje1 = new Mensaje();
            mensaje1.codMensaje = (codRemitente + destinatario).GetHashCode() * random.Next(1456789);
            mensaje1.codRemitente = remitente;
            mensaje1.destinatario = telefono2.nroTelefono;
            mensaje1.remitente = telefono1.nroTelefono;
            mensaje1.contenido = mensaje;
            mensaje1.fecha = DateTime.Now;

            db.Mensaje.Add(mensaje1);
            db.SaveChanges();

            return RedirectToAction("Index", "Login");
        }

        public ActionResult Historial(int telefono)
        {
            Telefono telefono1 = db.Telefono.Find(telefono);
            List<Mensaje> mensajes = (from d in db.Mensaje where d.codRemitente == telefono || d.destinatario == telefono1.nroTelefono select d).ToList();

            return View(mensajes);
        }
    }
}