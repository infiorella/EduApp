using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EduApp.Models
{
    public class InformesActividadesModel
    {
        public int cantidadCreadas;
        public int cantidadCompletadas;
        public int actividadesLogroAlcanzado;
        public int actividadesSinLogroAlcanzado;


        public InformesActividadesModel(int cantidadCreadas, int cantidadCompletadas, int actividadesLogroAlcanzado, int actividadesSinLogroAlcanzado)
        {
            this.cantidadCreadas = cantidadCreadas;
            this.cantidadCompletadas = cantidadCompletadas;
            this.actividadesLogroAlcanzado = actividadesLogroAlcanzado;
            this.actividadesSinLogroAlcanzado = actividadesSinLogroAlcanzado;

        }
    }
}