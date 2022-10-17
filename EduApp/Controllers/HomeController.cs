using EduApp.Models;
using EduApp.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace EduApp.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        
        
        public ActionResult Login(string message="")
        {
            ViewBag.Message = message;
            return View();
        }

        [HttpPost]
        public ActionResult Login(string DNI, string password)
        {
            if(!string.IsNullOrEmpty(DNI) && !string.IsNullOrEmpty(password))
            {
                EduApp_Entities db = new EduApp_Entities();
                var user=db.USUARIO.FirstOrDefault(e => e.USUARIO1 == DNI && e.CONTRASEÑA == password);
                if (user != null)
                {
                    if(user.ROL.ID_ROL == 1)
                    {
                        var IdDocente = db.DOCENTE.Where(e => e.ID_USUARIO == user.ID_USUARIO).Select(u=> u.ID_DOCENTE).FirstOrDefault();
                        FormsAuthentication.SetAuthCookie(user.USUARIO1, true);
                        TempData["IdDocente"]= IdDocente;
                        return RedirectToAction("Index", "Docente");
                    } else if(user.ROL.ID_ROL == 2)                    {
                        var IdAlumno = db.ALUMNO.Where(e => e.ALUMNO_USUARIO == user.ID_USUARIO).Select(u => u.ID_ALUMNO).FirstOrDefault();
                        FormsAuthentication.SetAuthCookie(user.USUARIO1, true);
                        TempData["IdAlumno"] = IdAlumno;
                        return RedirectToAction("Index", "Estudiante");
                    }
                }
                else
                {
                    return RedirectToAction("Login", new { message = "No se encontraron los datos" });
                }
            }
            else
            {
                return RedirectToAction("Login", new { message = "Llena los campos para poder inciar sesión" });
            }
            return Login("Llena los campos para poder iniciar sesión");
        }
       

        [Authorize]
        public ActionResult LogOut()
        {
            Session.Abandon();
            FormsAuthentication.SignOut();
            return RedirectToAction("Index");
        }
    }
}