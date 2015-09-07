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
    
    internal partial class Accreditation
    {
        public Accreditation()
        {
            this.Work = new HashSet<Work>();
        }
    
        public int id { get; set; }
        public string Apellido { get; set; }
        public string Nombre { get; set; }
        public string Profesion { get; set; }
        public string Pais { get; set; }
        public string Provincia { get; set; }
        public string Localidad { get; set; }
        public string Direccion { get; set; }
        public string Telefono { get; set; }
        public string Movil { get; set; }
        public string NumeroMatricula { get; set; }
        public string NumeroSocio { get; set; }
        public string EleccionDePago { get; set; }
        public bool AcreditacionRealizada { get; set; }
        public string AcreditacionComprobanteNro { get; set; }
        public string Email { get; set; }
        public string DNI { get; set; }
        public Nullable<System.DateTime> FechaAcreditacion { get; set; }
        public Nullable<int> CantTrabajosPresenta { get; set; }
        public string InstitucionALaQuePertenece { get; set; }
        public byte[] ComprobanteBinaryArr { get; set; }
    
        internal virtual ICollection<Work> Work { get; set; }
    }
}