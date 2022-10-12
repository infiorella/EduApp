//------------------------------------------------------------------------------
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
    using System.Collections.Generic;
    
    public partial class DOCENTE
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public DOCENTE()
        {
            this.ACTIVIDAD = new HashSet<ACTIVIDAD>();
            this.COMPETENCIA_SECCION = new HashSet<COMPETENCIA_SECCION>();
        }
    
        public int ID_DOCENTE { get; set; }
        public string NOMBRES { get; set; }
        public string APELLIDO_PATERNO { get; set; }
        public string APELLIDO_MATERNO { get; set; }
        public string DIRECCION { get; set; }
        public string TELEFONO { get; set; }
        public string CORREO { get; set; }
        public string ESPECIALIDAD { get; set; }
        public System.DateTime FECHA_NACIMIENTO { get; set; }
        public int ESTADO { get; set; }
        public int ID_USUARIO { get; set; }
        public int ID_GRADO_SECCION { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ACTIVIDAD> ACTIVIDAD { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<COMPETENCIA_SECCION> COMPETENCIA_SECCION { get; set; }
        public virtual GRADO_SECCION GRADO_SECCION { get; set; }
        public virtual USUARIO USUARIO { get; set; }
    }
}
