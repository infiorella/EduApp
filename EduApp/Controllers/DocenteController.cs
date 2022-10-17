using EduApp.Models;
using EduApp.Models.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static EduApp.Models.CursoModel;

namespace EduApp.Controllers
{
    //Session["FirstName"] = FirstNameTextBox.Text;
    [Authorize]
    public class DocenteController : Controller
    {
        // GET: Docente
        public ActionResult Index()
        {
            EduApp_Entities db = new EduApp_Entities();
            DOCENTE docente = new DOCENTE();
            int actividadesCreadas = 0;
            int actividadesCompletadas = 0;
            int estudiantes = 0;
            //DOCENTE
            if (TempData["IdDocente"] != null)
            {
                Session["IdDocente"] = TempData["IdDocente"];
                int IdDocente = int.Parse(TempData["IdDocente"].ToString());
                docente = db.DOCENTE.SingleOrDefault(x => x.ID_DOCENTE == IdDocente);

                Session["GradoSeccion"] = docente.GRADO_SECCION.ID_GRADO_SECCION.ToString();

                //ACTIVIDADES
                actividadesCreadas = db.ACTIVIDAD.Where(x => x.FECHA_CREACION.Month == DateTime.Now.Month).Count();

                actividadesCompletadas = db.ACTIVIDAD.Where(x => x.ESTADO == 2 &&
                    x.DETALLE_ACTIVIDAD.Where(e => e.ID_ACTIVIDAD == IdDocente && e.NOTA == "A" || e.NOTA == "AD").Count() >
                    0.5 * (x.DETALLE_ACTIVIDAD.Where(e => e.ID_ACTIVIDAD == IdDocente).Count())).Count();

                //Estudiantes
                estudiantes = db.ALUMNO.Where(x => x.ID_GRADO_SECCION == docente.ID_GRADO_SECCION).Count();

                List<ALUMNO> alumnosayuda = db.DETALLE_ACTIVIDAD.OrderByDescending(x => x.FECHA)
                    .Where(x => x.NOTA == "C" && x.ALUMNO.ID_GRADO_SECCION == docente.ID_GRADO_SECCION).Take(4).Select(e => e.ALUMNO).ToList();

                List<ACTIVIDAD> utilmasactividades = db.ACTIVIDAD.Where(x => x.ID_DOCENTE == IdDocente).OrderByDescending(x => x.FECHA_CREACION).Take(3).ToList();

                //Competencias
                int competencias = db.COMPETENCIA.Count();

                List<CURSOS> cursos = db.CURSOS.ToList();
                //Ultima actividad
                ACTIVIDAD actividad = db.ACTIVIDAD.OrderByDescending(p => p.FECHA_CREACION).FirstOrDefault();

                //Para el chart de nota
                List<TIPO_ACTIVIDAD> ListActividades = db.TIPO_ACTIVIDAD.OrderBy(x => x.NOMBRE).ToList();


                DocenteIndexModel index = new DocenteIndexModel(docente, actividadesCreadas, actividadesCompletadas, estudiantes, competencias, cursos, actividad,
                alumnosayuda, utilmasactividades, ListActividades);

                return View("~/Views/Docente/Dashboard/Index.cshtml", index);
            }

            return View("~/Views/Shared/Error.cshtml");
        }

        [HttpPost]
        public JsonResult GetChartNotasIndex(int IdTipoActividad)
        {
            EduApp_Entities db = new EduApp_Entities();
            int AD = db.DETALLE_ACTIVIDAD.Where(x => x.ACTIVIDAD.ACTIVIDAD_TIPO == IdTipoActividad && x.NOTA == "AD").Count();
            int A = db.DETALLE_ACTIVIDAD.Where(x => x.ACTIVIDAD.ACTIVIDAD_TIPO == IdTipoActividad && x.NOTA == "A").Count();
            int B = db.DETALLE_ACTIVIDAD.Where(x => x.ACTIVIDAD.ACTIVIDAD_TIPO == IdTipoActividad && x.NOTA == "B").Count();
            int C = db.DETALLE_ACTIVIDAD.Where(x => x.ACTIVIDAD.ACTIVIDAD_TIPO == IdTipoActividad && x.NOTA == "C").Count();

            List<int> data = new List<int> { AD, A, B, C };

            //String.Join(", ", data.ToArray());

            return Json(data);
        }

        [HttpPost]
        public JsonResult GetChartCompetenciasIndex(int IdCurso)
        {

            EduApp_Entities db = new EduApp_Entities();
            List<COMPETENCIA> competencias = db.COMPETENCIA.Where(x => x.ID_CURSO == IdCurso).ToList();
            List<int> data = new List<int>();
            List<string> labels = new List<string>();
            foreach (var comp in competencias)
            {
                labels.Add(comp.NOMBRE.Substring(0, 20));
                data.Add(db.ACTIVIDAD.Where(x => x.ID_COMPETENCIA == comp.ID_COMPETENCIA && x.FECHA_CREACION.Month == DateTime.Now.Month).Count());
            }

            return Json(new { labels, data }, JsonRequestBehavior.AllowGet);
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
                    .Select(x=> x.NOTA).SingleOrDefault();

                if (PROGRESO == "AD")
                {
                    nota = 20;
                } else if (PROGRESO == "A")
                {
                    nota = 15;
                } else if (PROGRESO == "B")
                {
                    nota = 10;
                }
                else if(PROGRESO=="C")
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


        //Alumnos
        public ActionResult Alumnos()
        {
            List<ALUMNO> list;
            int gradoseccion = int.Parse(Session["GradoSeccion"].ToString());

            using (EduApp_Entities db = new EduApp_Entities())
            {
                list = db.ALUMNO.Where(x=> x.ID_GRADO_SECCION==gradoseccion).ToList();
            }

            return View("~/Views/Docente/Alumnos/Index.cshtml", list);
        }

        public ActionResult AlumnoDetalle(int id)
        {
            EduApp_Entities db = new EduApp_Entities();
            ALUMNO alumno = db.ALUMNO.SingleOrDefault(x => x.ID_ALUMNO == id);

            //Actividad
            List<DETALLE_ACTIVIDAD> detalleactividad = db.DETALLE_ACTIVIDAD.Where(x => x.ID_ALUMNO == id).OrderByDescending(x => x.FECHA).ToList();

            List<DETALLE_ACTIVIDAD> TodasActividades = db.DETALLE_ACTIVIDAD.Where(x => x.ID_ALUMNO == id).ToList();

            //EXTRAS
            List<CURSOS> cursos = db.CURSOS.ToList();
            int cantidadActividades = db.DETALLE_ACTIVIDAD.Where(x => x.ID_ALUMNO == id).Count();
            int cursosSinLogroAlcanzado = db.DETALLE_ACTIVIDAD.Where(x => x.ID_ALUMNO == id && x.NOTA!="AD"  && x.NOTA!="A").GroupBy(x => x.ACTIVIDAD.ID_ACTIVIDAD).Count();


            var ViewModel = new AlumnoDetalleModel()
            {
                Alumno = alumno,
                Actividad = detalleactividad,
                cursos = cursos,
                cantidadActividades = cantidadActividades,
                cursosSinLogroAlcanzado = cursosSinLogroAlcanzado,
                TodasActividades= TodasActividades
            };

            return View("~/Views/Docente/Alumnos/AlumnoDetalle.cshtml", ViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddAlumno(ALUMNO alumno)
        {
            try
            {
                using (EduApp_Entities db = new EduApp_Entities())
                {
                    int id = db.USUARIO.Select(x => x.ID_USUARIO).LastOrDefault();
                    alumno.USUARIO.ID_USUARIO = id;
                    alumno.ALUMNO_USUARIO = id;
                    db.USUARIO.Add(alumno.USUARIO);
                    db.ALUMNO.Add(alumno);
                    db.SaveChanges();
                }
            }
            catch (Exception e)
            {

            }
            return View("~/Views/Docente/Actividades/AddForo.cshtml");
        }

        public ActionResult EditAlumno(int id)
        {
            EduApp_Entities db = new EduApp_Entities();
            ALUMNO alumno = db.ALUMNO.SingleOrDefault(x => x.ID_ALUMNO == id);


            AlumnoModel alumnoVM = new AlumnoModel();
            //Detalles del alumno
            alumnoVM.Nombres = alumno.NOMBRES;
            alumnoVM.Apellido_Paterno = alumno.APELLIDO_PATERNO;
            alumnoVM.Apellido_Materno = alumno.APELLIDO_MATERNO;
            alumnoVM.Direccion = alumno.DIRECCION;
            alumnoVM.Fecha_Nacimiento = alumno.FECHA_NACIMIENTO;
            alumnoVM.telefono = alumno.TELEFONO;
            alumnoVM.Estado = alumno.ESTADO;

            return PartialView("~/Views/Docente/Alumnos/EditAlumno.cshtml", alumno);
        }

        public ActionResult AddAlumno()
        {
            var alumno = new ALUMNO();
            return View("~/Views/Docente/Alumnos/AddAlumno.cshtml", alumno);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateAlumno(int? id, ALUMNO alumno)
        {
            using (EduApp_Entities db = new EduApp_Entities())
            {
                var data = db.ALUMNO.FirstOrDefault(x => x.ID_ALUMNO == id);

                // Checking if any such record exist
                if (data != null)
                {
                    data.NOMBRES = alumno.NOMBRES;
                    data.APELLIDO_MATERNO = alumno.APELLIDO_PATERNO;
                    data.APELLIDO_PATERNO = alumno.APELLIDO_PATERNO;
                    data.FECHA_NACIMIENTO = alumno.FECHA_NACIMIENTO;
                    data.SEXO = alumno.SEXO;
                    data.TELEFONO = alumno.TELEFONO;
                    data.DIRECCION = alumno.DIRECCION;

                    db.SaveChanges();

                    return RedirectToAction("Read");
                }
                else
                    return View();
            }
        }



        public ActionResult AlumnosProgreso()
        {
            EduApp_Entities db = new EduApp_Entities();
            List<COMPETENCIA> competencias = db.COMPETENCIA.ToList();
            int gradoseccion = Convert.ToInt16(Session["GradoSeccion"].ToString());

            List<ALUMNO> alumnos = db.ALUMNO.Where(x => x.GRADO_SECCION.ID_GRADO_SECCION == gradoseccion).ToList();

            List<string> promedioNotas= new List<string>();
            List<int> promedioNotasValor = new List<int>();
            int valor=0; double promedio; string valorRegular="";


            List<ProgresoAlumnosModel> progreso = new List<ProgresoAlumnosModel>();

            foreach(var alumno in alumnos)
            {

                foreach (var comp in competencias)
                {
                    List<string> PROGRESO = db.DETALLE_ACTIVIDAD.
                        Where(x => x.ACTIVIDAD.COMPETENCIA.ID_COMPETENCIA == comp.ID_COMPETENCIA && x.ALUMNO.ID_ALUMNO == alumno.ID_ALUMNO)
                        .Select(x => x.NOTA).ToList();

                    //Convertir a valores numericos
                    foreach (var nota in PROGRESO)
                    {
                        if (nota == "AD") valor = 5;
                        if (nota == "A") valor = 4;
                        if (nota == "B") valor = 3;
                        if (nota == "C") valor = 2;
                        else valor = 0;

                        promedioNotasValor.Add(valor);
                    }
                    promedio = promedioNotasValor.Average();

                    //Convertir a valores regulares
                    if (promedio > 1 && promedio <= 2) valorRegular = "C";
                    if (promedio > 2 && promedio <= 3) valorRegular = "B";
                    if (promedio > 3 && promedio <= 4) valorRegular = "A";
                    if (promedio > 4 && promedio <= 5) valorRegular = "AD";
                    else valorRegular = "";

                    promedioNotas.Add(valorRegular);
                }

                progreso.Add(new ProgresoAlumnosModel(alumno.USUARIO.USUARIO1, alumno.APELLIDO_PATERNO + " " + alumno.NOMBRES, promedioNotas));

            }
            
            return View("~/Views/Docente/Alumnos/Progreso.cshtml", progreso);
        }



        //Curso
        public ActionResult Cursos()
        {
            EduApp_Entities db = new EduApp_Entities();
            List<CURSOS> list = db.CURSOS.ToList();

            return View("~/Views/Docente/Cursos/Index.cshtml", list);
        }

        public ActionResult CursoDetalle(int id)
        {
            EduApp_Entities db = new EduApp_Entities();


            //LUEGO VERIFICAR
            //APROBADOS
            int aprobado = db.DETALLE_ACTIVIDAD.Where(x => x.ACTIVIDAD.ID_CURSO == id && x.NOTA == "A" || x.NOTA == "AD")
                .GroupBy(x => x.ACTIVIDAD.ID_ACTIVIDAD & x.ALUMNO.ID_ALUMNO).Count();

            //Desaprobados
            int desaprobados = db.DETALLE_ACTIVIDAD.Where(x => x.ACTIVIDAD.ID_CURSO == id && x.NOTA == "C" || x.NOTA == "B")
                .GroupBy(x => x.ACTIVIDAD.ID_ACTIVIDAD & x.ALUMNO.ID_ALUMNO).Count();

            CURSOS curso = db.CURSOS.SingleOrDefault(x => x.ID_CURSO == id);

            //Detalles de curso
            List<COMPETENCIA> listCompetencia = db.COMPETENCIA.OrderByDescending(x => x.NOMBRE).Where(x => x.ID_CURSO == id).ToList();

            //Pendiente
            //List<int> progresoCompetencias=
            
            
            var ViewModel = new CursoModel()
            {
                curso = curso,
                aprobado=aprobado, 
                desaprobado=desaprobados,
                listCompetencia = listCompetencia
            };

            return View("~/Views/Docente/Cursos/CursoDetalle.cshtml", ViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddCurso(CURSOS curso)
        {
            try
            {
                using (EduApp_Entities db = new EduApp_Entities())
                {
                    db.CURSOS.Add(curso);
                    db.SaveChanges();
                }
            }
            catch (Exception e)
            {

            }
            return View("~/Views/Docente/Actividades/AddForo.cshtml");
        }


        public ActionResult EditCurso(int id)
        {
            EduApp_Entities db = new EduApp_Entities();

            CURSOS curso = db.CURSOS.SingleOrDefault(x => x.ID_CURSO == id);

            return PartialView("~/Views/Docente/Cursos/EditCurso.cshtml", curso);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateCurso(int? id, CURSOS curso)
        {
            using (EduApp_Entities db = new EduApp_Entities())
            {
                var data = db.CURSOS.FirstOrDefault(x => x.ID_CURSO == id);

                // Checking if any such record exist
                if (data != null)
                {
                    data.NOMBRE = curso.NOMBRE;
                    data.HORAS = curso.HORAS;
                    data.NIVEL = curso.NIVEL;
                    data.DESCRIPCION = curso.DESCRIPCION;
                    data.IMAGEN = curso.IMAGEN;

                    db.SaveChanges();

                    return RedirectToAction("Read");
                }
                else
                    return View();
            }
        }


        public ActionResult CursosCompetencias()
        {
            EduApp_Entities db = new EduApp_Entities();

            List<COMPETENCIA> list = db.COMPETENCIA.ToList();

            return View("~/Views/Docente/Cursos/Competencias.cshtml", list);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddCompetencia(COMPETENCIA competencia)
        {
            try
            {
                using (EduApp_Entities db = new EduApp_Entities())
                {
                    db.COMPETENCIA.Add(competencia);
                    db.SaveChanges();
                }
            }
            catch (Exception e)
            {

            }
            return View("~/Views/Docente/Actividades/AddForo.cshtml");
        }

        public ActionResult EditarCompetencia(int id)
        {
            EduApp_Entities db = new EduApp_Entities();

            COMPETENCIA competencia = db.COMPETENCIA.SingleOrDefault(x => x.ID_COMPETENCIA == id);

            return PartialView("~/Views/Docente/Cursos/EditCompetencia.cshtml", competencia);
        }

        public ActionResult ShowCompetencia(int id)
        {
            EduApp_Entities db = new EduApp_Entities();

            COMPETENCIA competencia = db.COMPETENCIA.SingleOrDefault(x => x.ID_COMPETENCIA == id);

            return PartialView("~/Views/Docente/Cursos/CompetenciaDetalle.cshtml", competencia);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateCompetencia(int? id, COMPETENCIA competencia)
        {
            using (EduApp_Entities db = new EduApp_Entities())
            {
                var data = db.COMPETENCIA.FirstOrDefault(x => x.ID_COMPETENCIA == id);

                // Checking if any such record exist
                if (data != null)
                {
                    data.NOMBRE = competencia.NOMBRE;
                    data.ID_CURSO = competencia.ID_CURSO;
                    data.DETALLE = competencia.DETALLE;

                    db.SaveChanges();

                    return RedirectToAction("Read");
                }
                else
                    return View();
            }
        }




        /*******Informes**************/
        public ActionResult InformesActividades()
        {
            EduApp_Entities db = new EduApp_Entities();
            int id = Convert.ToInt16(Session["IdDocente"].ToString());

            int cantidadCreadas = db.ACTIVIDAD.Where(x => x.ID_DOCENTE == id).Count();
            int cantidadCompletadas = db.ACTIVIDAD.Where(x => x.ID_DOCENTE == id && x.ESTADO == 2).Count();
            
            int actividadesLogroAlcanzado = db.ACTIVIDAD.Where(x => x.ESTADO == 2 &&
                x.DETALLE_ACTIVIDAD.Where(e => e.ID_ACTIVIDAD == x.ID_ACTIVIDAD && e.NOTA == "A" || e.NOTA == "AD").Count() >
                0.5 * (x.DETALLE_ACTIVIDAD.Where(e => e.ID_ACTIVIDAD == x.ID_ACTIVIDAD).Count())).Count();

            int actividadesSinLogroAlcanzado = db.ACTIVIDAD.Where(x => x.ESTADO == 2 &&
                 x.DETALLE_ACTIVIDAD.Where(e => e.ID_ACTIVIDAD == x.ID_ACTIVIDAD && e.NOTA == "C" || e.NOTA == "B").Count() >
                 0.5 * (x.DETALLE_ACTIVIDAD.Where(e => e.ID_ACTIVIDAD ==x.ID_ACTIVIDAD).Count())).Count(); ;

            List<Tuple<string, int>> actividadesSegunMes;
            List<Tuple<string, string, int>> actividadesCalificacion;

            //InformesModel informesactividades = new InformesModel();


            InformesActividadesModel model = new InformesActividadesModel(cantidadCreadas, cantidadCompletadas, actividadesLogroAlcanzado, actividadesSinLogroAlcanzado);
            return View("~/Views/Docente/Informes/Actividades.cshtml", model);
        }
        public ActionResult InformesAvance()
        {
            EduApp_Entities db = new EduApp_Entities();

            List<int> foro = new List<int>();
            for(int i=1; i<13; i++)
            {
                foro.Add(db.ACTIVIDAD.Where(x => x.TIPO_ACTIVIDAD.ID_TIPO_ACTIVIDAD == 1 && x.FECHA_CREACION.Month==i).Count());
            }

            List<COMPETENCIA> listCompetencias = db.COMPETENCIA.ToList();

            InformesAvanceModel model = new InformesAvanceModel(listCompetencias);
            return View("~/Views/Docente/Informes/Avance.cshtml", model);
        }





        /*******************foros***************/
        public ActionResult ActividadesForos()
        {
            EduApp_Entities db = new EduApp_Entities();
            int IdDocente = Convert.ToInt32(Session["IdDocente"].ToString());
            List<ACTIVIDAD> list = db.ACTIVIDAD.Where(x => x.ACTIVIDAD_TIPO == 1 && x.DOCENTE.ID_DOCENTE== IdDocente).ToList();
            List<ForosModel> foros = new List<ForosModel>();

            foreach(var detalle in list)
            {
                double nota = Math.Abs(db.VALORACION_ACTIVIDAD.
                    Where(x => x.ID_ACTIVIDAD == detalle.ID_ACTIVIDAD).Select(x => x.VALOR).Average());
                foros.Add(new ForosModel(detalle, nota));
            }
            return View("~/Views/Docente/Actividades/Foros.cshtml", foros);
        }


        //Añdir
        public ActionResult AddForo()
        {
            EditarActividadModel model = new EditarActividadModel();

            using (EduApp_Entities db = new EduApp_Entities())
            {
                var competencias = db.COMPETENCIA.Select(x => new SelectListItem
                {
                    Value = x.ID_COMPETENCIA.ToString(),
                    Text = x.NOMBRE
                }).ToList().Take(3);
                var cursos = db.CURSOS.Select(x => new SelectListItem
                {
                    Value = x.ID_CURSO.ToString(),
                    Text = x.NOMBRE
                }).ToList();
                model.competencias = competencias;
                model.cursos = cursos;
            }
                return View("~/Views/Docente/Actividades/AddForo.cshtml", model);
        }

        /*[HttpPost]
        public IEnumerable<SelectListItem> GetCompetencias(int IdCurso)
        {
            using (EduApp_Entities db = new EduApp_Entities())
            {
                IEnumerable<SelectListItem> competencias = db.COMPETENCIA.Where(x=> x.ID_CURSO== IdCurso).Select(x => new SelectListItem
                {
                    Value = x.ID_COMPETENCIA.ToString(),
                    Text = x.NOMBRE
                }).ToList();
                return competencias;
            }
        }*/


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddForo(ACTIVIDAD actividad)
        {
            try
            {
                using (EduApp_Entities db = new EduApp_Entities())
                {
                    db.ACTIVIDAD.Add(actividad);
                    db.SaveChanges();
                }
            }
            catch (Exception e)
            {

            }
            return View("~/Views/Docente/Actividades/AddForo.cshtml");
        }



        //Editar
        public ActionResult EditForo(int? id)
        {
            EditarActividadModel model = new EditarActividadModel();
            int IdDocente = Convert.ToInt32(Session["IdDocente"].ToString());
            using (EduApp_Entities db = new EduApp_Entities())
            {
                var competencias = db.COMPETENCIA.Select(x=>new SelectListItem { 
                    Value = x.ID_COMPETENCIA.ToString(),
                    Text = x.NOMBRE
                }).ToList();
                var cursos = db.CURSOS.Select(x => new SelectListItem
                {
                    Value = x.ID_CURSO.ToString(),
                    Text = x.NOMBRE
                }).ToList();

                var foro = db.ACTIVIDAD.Where(x => x.ID_ACTIVIDAD == id && x.ID_DOCENTE== IdDocente).SingleOrDefault();

                model.competencias = competencias;
                model.cursos = cursos;
                model.actividad = foro;
                return View("~/Views/Docente/Actividades/EditarForo.cshtml", model);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateForo(int? id, ACTIVIDAD model)
        {
            using (EduApp_Entities db = new EduApp_Entities())
            {
                var data = db.ACTIVIDAD.FirstOrDefault(x => x.ID_ACTIVIDAD == id);

                // Checking if any such record exist
                if (data != null)
                {
                    data.TITULO = model.TITULO;
                    data.FECHA_LIMITE = model.FECHA_LIMITE;
                    data.CURSOS.ID_CURSO = model.CURSOS.ID_CURSO;
                    data.DESCRIPCION = model.DESCRIPCION;
                    data.ARCHIVO = model.ARCHIVO;
                    data.COMPETENCIA.ID_COMPETENCIA = model.COMPETENCIA.ID_COMPETENCIA;

                    db.SaveChanges();

                    return RedirectToAction("Read");
                }
                else
                    return View();
            }
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteForo(int id)
        {
            EduApp_Entities db = new EduApp_Entities();

            try
            {
                var lectura = db.ACTIVIDAD.SingleOrDefault(b => b.ID_ACTIVIDAD == id);

                db.ACTIVIDAD.Remove(lectura);
                db.SaveChanges();

            }
            catch (Exception e)
            {
            }
            return View("~/Views/Docente/Actividades/Lecturas.cshtml");
        }


        public ActionResult VerForo(int id)
        {
            EduApp_Entities db = new EduApp_Entities();

            ACTIVIDAD actividad = db.ACTIVIDAD.Where(x => x.ID_ACTIVIDAD == id).FirstOrDefault();
            double valoracion = db.VALORACION_ACTIVIDAD.Where(x => x.ID_ACTIVIDAD == id).Select(x=>x.VALOR).ToList().Average();
            int cantidad = db.DETALLE_ACTIVIDAD.Where(x => x.ID_ACTIVIDAD == id).Count();
            List<DETALLE_ACTIVIDAD> detalle = db.DETALLE_ACTIVIDAD.Where(x => x.ID_ACTIVIDAD == id).OrderByDescending(x=>x.FECHA).ToList();
            string estado ;
            if (actividad.FECHA_LIMITE < DateTime.Now)
            {
                estado = "Finalizado";
            }
            else
            {
                estado = "Activo";
            }

            VerForoModel model = new VerForoModel();
            model.actividad = actividad;
            model.valoracion = valoracion;
            model.cantidad = cantidad;
            model.detalle = detalle;
            model.estado = estado;
            return View("~/Views/Docente/Actividades/VerForo.cshtml", model);
        }




        /*************Lectura*******************/
        public ActionResult ActividadesLectura()
        {
            EduApp_Entities db = new EduApp_Entities();
            int id= Convert.ToInt32(Session["IdDocente"].ToString());
            List<ACTIVIDAD> list = db.ACTIVIDAD.Where(x => x.ACTIVIDAD_TIPO == 3 && x.ID_DOCENTE==id).OrderByDescending(x => x.FECHA_CREACION).ToList();

            return View("~/Views/Docente/Actividades/Lecturas.cshtml", list);
        }
        //Agregar
        public ActionResult AddLectura()
        {
            EditarLecturaModel model = new EditarLecturaModel();

            using (EduApp_Entities db = new EduApp_Entities())
            {
                var competencias = db.COMPETENCIA.Select(x => new SelectListItem
                {
                    Value = x.ID_COMPETENCIA.ToString(),
                    Text = x.NOMBRE
                }).ToList().Take(3);
                var cursos = db.CURSOS.Select(x => new SelectListItem
                {
                    Value = x.ID_CURSO.ToString(),
                    Text = x.NOMBRE
                }).ToList();
                model.competencias = competencias;
                model.cursos = cursos;
            }

            return View("~/Views/Docente/Actividades/AddLectura.cshtml", model);
        }

        [HttpPost]
        public ActionResult AddLectura(ACTIVIDAD lectura)
        {
            try
            {
                using (EduApp_Entities db = new EduApp_Entities())
                {
                    db.ACTIVIDAD.Add(lectura);
                    db.SaveChanges();
                }
            }
            catch (Exception e)
            {

            }
            return View("~/Views/Docente/Actividades/AddLectura.cshtml");
        }


        //Editar
        public ActionResult EditLectura(int id)
        {
            EduApp_Entities db = new EduApp_Entities();

            ACTIVIDAD lectura = db.ACTIVIDAD.SingleOrDefault(x => x.ID_ACTIVIDAD == id);

            return View("~/Views/Docente/Actividades/EditLectura.cshtml", lectura);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateLectura(int? id, ACTIVIDAD model)
        {
            using (EduApp_Entities db = new EduApp_Entities())
            {
                var data = db.ACTIVIDAD.FirstOrDefault(x => x.ID_ACTIVIDAD == id);

                // Checking if any such record exist
                if (data != null)
                {
                    data.TITULO = model.TITULO;
                    data.FECHA_LIMITE = model.FECHA_LIMITE;
                    data.CURSOS.ID_CURSO = model.CURSOS.ID_CURSO;
                    data.DESCRIPCION = model.DESCRIPCION;
                    data.ARCHIVO = model.ARCHIVO;
                    data.COMPETENCIA.ID_COMPETENCIA = model.COMPETENCIA.ID_COMPETENCIA;

                    db.SaveChanges();

                    return RedirectToAction("Read");
                }
                else
                    return View();
            }
        }

        //Visualizar
        public ActionResult ShowLectura(int id)
        {

            EduApp_Entities db = new EduApp_Entities();

            ACTIVIDAD actividad = db.ACTIVIDAD.Where(x => x.ID_ACTIVIDAD == id).FirstOrDefault();
            double valoracion = db.VALORACION_ACTIVIDAD.Where(x => x.ID_ACTIVIDAD == id).Select(x => x.VALOR).ToList().Average();
            int cantidad = db.DETALLE_ACTIVIDAD.Where(x => x.ID_ACTIVIDAD == id).Count();
            List<DETALLE_ACTIVIDAD> detalle = db.DETALLE_ACTIVIDAD.Where(x => x.ID_ACTIVIDAD == id).OrderByDescending(x => x.FECHA).ToList();

            List<CUESTIONARIO> cuestionario = db.CUESTIONARIO.Where(x => x.ID_ACTIVIDAD == id).ToList();


            List<CALIFICACION>  calificacion= db.CALIFICACION.Where(x => x.DETALLE_ACTIVIDAD.ID_ACTIVIDAD == id).ToList();
            string estado;
            if (actividad.FECHA_LIMITE < DateTime.Now)
            {
                estado = "Finalizado";
            }
            else
            {
                estado = "Activo";
            }

           ShowLecturaModel model = new ShowLecturaModel();
            model.actividad = actividad;
            model.valoracion = valoracion;
            model.cantidad = cantidad;
            model.detalle = detalle;
            model.estado = estado;
            model.cuestionario = cuestionario;
            model.calificacion = calificacion;
            return View("~/Views/Docente/Actividades/ShowLectura.cshtml", model);
        }


        [HttpPost]
        public JsonResult GetAlumnoRepuestas(int IdAlumno, int IdActividad)
        {

            EduApp_Entities db = new EduApp_Entities();
            List<DETALLE_ACTIVIDAD> detalle = db.DETALLE_ACTIVIDAD.Where(x => x.ID_ALUMNO== IdAlumno && x.ID_ACTIVIDAD== IdActividad).ToList();            

            return Json(new { detalle }, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteLectura(int id)
        {
            EduApp_Entities db = new EduApp_Entities();

            try
            {
                var lectura = db.ACTIVIDAD.SingleOrDefault(b => b.ID_ACTIVIDAD == id);



                db.ACTIVIDAD.Remove(lectura);
                db.SaveChanges();

            }
            catch (Exception e)
            {
            }

            return View("~/Views/Docente/Actividades/Lecturas.cshtml");
        }




        /**********Test*******/
        public ActionResult ActividadesTest()
        {
            EduApp_Entities db = new EduApp_Entities();

            List<ACTIVIDAD> list = db.ACTIVIDAD.Where(x => x.ACTIVIDAD_TIPO == 2).ToList();
            return View("~/Views/Docente/Actividades/Test.cshtml", list);
        }

        public ActionResult EditTest(int? id)
        {
            using (EduApp_Entities db = new EduApp_Entities())
            {
                var test = db.ACTIVIDAD.Where(x => x.ID_ACTIVIDAD == id).SingleOrDefault();
                return View("~/Views/Docente/Actividades/EditarTest.cshtml", test);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateTest(int? id, ACTIVIDAD model)
        {
            using (EduApp_Entities db = new EduApp_Entities())
            {
                var data = db.ACTIVIDAD.FirstOrDefault(x => x.ID_ACTIVIDAD == id);

                // Checking if any such record exist
                if (data != null)
                {
                    data.TITULO = model.TITULO;
                    data.FECHA_LIMITE = model.FECHA_LIMITE;
                    data.CURSOS.ID_CURSO = model.CURSOS.ID_CURSO;
                    data.DESCRIPCION = model.DESCRIPCION;
                    data.ARCHIVO = model.ARCHIVO;
                    data.COMPETENCIA.ID_COMPETENCIA = model.COMPETENCIA.ID_COMPETENCIA;

                    db.SaveChanges();

                    return RedirectToAction("Read");
                }
                else
                    return View();
            }
        }


        public ActionResult ShowTest(int? id)
        {
            EduApp_Entities db = new EduApp_Entities();

            //Lectura
            ACTIVIDAD test = db.ACTIVIDAD.SingleOrDefault(x => x.ID_ACTIVIDAD == id);

            //Valoracion
            var valoracion = db.VALORACION_ACTIVIDAD.Where(x => x.ACTIVIDAD.ID_ACTIVIDAD == id).
                Select(s => s.VALOR);
            double promedio = Math.Abs(valoracion.Average());


            ActividadModel actividad = new ActividadModel()
            {
                actividad = test,
                valoracion = promedio
            };

            return View("~/Views/Docente/Actividades/VerTest.cshtml", actividad);
        }

        public ActionResult AddTest()
        {
            EditarLecturaModel model = new EditarLecturaModel();

            using (EduApp_Entities db = new EduApp_Entities())
            {
                var competencias = db.COMPETENCIA.Select(x => new SelectListItem
                {
                    Value = x.ID_COMPETENCIA.ToString(),
                    Text = x.NOMBRE
                }).ToList().Take(3);
                var cursos = db.CURSOS.Select(x => new SelectListItem
                {
                    Value = x.ID_CURSO.ToString(),
                    Text = x.NOMBRE
                }).ToList();
                model.competencias = competencias;
                model.cursos = cursos;
            }
            return View("~/Views/Docente/Actividades/AddTest.cshtml", model);
        }

        [HttpPost]
        public ActionResult AddTest(ACTIVIDAD test)
        {
            try
            {
                using (EduApp_Entities db = new EduApp_Entities())
                {
                    db.ACTIVIDAD.Add(test);
                    db.SaveChanges();
                }
            }
            catch (Exception e)
            {

            }
            return View("~/Views/Docente/Actividades/AddTest.cshtml");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteTest(int id)
        {
            EduApp_Entities db = new EduApp_Entities();

            try
            {
                var lectura = db.ACTIVIDAD.SingleOrDefault(b => b.ID_ACTIVIDAD == id);

                db.ACTIVIDAD.Remove(lectura);
                db.SaveChanges();

            }
            catch (Exception e)
            {
            }

            return View("~/Views/Docente/Actividades/Test.cshtml");
        }

    }
}