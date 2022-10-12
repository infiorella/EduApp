using EduApp.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EduApp.Models
{
    public class CursoModel
    {
        public CURSOS curso;

        public List<COMPETENCIA> listCompetencia { get; set; }

        public List<int> progresoCompetencias { get; set; }
        public int aprobado { get; set; }
        public int desaprobado { get; set; }


    }

}