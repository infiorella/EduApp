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
    
    public partial class CUESTIONARIO
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public CUESTIONARIO()
        {
            this.CALIFICACION = new HashSet<CALIFICACION>();
        }
    
        public int ID_CUESTIONARIO { get; set; }
        public string PREGUNTA { get; set; }
        public string RESPUESTA { get; set; }
        public string OPCIONES { get; set; }
        public Nullable<int> ID_ACTIVIDAD { get; set; }
    
        public virtual ACTIVIDAD ACTIVIDAD { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CALIFICACION> CALIFICACION { get; set; }
    }
}
