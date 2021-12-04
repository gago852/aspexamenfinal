using finalaspphone.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace finalaspphone.Controllers
{
    public class TelefonoController : Controller
    {

        private BasesitaEntities db = new BasesitaEntities();
        // GET: Telefono
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Agregar()
        {
            return View();
        }

        public ActionResult Bajar(string telefono)
        {
            var persona = HttpContext.Session["logeo"] as Persona;

            Baja baja = new Baja();
            baja.cedula = persona.cedula;
            baja.numero = telefono;

            Baja baja1 = (from d in db.Baja where d.numero == telefono select d).FirstOrDefault();
            if (baja1!=null)
            {
                return RedirectToAction("Index","Login");
            }

            db.Baja.Add(baja);
            db.SaveChanges();

            return RedirectToAction("Index", "Login");
        }

        public ActionResult Recargar(int? telefono)
        {
            Telefono telefono1 = db.Telefono.Find(telefono);
            return View(telefono1);
        }

        public ActionResult Recargacion(int telefono, int saldo)
        {
            Telefono telefono1 = db.Telefono.Find(telefono);
            int saldoActual = telefono1.saldo;
            int saldoNuevo = saldoActual + saldo;
            telefono1.saldo = saldoNuevo;
            db.Entry(telefono1).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index", "Login");
        }

        public ActionResult Altar(string telefono)
        {
            var persona = HttpContext.Session["logeo"] as Persona;

            try
            {
                Alta alta = new Alta();
                alta.cedula = persona.cedula;
                alta.telefono = telefono;
                db.Alta.Add(alta);
                db.SaveChanges();
            }
            catch
            {
                return View("Error");
            }

            return RedirectToAction("Index", "Login");
        }

        public ActionResult Prender(int telefono)
        {
            Telefono telefono1 = db.Telefono.Find(telefono);
            if (telefono1.estado)
            {
                telefono1.estado = false;
            }
            else
            {
                telefono1.estado = true;
            }

            db.Entry(telefono1).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index", "Login");
        }
    }
}