using EduApp.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EduApp.Models
{
    public class EditarLecturaModel
    {
        public ACTIVIDAD actividad { get; set; }
        public IEnumerable<SelectListItem> cursos { get; set; }
        public IEnumerable<SelectListItem> competencias { get; set; }
        
        
    }
}