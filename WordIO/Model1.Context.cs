﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class XIXCNPsicodiagnosticoEntities : DbContext
    {
        public XIXCNPsicodiagnosticoEntities()
            : base("name=XIXCNPsicodiagnosticoEntities")
        {
            Accreditation = Set<Accreditation>();
            Work = Set<Work>();
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<C__MigrationHistory> C__MigrationHistory { get; set; }
        internal virtual DbSet<Accreditation> Accreditation { get; set; }
        public virtual DbSet<AspNetRoles> AspNetRoles { get; set; }
        public virtual DbSet<AspNetUserClaims> AspNetUserClaims { get; set; }
        public virtual DbSet<AspNetUserLogins> AspNetUserLogins { get; set; }
        public virtual DbSet<AspNetUsers> AspNetUsers { get; set; }
        public virtual DbSet<Author> Author { get; set; }
        public virtual DbSet<CMS_ARCHIVO> CMS_ARCHIVO { get; set; }
        public virtual DbSet<CMS_ARTICULO> CMS_ARTICULO { get; set; }
        public virtual DbSet<CMS_CATEGORIA> CMS_CATEGORIA { get; set; }
        public virtual DbSet<CMS_IMAGEN> CMS_IMAGEN { get; set; }
        public virtual DbSet<CMS_MENU> CMS_MENU { get; set; }
        public virtual DbSet<CMS_TIPOARCHIVO> CMS_TIPOARCHIVO { get; set; }
        public virtual DbSet<Establishment> Establishment { get; set; }
        public virtual DbSet<sysdiagrams> sysdiagrams { get; set; }
        internal virtual DbSet<Work> Work { get; set; }
        public virtual DbSet<WorkArea> WorkArea { get; set; }
        public virtual DbSet<WorkDocument> WorkDocument { get; set; }
        public virtual DbSet<WorkStatus> WorkStatus { get; set; }
        public virtual DbSet<MesasDePonenciasLibres> MesasDePonenciasLibres { get; set; }
    }
}