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
        public ActionResult Login(string usuario, string password)
        {
            if(!string.IsNullOrEmpty(usuario) && !string.IsNullOrEmpty(password))
            {
                EduApp_BDEntities db = new EduApp_BDEntities();
                var user=db.USUARIO.FirstOrDefault(e => e.USUARIO1 == usuario && e.CONTRASEÑA == password);
                if (user != null)
                {
                    if(user.ROL.ID_ROL == 1)
                    {
                        FormsAuthentication.SetAuthCookie(user.USUARIO1, true);
                        return RedirectToAction("Index", "Docente");
                    } else if(user.ROL.ID_ROL == 2)
                    {
                        FormsAuthentication.SetAuthCookie(user.USUARIO1, true);
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
            FormsAuthentication.SignOut();
            return RedirectToAction("Index");
        }
    }
}