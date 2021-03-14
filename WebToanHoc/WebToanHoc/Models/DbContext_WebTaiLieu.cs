namespace WebToanHoc.Models
{
     using System;
     using System.Data.Entity;
     using System.ComponentModel.DataAnnotations.Schema;
     using System.Linq;

     public partial class DbContext_WebTaiLieu : DbContext
     {
          public DbContext_WebTaiLieu()
              : base("name=DbContext_WebTaiLieu")
          {
          }

          public virtual DbSet<sysdiagram> sysdiagrams { get; set; }
          public virtual DbSet<tbl_category> tbl_category { get; set; }
          public virtual DbSet<tbl_description> tbl_description { get; set; }
          public virtual DbSet<tbl_file> tbl_file { get; set; }
          public virtual DbSet<tbl_subject> tbl_subject { get; set; }
          public virtual DbSet<tbl_user> tbl_user { get; set; }

          protected override void OnModelCreating(DbModelBuilder modelBuilder)
          {
               modelBuilder.Entity<tbl_user>()
                   .Property(e => e.username)
                   .IsUnicode(false);

               modelBuilder.Entity<tbl_user>()
                   .Property(e => e.password)
                   .IsUnicode(false);
          }
     }
}
