using finalaspphone.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace finalaspphone.Controllers
{
    public class LoginController : Controller
    {

        private BasesitaEntities db = new BasesitaEntities();

        // GET: Login
        public ActionResult Index()
        {
            Persona persona = HttpContext.Session["logeo"] as Persona;
            var admin = HttpContext.Session["logueoAdmin"] as Admin;

            if (admin != null)
            {
                return RedirectToAction("Index", "Admin");
            }

            if (persona == null)
            {
                return RedirectToAction("Login");
            }
            else
            {
                return View(persona);
            }

        }

        public ActionResult Login()
        {
            Persona persona = HttpContext.Session["logeo"] as Persona;
            var admin = HttpContext.Session["logueoAdmin"] as Admin;

            if (admin != null)
            {
                return RedirectToAction("Index", "Admin");
            }

            if (persona != null)
            {
                return RedirectToAction("Index");
            }
            else
            {
                return View();
            }
        }

        public ActionResult Cerrar()
        {
            HttpContext.Session["logeo"] = null;
            return RedirectToAction("Login");
        }

        public ActionResult UserNoActivo()
        {
            Persona persona = HttpContext.Session["logeo"] as Persona;
            if (persona != null)
            {
                List<Alta> alta = (from d in db.Alta where d.cedula == persona.cedula select d).ToList();
                if (alta.Count != 0)
                {
                    return View("UserNoActivoEspera");
                }
                else
                {
                    return View("UserNoActivo");
                }
            }
            else
            {
                return RedirectToAction("Login");
            }
        }

        public ActionResult Loguear(string correo, string pass)
        {
            Persona persona = (from d in db.Persona
                               where d.correo == correo && d.pass == pass.Trim()
                               select d).FirstOrDefault();

            if (persona != null)
            {
                List<Telefono> telefonos = (from d in db.Telefono where d.codPersona == persona.cedula select d).ToList();
                HttpContext.Session["logeo"] = persona;
                if (telefonos.Count == 0)
                {
                    return RedirectToAction("UserNoActivo");
                }
                return RedirectToAction("Index");
            }
            return RedirectToAction("Login");

        }

        [ChildActionOnly]
        public ActionResult Telefonos()
        {
            Persona persona = (Persona)System.Web.HttpContext.Current.Session["logeo"];
            List<Telefono> telefonos = (from d in db.Telefono
                                        where d.codPersona == persona.cedula
                                        select d).ToList();
            return PartialView("TelefonosParcial", telefonos);
        }
    }
}