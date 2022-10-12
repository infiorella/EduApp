using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EduApp.Models.ViewModels
{
    public class ProgresoAlumnosModel
    {
        //public List<COMPETENCIA> competencias { get; set; } 
        public string dni { get; set; }
        public string nombrecompleto { get; set; }
        public List<string> notas { get; set; }


        public ProgresoAlumnosModel(string dni, string nombrecompleto, List<string> notas)
        {
            this.dni = dni;
            this.nombrecompleto = nombrecompleto;
            this.notas = notas;
        }
    }
}