using EduApp.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EduApp.Models
{
    public class VerForoModel
    {
        public ACTIVIDAD actividad { get; set; }
        public double valoracion { get; set; }
        public int cantidad { get; set; }
        public string estado { get; set; }
        public List<DETALLE_ACTIVIDAD> detalle { get; set; }

        //public CALIFICACION calificacion;
        //public CUESTIONARIO cuestionario;
    }
}