using EduApp.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EduApp.Models
{
    public class InformesModel
    {
        public int cantidadCreadas;
        public int cantidadCompletadas;
        public int actividadesLogroAlcanzado;
        public int actividadesSinLogroAlcanzado;


        public List<Tuple<string, int>> actividadesSegunMes;
        public List<Tuple<string, string, int>> actividadesCalificacion;
        //public CALIFICACION calificacion;
        //public CUESTIONARIO cuestionario;
    }
}