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

        public virtual DbSet<Personal> Personals { get; set; } = null!;
        public virtual DbSet<Pme> Pmes { get; set; } = null!;
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
            modelBuilder.Entity<Personal>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("personal");

                entity.Property(e => e.ApellidosPersonal)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("apellidos_personal")
                    .IsFixedLength();

                entity.Property(e => e.CedulaPersonal)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("cedula_personal")
                    .IsFixedLength();

                entity.Property(e => e.CorreoPersonal)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("correo_personal")
                    .IsFixedLength();

                entity.Property(e => e.DireccionCantonPersonal)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("direccion_canton_personal")
                    .IsFixedLength();

                entity.Property(e => e.DireccionDistritoPersonal)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("direccion_distrito_personal")
                    .IsFixedLength();

                entity.Property(e => e.DireccionProvinciaPersonal)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("direccion_provincia_personal")
                    .IsFixedLength();

                entity.Property(e => e.FechaNacimientoPersonal)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("fecha_nacimiento_personal")
                    .IsFixedLength();

                entity.Property(e => e.GeneroPersonal)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("genero_personal")
                    .IsFixedLength();

                entity.Property(e => e.NombrePersonal)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("nombre_personal")
                    .IsFixedLength();

                entity.Property(e => e.OtrasSenasPersonal)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("otras_senas_personal")
                    .IsFixedLength();

                entity.Property(e => e.PuestoPersonal)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("puesto_personal")
                    .IsFixedLength();

                entity.Property(e => e.TelefonoPersonal)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("telefono_personal")
                    .IsFixedLength();
            });

            modelBuilder.Entity<Pme>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("pme");

                entity.Property(e => e.ApellidosPme)
                    .HasMaxLength(10)
                    .HasColumnName("apellidos_pme")
                    .IsFixedLength();

                entity.Property(e => e.CedulaPme)
                    .HasMaxLength(10)
                    .HasColumnName("cedula_pme")
                    .IsFixedLength();

                entity.Property(e => e.CondicionMigratoriaPme)
                    .HasMaxLength(10)
                    .HasColumnName("condicion_migratoria_pme")
                    .IsFixedLength();

                entity.Property(e => e.DireccionCantonPme)
                    .HasMaxLength(10)
                    .HasColumnName("direccion_canton_pme")
                    .IsFixedLength();

                entity.Property(e => e.DireccionDistritoPme)
                    .HasMaxLength(10)
                    .HasColumnName("direccion_distrito_pme")
                    .IsFixedLength();

                entity.Property(e => e.DireccionProvinciaPme)
                    .HasMaxLength(10)
                    .HasColumnName("direccion_provincia_pme")
                    .IsFixedLength();

                entity.Property(e => e.EdadPme)
                    .HasMaxLength(10)
                    .HasColumnName("edad_pme")
                    .IsFixedLength();

                entity.Property(e => e.FechaEgresoPme)
                    .HasMaxLength(10)
                    .HasColumnName("fecha_egreso_pme")
                    .IsFixedLength();

                entity.Property(e => e.FechaIngresoPme)
                    .HasMaxLength(10)
                    .HasColumnName("fecha_ingreso_pme")
                    .IsFixedLength();

                entity.Property(e => e.FechaNaciemientoPme)
                    .HasMaxLength(10)
                    .HasColumnName("fecha_naciemiento_pme")
                    .IsFixedLength();

                entity.Property(e => e.GeneroPme)
                    .HasMaxLength(10)
                    .HasColumnName("genero_pme")
                    .IsFixedLength();

                entity.Property(e => e.NacionalidadPme)
                    .HasMaxLength(10)
                    .HasColumnName("nacionalidad_pme")
                    .IsFixedLength();

                entity.Property(e => e.NivelEducativoPme)
                    .HasMaxLength(10)
                    .HasColumnName("nivel_educativo_pme")
                    .IsFixedLength();

                entity.Property(e => e.NombrePme)
                    .HasMaxLength(10)
                    .HasColumnName("nombre_pme")
                    .IsFixedLength();

                entity.Property(e => e.PolizaSeguroPme)
                    .HasMaxLength(10)
                    .HasColumnName("poliza_seguro_pme")
                    .IsFixedLength();

                entity.Property(e => e.RecibeSubvencionPme)
                    .HasMaxLength(10)
                    .HasColumnName("recibe_subvencion_pme")
                    .IsFixedLength();
            });

            modelBuilder.Entity<Usuario>(entity =>
            {
                entity.HasKey(e => e.Correo);

                entity.ToTable("usuarios");

                entity.Property(e => e.Correo)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("correo")
                    .IsFixedLength();

                entity.Property(e => e.Clave)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("clave")
                    .IsFixedLength();

                entity.Property(e => e.NombreUsuario)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("nombre_usuario")
                    .IsFixedLength();
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
