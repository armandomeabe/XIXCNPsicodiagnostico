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
    
    public partial class MesasDePonenciasLibres
    {
        public MesasDePonenciasLibres()
        {
            this.Work = new HashSet<Work>();
        }
    
        public int id { get; set; }
        public string Titulo { get; set; }
        public string Coordinadores { get; set; }
        public string DescripcionOpcional { get; set; }
    
        internal virtual ICollection<Work> Work { get; set; }
    }
}
