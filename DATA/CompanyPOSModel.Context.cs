﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------
 //     this.Configuration.LazyLoadingEnabled = false;
 //     this.Configuration.ProxyCreationEnabled = false;

namespace DATA
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class CompanyPOSEntities : DbContext
    {
        public CompanyPOSEntities()
            : base("name=CompanyPOSEntities")
        {
            this.Configuration.LazyLoadingEnabled = false;
            this.Configuration.ProxyCreationEnabled = false;
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Users> Users { get; set; }
        public virtual DbSet<Session> Session { get; set; }
        public virtual DbSet<Store> Store { get; set; }
        public virtual DbSet<UserActivity> UserActivity { get; set; }
        public virtual DbSet<Menu> Menu { get; set; }
        public virtual DbSet<ItemPagePosition> ItemPagePosition { get; set; }
        public virtual DbSet<Item> Item { get; set; }
        public virtual DbSet<Category> Category { get; set; }
        public virtual DbSet<ItemAttribute> ItemAttribute { get; set; }
    }
}
