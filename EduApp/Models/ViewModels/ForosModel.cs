using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EduApp.Models.ViewModels
{
    public class ForosModel
    {
        public ACTIVIDAD actividad;
        public double valoracion;

        public ForosModel(ACTIVIDAD actividad, double valoracion)
        {
            this.actividad = actividad;
            this.valoracion = valoracion;
        }
    }
}