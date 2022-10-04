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
        public CursoDetalleModel cursoDetalle;

        public List<COMPETENCIA> listCompetencia { get; set; }


        public class CursoDetalleModel {
            public int CantidadAprobado { get; set; }
            public int CantidadDesaprobado { get; set; }


            public CursoDetalleModel(int CantidadAprobados, int CantidadDesaprobados)
            {
                CantidadAprobado = CantidadAprobados;
                CantidadDesaprobado = CantidadDesaprobados;
            }
        }

    }
}