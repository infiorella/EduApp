using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EduApp.Models.ViewModels
{
    public class InformesAvanceModel
    {
        List<COMPETENCIA> listCompetencias { get; set; }

        public InformesAvanceModel(List<COMPETENCIA> listCompetencias)
        {
            this.listCompetencias = listCompetencias;
        }
    }
}