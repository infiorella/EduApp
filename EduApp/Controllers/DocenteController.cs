using EduApp.Models;
using EduApp.Models.ViewModels;
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
        public ActionResult Index(int id)
        {
            EduApp_BDEntities db = new EduApp_BDEntities();
            //DOCENTE
            DOCENTE docente= db.DOCENTE.SingleOrDefault(x => x.ID_DOCENTE == id);

            //ACTIVIDADES
            int actividadesCreadas= db.ACTIVIDAD.Where(x=> x.FECHA_CREACION.Month == DateTime.Now.Month).Count();
            int actividadesCompletadas = db.ACTIVIDAD.Where(x => x.ESTADO == 2 &&
                x.DETALLE_ACTIVIDAD.Where(e=>e.ID_ACTIVIDAD==id && e.NOTA=="A" || e.NOTA=="AD").Count()>
                0.5*(x.DETALLE_ACTIVIDAD.Where(e => e.ID_ACTIVIDAD == id).Count())).Count();

            //Estudiantes
            int estudiantes = db.ALUMNO.Where(x => x.ID_GRADO_SECCION == docente.ID_GRADO_SECCION).Count();

            //Competencias
            int competencias = db.COMPETENCIA.Where(x => x.CURSOS.ID_DOCENTE ==id).Count();

            //Ultima actividad
            ACTIVIDAD actividad = db.ACTIVIDAD.OrderByDescending(p => p.FECHA_CREACION).FirstOrDefault();

            //ALUMNOS
            List<ALUMNO> listaAlumnoAyuda = db.ALUMNO.Where(x => 
                x.DETALLE_ACTIVIDAD.Where(e=> e.NOTA=="C" && 
                e.ID_ACTIVIDAD==actividad.ID_ACTIVIDAD) 
                ==x.DETALLE_ACTIVIDAD).Take(4).ToList();

            return View("~/Views/Docente/Dashboard/Index.cshtml");
        }

        //Alumnos
        public ActionResult Alumnos()
        {
            List<AlumnoModel> list;
            using (EduApp_BDEntities db = new EduApp_BDEntities())
            {
                list = db.ALUMNO.
                    Select(s => new AlumnoModel() { Id_Alumno=s.ID_ALUMNO, Nombres = s.NOMBRES, Apellido_Paterno = s.APELLIDO_PATERNO, 
                    Apellido_Materno= s.APELLIDO_MATERNO, Fecha_Nacimiento=s.FECHA_NACIMIENTO, Direccion = s.DIRECCION 
                    , telefono= s.TELEFONO}).ToList();
            }

            return View("~/Views/Docente/Alumnos/Index.cshtml",list);
        }

        public ActionResult AlumnoDetalle(int id)
        {
            EduApp_BDEntities db = new EduApp_BDEntities();
            ALUMNO alumno = db.ALUMNO.SingleOrDefault(x => x.ID_ALUMNO == id);

            //Actividad
            List<ACTIVIDAD> actividad = db.ACTIVIDAD.Take(5).OrderByDescending(x => x.FECHA_ACTUALIZACION).ToList();
            List <DETALLE_ACTIVIDAD> detalleactividad = db.DETALLE_ACTIVIDAD.Take(5).OrderByDescending(x => x.FECHA).Where(x => x.ID_ALUMNO == id).ToList();

            var ViewModel = new AlumnoDetalleModel()
            {
                Alumno = alumno,
                Actividad = detalleactividad
            };

            return View("~/Views/Docente/Alumnos/AlumnoDetalle.cshtml", ViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddAlumno(ALUMNO alumno)
        {
            try
            {
                using (EduApp_BDEntities db = new EduApp_BDEntities())
                {
                    db.ALUMNO.Add(alumno);
                    db.SaveChanges();
                }
            }
            catch (Exception e)
            {

            }
            return View("~/Views/Docente/Actividades/AddForo.cshtml");
        }

        public ActionResult EditAlumno(int Id_Alumno, AlumnoModel alumno)
        {
            EduApp_BDEntities db = new EduApp_BDEntities();
            ALUMNO alumnoVM = db.ALUMNO.SingleOrDefault(x => x.ID_ALUMNO == Id_Alumno);


            //Detalles del alumno
            alumnoVM.ID_ALUMNO = alumno.Id_Alumno;
            alumnoVM.NOMBRES = alumno.Nombres;
            alumnoVM.APELLIDO_PATERNO= alumno.Apellido_Paterno ;
            alumnoVM.APELLIDO_MATERNO = alumno.Apellido_Materno;
            alumnoVM.DIRECCION = alumno.Direccion;
            alumnoVM.FECHA_NACIMIENTO = alumno.Fecha_Nacimiento;
            alumnoVM.TELEFONO = alumno.telefono;
            alumnoVM.ESTADO =alumno.Estado ;

            return PartialView("~/Views/Docente/Alumnos/EditAlumno.cshtml", alumno);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateAlumno(int? id, ALUMNO alumno)
        {
            using (EduApp_BDEntities db = new EduApp_BDEntities())
            {
                var data = db.ALUMNO.FirstOrDefault(x => x.ID_ALUMNO == id);

                // Checking if any such record exist
                if (data != null)
                {
                    data.NOMBRES= alumno.NOMBRES;
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
            return View("~/Views/Docente/Alumnos/Progreso.cshtml");
        }



        //Curso
        public ActionResult Cursos()
        {
            List<CURSOS> list;
            using (EduApp_BDEntities db = new EduApp_BDEntities())
            {
                list = db.CURSOS.
                    Select(s => new CURSOS()
                    {
                        NOMBRE = s.NOMBRE,
                        DESCRIPCION = s.DESCRIPCION,
                        HORAS = s.HORAS,
                        IMAGEN = s.IMAGEN,
                        NIVEL = s.NIVEL
                    }).ToList();
            }

            return View("~/Views/Docente/Cursos/Index.cshtml", list);
        }

        public ActionResult CursoDetalle(int id)
        {
            EduApp_BDEntities db = new EduApp_BDEntities();

            //APROBADOS
            int aprobado = db.DETALLE_ACTIVIDAD.Where(x => x.ACTIVIDAD.ID_CURSO == id && x.NOTA=="A" || x.NOTA=="AD")
                .GroupBy(x=>x.ACTIVIDAD.ID_ACTIVIDAD & x.ALUMNO.ID_ALUMNO).Count();

            //Desaprobados
            int desaprobados = db.DETALLE_ACTIVIDAD.Where(x => x.ACTIVIDAD.ID_CURSO == id && x.NOTA == "C" || x.NOTA == "B")
                .GroupBy(x => x.ACTIVIDAD.ID_ACTIVIDAD & x.ALUMNO.ID_ALUMNO).Count();

            CURSOS curso = db.CURSOS.SingleOrDefault(x => x.ID_CURSO == id);

            //Detalles de curso
            List<COMPETENCIA> listCompetencia = db.COMPETENCIA.OrderByDescending(x => x.NOMBRE).Where(x=> x.ID_CURSO==id).ToList();


            var ViewModel = new CursoModel()
            {
                curso = curso,
                cursoDetalle = new CursoDetalleModel(aprobado, desaprobados),
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
                using (EduApp_BDEntities db = new EduApp_BDEntities())
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
            EduApp_BDEntities db = new EduApp_BDEntities();

            CURSOS curso = db.CURSOS.SingleOrDefault(x => x.ID_CURSO == id);

            return PartialView("~/Views/Docente/Cursos/EditCurso.cshtml", curso);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateCurso(int? id, CURSOS curso)
        {
            using (EduApp_BDEntities db = new EduApp_BDEntities())
            {
                var data = db.CURSOS.FirstOrDefault(x => x.ID_CURSO== id);

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
            EduApp_BDEntities db = new EduApp_BDEntities();

           List<COMPETENCIA> list= db.COMPETENCIA.ToList();

            return View("~/Views/Docente/Cursos/Competencias.cshtml", list);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddCompetencia(COMPETENCIA competencia)
        {
            try
            {
                using (EduApp_BDEntities db = new EduApp_BDEntities())
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
            EduApp_BDEntities db = new EduApp_BDEntities();

            COMPETENCIA competencia = db.COMPETENCIA.SingleOrDefault(x => x.ID_COMPETENCIA == id);

            return PartialView("~/Views/Docente/Cursos/EditCompetencia.cshtml", competencia);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateCompetencia(int? id, COMPETENCIA competencia)
        {
            using (EduApp_BDEntities db = new EduApp_BDEntities())
            {
                var data = db.COMPETENCIA.FirstOrDefault(x => x.ID_COMPETENCIA == id);

                // Checking if any such record exist
                if (data != null)
                {
                    data.NOMBRE = competencia.NOMBRE;
                    data.ID_TIPO_COMPETENCIA = competencia.ID_TIPO_COMPETENCIA;
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
        public ActionResult InformesActividades(int id)
        {
            EduApp_BDEntities db = new EduApp_BDEntities();


             int cantidadCreadas= db.ACTIVIDAD.Where(x=>x.CURSOS.DOCENTE.ID_DOCENTE==id).Count();
            int cantidadCompletadas= db.ACTIVIDAD.Where(x => x.CURSOS.DOCENTE.ID_DOCENTE == id && x.ESTADO==2).Count();
            /*int actividadesLogroAlcanzado =  db.DETALLE_ACTIVIDAD.
                Where(e => e.NOTA == "A" || e.NOTA=="AD"
                && e.ACTIVIDAD.CURSOS.DOCENTE.ID_DOCENTE == id && 
                e.ACTIVIDAD.ESTADO == 2).Count();*/
            int actividadesLogroAlcanzado = db.ACTIVIDAD.Where(x => x.ESTADO == 2 &&
                x.DETALLE_ACTIVIDAD.Where(e => e.ID_ACTIVIDAD == id && e.NOTA == "A" || e.NOTA == "AD").Count() >
                0.5 * (x.DETALLE_ACTIVIDAD.Where(e => e.ID_ACTIVIDAD == id).Count())).Count();
            int actividadesSinLogroAlcanzado= db.ACTIVIDAD.Where(x => x.ESTADO == 2 &&
                x.DETALLE_ACTIVIDAD.Where(e => e.ID_ACTIVIDAD == id && e.NOTA == "C" || e.NOTA == "B").Count() >
                0.5 * (x.DETALLE_ACTIVIDAD.Where(e => e.ID_ACTIVIDAD == id).Count())).Count(); ;



            List<Tuple<string, int>> actividadesSegunMes;
            List<Tuple<string, string, int>> actividadesCalificacion;

            InformesModel informesactividades = new InformesModel();



            return View("~/Views/Docente/Informes/Actividades.cshtml");
        }
        public ActionResult InformesAvance()
        {
            return View("~/Views/Docente/Informes/Avance.cshtml");
        }





        /*******************foros***************/        
        public ActionResult ActividadesForos()
        {
            EduApp_BDEntities db = new EduApp_BDEntities();

            List<ACTIVIDAD> list = db.ACTIVIDAD.Where(x=> x.ACTIVIDAD_TIPO==2).ToList();

            return View("~/Views/Docente/Actividades/Foros.cshtml", list);
        }

        
        //Añdir
        public ActionResult AddForo()
        {           
            return View("~/Views/Docente/Actividades/AddForo.cshtml");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddForo(ACTIVIDAD actividad)
        {
            try
            {
                using (EduApp_BDEntities db = new EduApp_BDEntities())
                {
                    db.ACTIVIDAD.Add(actividad);
                    db.SaveChanges();
                }
            } catch(Exception e)
            {

            }
            return View("~/Views/Docente/Actividades/AddForo.cshtml");
        }



        //Editar
        public ActionResult EditForo(int? id)
        {
            using (EduApp_BDEntities db = new EduApp_BDEntities())
            {
                var foro = db.ACTIVIDAD.Where(x => x.ID_ACTIVIDAD == id).SingleOrDefault();
                return View("~/Views/Docente/Actividades/EditarForo.cshtml", foro);
            }            
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateForo(int? id, ACTIVIDAD model)
        {
            using (EduApp_BDEntities db = new EduApp_BDEntities())
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
            EduApp_BDEntities db = new EduApp_BDEntities();

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


        public ActionResult VerForo()
        {
            EduApp_BDEntities db = new EduApp_BDEntities();

            List<ACTIVIDAD> list = db.ACTIVIDAD.Where(x => x.ACTIVIDAD_TIPO == 2).ToList();
            return View("~/Views/Docente/Actividades/VerForo.cshtml");
        }




        /*************Lectura*******************/
        public ActionResult ActividadesLectura()
        {
            EduApp_BDEntities db = new EduApp_BDEntities();

            List<ACTIVIDAD> list = db.ACTIVIDAD.Where(x => x.ACTIVIDAD_TIPO == 3).OrderByDescending(x=> x.FECHA_CREACION).ToList();

            return View("~/Views/Docente/Actividades/Lecturas.cshtml", list);
        }
        //Agregar
        public ActionResult AddLectura()
        {
            return View("~/Views/Docente/Actividades/AddLectura.cshtml");
        }

        [HttpPost]
        public ActionResult AddLectura(ACTIVIDAD lectura)
        {
            try
            {
                using (EduApp_BDEntities db = new EduApp_BDEntities())
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
            EduApp_BDEntities db = new EduApp_BDEntities();

            ACTIVIDAD lectura = db.ACTIVIDAD.SingleOrDefault(x => x.ID_ACTIVIDAD == id);

            return View("~/Views/Docente/Actividades/EditLectura.cshtml", lectura);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateLectura(int? id, ACTIVIDAD model)
        {
            using (EduApp_BDEntities db = new EduApp_BDEntities())
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
        public ActionResult ShowLectura(int? id)
        {
            EduApp_BDEntities db = new EduApp_BDEntities();

            //Lectura
            ACTIVIDAD lectura = db.ACTIVIDAD.SingleOrDefault(x => x.ID_ACTIVIDAD == id);

            //Valoracion
            var valoracion = db.VALORACION.Where(x=> x.ACTIVIDAD.ID_ACTIVIDAD== id).
                Select( s => s.ACTIVIDAD_VALOR);
            double promedio=valoracion.Average();

            
            ActividadModel actividad = new ActividadModel()
            {
                actividad = lectura,
                valoracion = promedio
            };
            return View("~/Views/Docente/Actividades/ShowLectura.cshtml", actividad);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteLectura(int id)
        {
            EduApp_BDEntities db = new EduApp_BDEntities();

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
            EduApp_BDEntities db = new EduApp_BDEntities();

            List<ACTIVIDAD> list = db.ACTIVIDAD.Where(x => x.ACTIVIDAD_TIPO == 1).ToList();
            return View("~/Views/Docente/Actividades/Test.cshtml", list);
        }

        public ActionResult EditTest(int? id)
        {
            using (EduApp_BDEntities db = new EduApp_BDEntities())
            {
                var test = db.ACTIVIDAD.Where(x => x.ID_ACTIVIDAD == id).SingleOrDefault();
                return View("~/Views/Docente/Actividades/EditarTest.cshtml", test);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateTest(int? id, ACTIVIDAD model)
        {
            using (EduApp_BDEntities db = new EduApp_BDEntities())
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
            EduApp_BDEntities db = new EduApp_BDEntities();

            //Lectura
            ACTIVIDAD lectura = db.ACTIVIDAD.SingleOrDefault(x => x.ID_ACTIVIDAD == id);

            //Valoracion
            var valoracion = db.VALORACION.Where(x => x.ACTIVIDAD.ID_ACTIVIDAD == id).
                Select(s => s.ACTIVIDAD_VALOR);
            double promedio = valoracion.Average();


            ActividadModel actividad = new ActividadModel()
            {
                actividad = lectura,
                valoracion = promedio
            };

            return View("~/Views/Docente/Actividades/VerTest.cshtml", actividad);
        }

        public ActionResult AddTest()
        {
            return View("~/Views/Docente/Actividades/AddTest.cshtml");
        }

        [HttpPost]
        public ActionResult AddTest(ACTIVIDAD test)
        {
            try
            {
                using (EduApp_BDEntities db = new EduApp_BDEntities())
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
            EduApp_BDEntities db = new EduApp_BDEntities();

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