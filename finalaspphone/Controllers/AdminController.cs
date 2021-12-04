using finalaspphone.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace finalaspphone.Controllers
{
    public class AdminController : Controller
    {

        private BasesitaEntities db = new BasesitaEntities();
        // GET: Admin
        public ActionResult Index()
        {
            var admin = HttpContext.Session["logueoAdmin"] as Admin;
            Persona persona = HttpContext.Session["logeo"] as Persona;

            if (persona != null)
            {
                return RedirectToAction("Index", "Login");
            }

            if (admin != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Login");
            }

        }

        public ActionResult Login()
        {
            var admin = HttpContext.Session["logueoAdmin"] as Admin;
            Persona persona = HttpContext.Session["logeo"] as Persona;

            if (persona != null)
            {
                return RedirectToAction("Index", "Login");
            }

            if (admin == null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Index");
            }
        }

        public ActionResult Cerrar()
        {
            HttpContext.Session["logueoAdmin"] = null;
            return RedirectToAction("Login");
        }

        public ActionResult Loguear(string usuario, string pass)
        {
            Admin admin = (from d in db.Admin
                           where d.usuario == usuario && d.passAdmin == pass.Trim()
                           select d).FirstOrDefault();

            if (admin != null)
            {
                HttpContext.Session["logueoAdmin"] = admin;

                return RedirectToAction("Index");
            }
            return RedirectToAction("Login");

        }

        public ActionResult Activar()
        {
            return View(db.Alta.ToList());
        }

        public ActionResult Bajar()
        {
            return View(db.Baja.ToList());
        }

        public ActionResult Deletear(int cedula, string telefono)
        {
            Telefono telefonoBorrar = (from d in db.Telefono where d.nroTelefono == telefono select d).FirstOrDefault();
            List<Mensaje> mensajes = (from d in db.Mensaje where d.codRemitente == telefonoBorrar.codTelefono select d).ToList();
            if (mensajes.Count != 0)
            {
                db.Mensaje.RemoveRange(mensajes);
            }
            db.Telefono.Remove(telefonoBorrar);
            db.SaveChanges();
            List<Telefono> telefonos = (from d in db.Telefono where d.codPersona == cedula select d).ToList();

            if (telefonos.Count == 0)
            {
                Persona persona = db.Persona.Find(cedula);
                db.Persona.Remove(persona);
            }
            Baja baja = db.Baja.Find(cedula);
            db.Baja.Remove(baja);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Activartear(int cedula, string telefono)
        {
            Telefono telefonoNuevo = new Telefono();
            telefonoNuevo.codPersona = cedula;
            telefonoNuevo.desvio = false;
            telefonoNuevo.estado = false;
            telefonoNuevo.nroTelefono = telefono;
            telefonoNuevo.saldo = 0;
            telefonoNuevo.codTelefono = (cedula + telefono).GetHashCode();
            db.Telefono.Add(telefonoNuevo);
            Alta alta = db.Alta.Find(cedula);
            db.Alta.Remove(alta);
            db.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}