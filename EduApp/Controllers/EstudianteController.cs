using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EduApp.Controllers
{
    [Authorize]
    public class EstudianteController : Controller
    {
        // GET: Estudiante
        public ActionResult Index()
        {
            return View("~/Views/Estudiante/Dashboard/Index.cshtml");
        }


        public ActionResult Cursos()
        {
            return View("~/Views/Estudiante/Cursos/Index.cshtml");
        }
        public ActionResult Actividades()
        {
            return View("~/Views/Alumnos/Actividades/Index.cshtml");
        }

        //Foro
        public ActionResult Foro()
        {
            return View("~/Views/Alumnos/Actividades/Foro.cshtml");
        }
        public ActionResult Lectura()
        {
            return View("~/Views/Alumnos/Actividades/Lectura.cshtml");
        }
        public ActionResult ActividadTest()
        {
            return View("~/Views/Alumnos/Actividades/Foro.cshtml");
        }
        //Perfil
        public ActionResult Perfil()
        {
            return View("~/Views/Estudiante/Perfil/Index.cshtml");
        }

    }
}