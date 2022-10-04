using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EduApp.Models
{
    public class UsuarioModel
    {
        [Display(Name = "Id Usuario")]
        [Range(10000000, 99999999, ErrorMessage ="Ingresa un dni valido")]
        public int Id_Usuario { get; set; }
        public string Usuario { get; set; }
        public string Contraseña { get; set; }
        public int Estado { get; set; }
        public DateTime? Fecha_Creacion { get; set; }
        public DateTime? Fecha_Actualizacion { get; set; }
        public int Id_Rol { get; set; }

        //public Int32?

        [Serializable]
        public class Rol
        {
            public int Id_Rol { get; set; }
            public string Nombre { get; set; }
        }
    }
}