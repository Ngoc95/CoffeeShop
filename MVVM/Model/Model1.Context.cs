﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace QuanLiCoffeeShop.MVVM.Model
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class CoffeeShopDBEntities : DbContext
    {
        public CoffeeShopDBEntities()
            : base("name=CoffeeShopDBEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<C_TABLE> C_TABLE { get; set; }
        public virtual DbSet<BILL> BILLs { get; set; }
        public virtual DbSet<BILL_INFO> BILL_INFO { get; set; }
        public virtual DbSet<CUSTOMER> CUSTOMERs { get; set; }
        public virtual DbSet<EMPLOYEE> EMPLOYEEs { get; set; }
        public virtual DbSet<EMPLOYEE_SHIFT> EMPLOYEE_SHIFT { get; set; }
        public virtual DbSet<ERROR> ERRORs { get; set; }
        public virtual DbSet<EXPORT> EXPORTs { get; set; }
        public virtual DbSet<EXPORT_INFO> EXPORT_INFO { get; set; }
        public virtual DbSet<GENRE_PRODUCT> GENRE_PRODUCT { get; set; }
        public virtual DbSet<GENRE_TABLE> GENRE_TABLE { get; set; }
        public virtual DbSet<IMPORT> IMPORTs { get; set; }
        public virtual DbSet<IMPORT_INFO> IMPORT_INFO { get; set; }
        public virtual DbSet<INGREDIENT> INGREDIENTs { get; set; }
        public virtual DbSet<PRODUCT> PRODUCTs { get; set; }
        public virtual DbSet<REQUEST> REQUESTs { get; set; }
        public virtual DbSet<RESERVATION> RESERVATIONs { get; set; }
        public virtual DbSet<SUPPLIER> SUPPLIERs { get; set; }
        public virtual DbSet<WORK_SHIFT> WORK_SHIFT { get; set; }
    }
}
