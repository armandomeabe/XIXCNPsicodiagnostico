//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WordIO
{
    using System;
    using System.Collections.Generic;
    
    public partial class CMS_MENU
    {
        public int ID_MENU { get; set; }
        public System.DateTime UI_FECHA_ALTA { get; set; }
        public Nullable<System.DateTime> UI_FECHA_MODIFICACION { get; set; }
        public Nullable<System.DateTime> UI_FECHA_BAJA { get; set; }
        public string UI_USUARIO { get; set; }
        public string UI_INFO { get; set; }
        public Nullable<int> ID_MENUPADRE { get; set; }
        public string NOMBRE { get; set; }
        public string OBSERVACIONES { get; set; }
        public int ID_ARTICULO { get; set; }
    
        public virtual CMS_ARTICULO CMS_ARTICULO { get; set; }
    }
}