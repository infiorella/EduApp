﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace EduApp.Models.ViewModels
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class EduApp_Entities : DbContext
    {
        public EduApp_Entities()
            : base("name=EduApp_Entities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<ACTIVIDAD> ACTIVIDAD { get; set; }
        public virtual DbSet<ALUMNO> ALUMNO { get; set; }
        public virtual DbSet<CALIFICACION> CALIFICACION { get; set; }
        public virtual DbSet<CAPACIDADES> CAPACIDADES { get; set; }
        public virtual DbSet<COMPETENCIA> COMPETENCIA { get; set; }
        public virtual DbSet<COMPETENCIA_SECCION> COMPETENCIA_SECCION { get; set; }
        public virtual DbSet<CUESTIONARIO> CUESTIONARIO { get; set; }
        public virtual DbSet<CURSOS> CURSOS { get; set; }
        public virtual DbSet<DETALLE_ACTIVIDAD> DETALLE_ACTIVIDAD { get; set; }
        public virtual DbSet<DOCENTE> DOCENTE { get; set; }
        public virtual DbSet<GRADO_SECCION> GRADO_SECCION { get; set; }
        public virtual DbSet<ROL> ROL { get; set; }
        public virtual DbSet<TIPO_ACTIVIDAD> TIPO_ACTIVIDAD { get; set; }
        public virtual DbSet<USUARIO> USUARIO { get; set; }
        public virtual DbSet<VALORACION> VALORACION { get; set; }
        public virtual DbSet<VALORACION_ACTIVIDAD> VALORACION_ACTIVIDAD { get; set; }
    }
}
