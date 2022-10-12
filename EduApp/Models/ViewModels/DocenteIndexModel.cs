using EduApp.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EduApp.Models
{
    public class DocenteIndexModel
    {
        public DOCENTE docente;
        public int actividadesCreadas;
        public int actividadesCompletadas;
        public int estudiantes;
        public int competencias;
        public List<CURSOS> cursos;
        public ACTIVIDAD actividad;
        public List<ALUMNO> alumnosayuda;
        public List<ACTIVIDAD> ultimasactividades;
        public List<TIPO_ACTIVIDAD> ListActividades;

        public DocenteIndexModel(DOCENTE docente, int actividadesCreadas, int actividadesCompletadas, int estudiantes,
            int competencias, List<CURSOS> cursos, ACTIVIDAD actividad, List<ALUMNO> alumnosayuda, 
            List<ACTIVIDAD> ultimasactividades, List<TIPO_ACTIVIDAD> ListActividades)
        {
            this.docente = docente;
            this.actividadesCreadas = actividadesCreadas;
            this.actividadesCompletadas = actividadesCompletadas;
            this.estudiantes = estudiantes;
            this.competencias = competencias;
            this.cursos = cursos;
            this.actividad = actividad;
            this.alumnosayuda = alumnosayuda;
            this.ultimasactividades = ultimasactividades;
            this.ListActividades = ListActividades;
        }
    }
}