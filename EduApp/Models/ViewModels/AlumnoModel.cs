using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EduApp.Models
{
    public class AlumnoModel
    {

        public int Id_Alumno { get; set; }
        public string Nombres { get; set; }
        public string Apellido_Paterno { get; set; }
        public string Apellido_Materno { get; set; }
        public DateTime Fecha_Nacimiento { get; set; }
        public string Direccion { get; set; }
        public string telefono { get; set; }
        public int Estado { get; set; }
        public string sexo { get; set; }
        public int Alumno_Usuario { get; set; }
        public int Id_Grado_Seccion {get; set;}

        
    }
}