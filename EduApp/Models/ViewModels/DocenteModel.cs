using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EduApp.Models
{
    public class DocenteModel
    {
        public Int32 Id_Docente { get; set; }
        public string Nombres { get; set; }
        public string Apellido_Paterno { get; set; }
        public string Apellido_Materno { get; set;}
        public string Direccion { get; set; }
        public string Telefono { get; set; }
        public string Correo { get; set; }
        public string Especialidad { get; set; }
        public DateTime Fecha_Nacimiento { get; set; }
        public Int32 Estado { get; set; }
        public Int32 Id_Usuario { get; set; }
    }
}