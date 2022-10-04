using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EduApp.Models
{
    public class CompetenciaModel
    {
        public int Id_Competencia { get; set; }
        public string Nombre { get; set; }
        public string Detalle { get; set; }
        public string Progreso { get; set; }
        public int Id_Curso { get; set; }
        public int Id_Tipo_Competencia { get; set; }


        [Serializable]
        public class Tipo_Competencia
        {
            public int Id_Tipo_Competencia { get; set; }
            public string Nombre { get; set; }
            public string Descripcion { get; set; }
        }


    }
}