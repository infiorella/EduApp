using EduApp.Models.ViewModels;
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
        // GET: Estudiante6
        public ActionResult Index()
        {
            EduApp_Entities db = new EduApp_Entities();
            if (TempData["IdAlumno"] != null)
            {
                //var GUID = System.Web.HttpContext.Current.User.Identity.
                Session["IdAlumno"] = TempData["IdAlumno"];
                int id= Convert.ToInt32(TempData["IdAlumno"]);

                ALUMNO alumno = db.ALUMNO.Where(x => x.ID_ALUMNO == id).FirstOrDefault();
                int actividadesRealizadas = db.DETALLE_ACTIVIDAD.Where(x => x.ID_ALUMNO == id && x.ACTIVIDAD.ESTADO==2).Count();
                int actividadesPendientes = db.DETALLE_ACTIVIDAD.Where(x => x.ID_ALUMNO == id && x.ACTIVIDAD.ESTADO == 2 && x.ACTIVIDAD.FECHA_LIMITE< DateTime.Now).Count();
                int competenciasAlcanzadas = db.DETALLE_ACTIVIDAD.Where(x => x.ID_ALUMNO == id && x.NOTA!="B" && x.NOTA!="C").Count();
                int competenciasNoAlcanzadas= db.DETALLE_ACTIVIDAD.Where(x => x.ID_ALUMNO == id && x.NOTA == "C").Count();
                List<CURSOS> cursos = db.CURSOS.ToList();
                List<DETALLE_ACTIVIDAD> actividad = db.DETALLE_ACTIVIDAD.Where(x => x.ID_ALUMNO == id && x.COMENTARIO!="").OrderByDescending(x => x.FECHA).Take(3).ToList();

                AlumnoIndexModel model = new AlumnoIndexModel(alumno, actividadesRealizadas, actividadesPendientes,
                    competenciasAlcanzadas, competenciasNoAlcanzadas, cursos, actividad);

                return View("~/Views/Estudiante/Dashboard/Index.cshtml", model);
            }
            else
            {
                return View("~/Views/Shared/Error.cshtml");
            }            
        }

        [HttpPost]
        public JsonResult GetChartCompetenciasIndex(int IdCurso)
        {
            int id = Convert.ToInt16(Session["IdAlumno"].ToString());
            EduApp_Entities db = new EduApp_Entities();
            List<COMPETENCIA> competencias = db.COMPETENCIA.Where(x => x.ID_CURSO == IdCurso).ToList();
            List<int> data = new List<int>();
            List<string> labels = new List<string>();
            foreach (var comp in competencias)
            {
                labels.Add(comp.NOMBRE.Substring(0, 20));
                data.Add(db.DETALLE_ACTIVIDAD.Where(x => x.ACTIVIDAD.ID_COMPETENCIA == comp.ID_COMPETENCIA && x.ID_ALUMNO == id).Count());
            }

            return Json(new { labels, data }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Cursos()
        {
            EduApp_Entities db = new EduApp_Entities();
            List<CURSOS> model = db.CURSOS.ToList();
            return View("~/Views/Estudiante/Cursos/Index.cshtml", model);
        }
        public ActionResult Actividades()
        {
            return View("~/Views/Estudiante/Actividades/Index.cshtml");
        }

        //Foro
        public ActionResult Foro()
        {
            EduApp_Entities db = new EduApp_Entities();
            int id = Convert.ToInt32(Session["IdAlumno"].ToString());
            int grado_Seccion = db.ALUMNO.Where(x => x.ID_ALUMNO == id).Select(x => x.ID_GRADO_SECCION).FirstOrDefault();
            List<ACTIVIDAD> model = db.ACTIVIDAD.Where(x => x.ACTIVIDAD_TIPO == 1 && x.DOCENTE.ID_GRADO_SECCION== grado_Seccion ).ToList();
            
            return View("~/Views/Estudiante/Actividades/Foro.cshtml", model);
        }

        public ActionResult VerForo(int id)
        {
            EduApp_Entities db = new EduApp_Entities();
            ACTIVIDAD model = db.ACTIVIDAD.Where(x => x.ID_ACTIVIDAD == id).FirstOrDefault();

            return View("~/Views/Estudiante/Actividades/VerForo.cshtml", model);
        }
        public ActionResult Lectura()
        {
            EduApp_Entities db = new EduApp_Entities();
            int id = Convert.ToInt32(Session["IdAlumno"].ToString());
            int grado_Seccion = db.ALUMNO.Where(x => x.ID_ALUMNO == id).Select(x => x.ID_GRADO_SECCION).FirstOrDefault();
            List<ACTIVIDAD> model = db.ACTIVIDAD.Where(x => x.ACTIVIDAD_TIPO == 3 && x.DOCENTE.ID_GRADO_SECCION == grado_Seccion).OrderByDescending(x => x.FECHA_CREACION).ToList();

            return View("~/Views/Estudiante/Actividades/Lecturas.cshtml", model);
        }

        public ActionResult VerLectura(int id)
        {
            EduApp_Entities db = new EduApp_Entities();
            ACTIVIDAD model = db.ACTIVIDAD.Where(x => x.ID_ACTIVIDAD == id).FirstOrDefault();

            return View("~/Views/Estudiante/Actividades/VerLectura.cshtml", model);
        }

        public ActionResult Test()
        {
            EduApp_Entities db = new EduApp_Entities();
            int id = Convert.ToInt32(Session["IdAlumno"].ToString());
            int grado_Seccion = db.ALUMNO.Where(x => x.ID_ALUMNO == id).Select(x => x.ID_GRADO_SECCION).FirstOrDefault();
            List<ACTIVIDAD> model = db.ACTIVIDAD.Where(x => x.ACTIVIDAD_TIPO == 2 && x.DOCENTE.ID_GRADO_SECCION == grado_Seccion).OrderByDescending(x => x.FECHA_CREACION).ToList();

            return View("~/Views/Estudiante/Actividades/Test.cshtml", model);
        }

        public ActionResult VerTest(int id)
        {
            EduApp_Entities db = new EduApp_Entities();
            ACTIVIDAD model = db.ACTIVIDAD.Where(x => x.ID_ACTIVIDAD == id).FirstOrDefault();

            return View("~/Views/Estudiante/Actividades/VerTest.cshtml", model);
        }

        //Perfil
        public ActionResult Perfil()
        {
            int id = Convert.ToInt16(Session["IdAlumno"].ToString());
            EduApp_Entities db = new EduApp_Entities();
            ALUMNO alumno = db.ALUMNO.SingleOrDefault(x => x.ID_ALUMNO == id);

            //Actividad
            List<DETALLE_ACTIVIDAD> detalleactividad = db.DETALLE_ACTIVIDAD.Where(x => x.ID_ALUMNO == id).OrderByDescending(x => x.FECHA).ToList();

            List<DETALLE_ACTIVIDAD> TodasActividades = db.DETALLE_ACTIVIDAD.Where(x => x.ID_ALUMNO == id).ToList();

            //EXTRAS
            List<CURSOS> cursos = db.CURSOS.ToList();
            int cantidadActividades = db.DETALLE_ACTIVIDAD.Where(x => x.ID_ALUMNO == id).Count();
            int cursosSinLogroAlcanzado = db.DETALLE_ACTIVIDAD.Where(x => x.ID_ALUMNO == id && x.NOTA != "AD" && x.NOTA != "A").GroupBy(x => x.ACTIVIDAD.ID_ACTIVIDAD).Count();


            var ViewModel = new AlumnoDetalleModel()
            {
                Alumno = alumno,
                Actividad = detalleactividad,
                cursos = cursos,
                cantidadActividades = cantidadActividades,
                cursosSinLogroAlcanzado = cursosSinLogroAlcanzado,
                TodasActividades = TodasActividades
            };

            return View("~/Views/Estudiante/Perfil/Index.cshtml", ViewModel);
        }

        [HttpPost]
        public JsonResult GetChartCompetenciasAlumnoDetalle(int IdCurso, int IdAlumno)
        {

            EduApp_Entities db = new EduApp_Entities();
            List<COMPETENCIA> competencias = db.COMPETENCIA.Where(x => x.ID_CURSO == IdCurso).ToList();

            List<int> data = new List<int>();
            List<string> labels = new List<string>();

            int nota;
            foreach (var comp in competencias)
            {
                labels.Add(comp.NOMBRE.Substring(0, 20));

                string PROGRESO = db.DETALLE_ACTIVIDAD.
                    Where(x => x.ACTIVIDAD.COMPETENCIA.ID_COMPETENCIA == comp.ID_COMPETENCIA && x.ALUMNO.ID_ALUMNO == IdAlumno)
                    .Select(x => x.NOTA).SingleOrDefault();

                if (PROGRESO == "AD")
                {
                    nota = 20;
                }
                else if (PROGRESO == "A")
                {
                    nota = 15;
                }
                else if (PROGRESO == "B")
                {
                    nota = 10;
                }
                else if (PROGRESO == "C")
                {
                    nota = 05;
                }
                else
                {
                    nota = 0;
                }

                data.Add(nota);
            }

            return Json(new { labels, data }, JsonRequestBehavior.AllowGet);
        }

    }
}