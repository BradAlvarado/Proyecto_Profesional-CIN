using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Sistema_CIN.Models
{
    public partial class CIN_dbContext : DbContext
    {
        public CIN_dbContext()
        {
        }

        public CIN_dbContext(DbContextOptions<CIN_dbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Encargado> Encargados { get; set; } = null!;
        public virtual DbSet<Role> Roles { get; set; } = null!;
        public virtual DbSet<Usuario> Usuarios { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=db-cin.ct8cke88yhne.us-east-2.rds.amazonaws.com,1433; Database=CIN_db ;User Id=administrador;Password=admin123!; integrated security=true; Trusted_Connection=False; TrustServerCertificate=Yes;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Encargado>(entity =>
            {
                entity.HasKey(e => e.cedula_e)
                    .HasName("PK__Encargad__BBA1521016A4489F");

                entity.Property(e => e.cedula_e)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.apellidos_e)
                    .HasMaxLength(60)
                    .IsUnicode(false);

                entity.Property(e => e.correo_e)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.direccion_e)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.lugar_trabajo_e)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.nombre_e)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.responsable_de)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.telefono_e)
                    .HasMaxLength(10)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.HasKey(e => e.id_rol)
                    .HasName("PK__Roles__6ABCB5E0A977ED4D");

                entity.Property(e => e.nombre_Rol)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Usuario>(entity =>
            {
                entity.HasKey(e => e.id_usuario)
                    .HasName("PK__Usuarios__4E3E04AD8C05001A");

                entity.Property(e => e.clave)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.correo_u)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.nombre_u)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
