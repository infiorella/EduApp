using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EduApp.Models.ViewModels
{
    public class AlumnoDetalleModel
    {
        public ALUMNO Alumno { get; set; }

        //ACTIVIDADES
        public List<DETALLE_ACTIVIDAD> Actividad { get; set; }

        //COMPETENCIAS
        public COMPETENCIA Competencia { get; set; }


        public List<CURSOS> cursos { get; set; }

        public int  cantidadActividades { get; set; }
        public int cursosSinLogroAlcanzado { get; set; }

        public List<DETALLE_ACTIVIDAD> TodasActividades { get; set; }
    }
}