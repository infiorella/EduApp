using EduApp.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EduApp.Models.ViewModels
{
    public class AlumnoIndexModel
    {
        public ALUMNO alumno;
        public int actividadesRealizadas;
        public int actividadesPendientes;
        public int competenciasAlcanzadas;
        public int competenciasNoAlcanzadas;
        public List<CURSOS> cursos;
        public List<DETALLE_ACTIVIDAD> actividad;

        public AlumnoIndexModel(ALUMNO alumno, int actividadesRealizadas, int actividadesPendientes,
            int competenciasAlcanzadas,
            int competenciasNoAlcanzadas, List<CURSOS> cursos, List<DETALLE_ACTIVIDAD> actividad)
        {
            this.alumno = alumno;
            this.actividadesRealizadas = actividadesRealizadas;
            this.actividadesPendientes = actividadesPendientes;
            this.competenciasAlcanzadas = competenciasAlcanzadas;
            this.competenciasNoAlcanzadas = competenciasNoAlcanzadas;
            this.cursos = cursos;
            this.actividad = actividad;
        }
    }
}