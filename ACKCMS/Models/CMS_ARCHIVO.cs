//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ACKCMS.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class CMS_ARCHIVO
    {
        public CMS_ARCHIVO()
        {
            this.CMS_ARTICULO = new HashSet<CMS_ARTICULO>();
        }
    
        public int ID_ARCHIVO { get; set; }
        public string Nombre { get; set; }
        public string RelativePath { get; set; }
        public int ID_TIPO { get; set; }
    
        public virtual CMS_TIPOARCHIVO CMS_TIPOARCHIVO { get; set; }
        public virtual ICollection<CMS_ARTICULO> CMS_ARTICULO { get; set; }
    }
}
