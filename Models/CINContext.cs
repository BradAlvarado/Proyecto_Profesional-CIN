using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Sistema_CIN.Models
{
    public partial class CINContext : DbContext
    {
        public CINContext()
        {
        }

        public CINContext(DbContextOptions<CINContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Encargado> Encargados { get; set; } = null!;
        public virtual DbSet<Modulo> Modulos { get; set; } = null!;
        public virtual DbSet<Permiso> Permisos { get; set; } = null!;
        public virtual DbSet<Personal> Personals { get; set; } = null!;
        public virtual DbSet<Pme> Pmes { get; set; } = null!;
        public virtual DbSet<Roles> Roles { get; set; } = null!;
        public virtual DbSet<Usuario> Usuarios { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
           
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Encargado>(entity =>
            {
                entity.HasKey(e => e.IdEncargado)
                    .HasName("PK__Encargad__770F28EABEB1715E");

                entity.Property(e => e.IdEncargado).HasColumnName("id_encargado");

                entity.Property(e => e.ApellidosE)
                    .HasMaxLength(60)
                    .IsUnicode(false)
                    .HasColumnName("apellidos_e");

                entity.Property(e => e.CedulaE)
                    .HasMaxLength(25)
                    .IsUnicode(false)
                    .HasColumnName("cedula_e");

                entity.Property(e => e.CorreoE)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("correo_e");

                entity.Property(e => e.DireccionE)
                    .HasMaxLength(60)
                    .IsUnicode(false)
                    .HasColumnName("direccion_e");

                entity.Property(e => e.Edad).HasColumnName("edad");

                entity.Property(e => e.FechaNaceE)
                    .HasColumnType("datetime")
                    .HasColumnName("fecha_nace_e");

                entity.Property(e => e.IdPme).HasColumnName("id_pme");

                entity.Property(e => e.LugarTrabajoE)
                    .HasMaxLength(25)
                    .IsUnicode(false)
                    .HasColumnName("lugar_trabajo_e");

                entity.Property(e => e.NombreE)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("nombre_e");

                entity.Property(e => e.ResponsableDe)
                    .HasMaxLength(25)
                    .IsUnicode(false)
                    .HasColumnName("responsable_de");

                entity.Property(e => e.TelefonoE)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("telefono_e");

                entity.HasOne(d => d.IdPmeNavigation)
                    .WithMany(p => p.Encargados)
                    .HasForeignKey(d => d.IdPme)
                    .HasConstraintName("FK_Pme");
            });

            modelBuilder.Entity<Modulo>(entity =>
            {
                entity.HasKey(e => e.IdModulo)
                    .HasName("PK__Modulos__B2584DFCAE42DB5E");

                entity.Property(e => e.IdModulo).HasColumnName("id_modulo");

                entity.Property(e => e.NombreModulo)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("nombre_modulo");
            });

            modelBuilder.Entity<Permiso>(entity =>
            {
                entity.HasKey(e => e.IdPermiso)
                    .HasName("PK__Permisos__228F224FA7705951");

                entity.Property(e => e.IdPermiso).HasColumnName("id_permiso");

                entity.Property(e => e.IdModulo).HasColumnName("id_modulo");

                entity.Property(e => e.IdUsuario).HasColumnName("id_usuario");

                entity.Property(e => e.Permitido).HasColumnName("permitido");

                entity.HasOne(d => d.IdModuloNavigation)
                    .WithMany(p => p.Permisos)
                    .HasForeignKey(d => d.IdModulo)
                    .HasConstraintName("FK_Modulos");

                entity.HasOne(d => d.IdUsuarioNavigation)
                    .WithMany(p => p.Permisos)
                    .HasForeignKey(d => d.IdUsuario)
                    .HasConstraintName("FK_Usuario");
            });

            modelBuilder.Entity<Personal>(entity =>
            {
                entity.HasKey(e => e.IdPersonal)
                    .HasName("PK__Personal__418FB8081CC9764B");

                entity.ToTable("Personal");

                entity.Property(e => e.IdPersonal).HasColumnName("id_personal");

                entity.Property(e => e.ApellidosP)
                    .HasMaxLength(60)
                    .IsUnicode(false)
                    .HasColumnName("apellidos_p");

                entity.Property(e => e.CantonP)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("canton_p");

                entity.Property(e => e.CedulaP)
                    .HasMaxLength(25)
                    .IsUnicode(false)
                    .HasColumnName("cedula_p");

                entity.Property(e => e.CorreoP)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("correo_p");

                entity.Property(e => e.DistritoP)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("distrito_p");

                entity.Property(e => e.EdadP).HasColumnName("edad_p");

                entity.Property(e => e.FechaNaceP)
                    .HasColumnType("datetime")
                    .HasColumnName("fecha_nace_p");

                entity.Property(e => e.GeneroP)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("genero_p");

                entity.Property(e => e.IdRol).HasColumnName("id_rol");

                entity.Property(e => e.NombreP)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("nombre_p");

                entity.Property(e => e.ProvinciaP)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("provincia_p");

                entity.Property(e => e.PuestoP)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("puesto_p");

                entity.HasOne(d => d.IdRolNavigation)
                    .WithMany(p => p.Personals)
                    .HasForeignKey(d => d.IdRol)
                    .HasConstraintName("FK_Rol");
            });

            modelBuilder.Entity<Pme>(entity =>
            {
                entity.HasKey(e => e.IdPme)
                    .HasName("PK__PME__6FC802365908AA76");

                entity.ToTable("PME");

                entity.Property(e => e.IdPme).HasColumnName("id_pme");

                entity.Property(e => e.ApellidosPme)
                    .HasMaxLength(60)
                    .IsUnicode(false)
                    .HasColumnName("apellidos_pme");

                entity.Property(e => e.CantonPme)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("canton_pme");

                entity.Property(e => e.CedulaPme)
                    .HasMaxLength(25)
                    .IsUnicode(false)
                    .HasColumnName("cedula_pme");

                entity.Property(e => e.CondiciónMigratoriaPme)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("condición_migratoria_pme");

                entity.Property(e => e.DistritoPme)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("distrito_pme");

                entity.Property(e => e.EdadPme).HasColumnName("edad_pme");

                entity.Property(e => e.EncargadoPme)
                    .HasMaxLength(25)
                    .IsUnicode(false)
                    .HasColumnName("encargado_pme");

                entity.Property(e => e.FechaEgresoPme)
                    .HasColumnType("datetime")
                    .HasColumnName("fecha_egreso_pme");

                entity.Property(e => e.FechaIngresoPme)
                    .HasColumnType("datetime")
                    .HasColumnName("fecha_ingreso_pme");

                entity.Property(e => e.FechaNacimientoPme)
                    .HasColumnType("date")
                    .HasColumnName("fecha_nacimiento_pme");

                entity.Property(e => e.GeneroPme)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("genero_pme");

                entity.Property(e => e.IdEncargado).HasColumnName("id_encargado");

                entity.Property(e => e.NacionalidadPme)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("nacionalidad_pme");

                entity.Property(e => e.NivelEducativoPme)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("nivel_educativo_pme");

                entity.Property(e => e.NombrePme)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("nombre_pme");

                entity.Property(e => e.PolizaSeguro)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("poliza_seguro");

                entity.Property(e => e.ProvinciaPme)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("provincia_pme");

                entity.Property(e => e.SubvencionPme).HasColumnName("subvencion_pme");

                entity.HasOne(d => d.IdEncargadoNavigation)
                    .WithMany(p => p.Pmes)
                    .HasForeignKey(d => d.IdEncargado)
                    .HasConstraintName("FK_Encargado");
            });

            modelBuilder.Entity<Roles>(entity =>
            {
                entity.HasKey(e => e.IdRol)
                    .HasName("PK__Roles__6ABCB5E05B6C6EEA");

                entity.Property(e => e.IdRol).HasColumnName("id_rol");

                entity.Property(e => e.NombreRol)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("nombre_rol");
            });

            modelBuilder.Entity<Usuario>(entity =>
            {
                entity.HasKey(e => e.IdUsuario)
                    .HasName("PK__Usuarios__4E3E04ADB4DC97B5");

                entity.Property(e => e.IdUsuario).HasColumnName("id_usuario");

                entity.Property(e => e.Clave)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("clave");

                entity.Property(e => e.CorreoU)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("correo_u");

                entity.Property(e => e.EstadoU).HasColumnName("estado_u");

                entity.Property(e => e.IdRol).HasColumnName("id_rol");

                entity.Property(e => e.ImagenU).HasColumnName("imagen_u");

                entity.Property(e => e.NombreRolU)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("nombre_rol_u");

                entity.Property(e => e.NombreU)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("nombre_u");

                entity.HasOne(d => d.IdRolNavigation)
                    .WithMany(p => p.Usuarios)
                    .HasForeignKey(d => d.IdRol)
                    .HasConstraintName("FK_NombreRol");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
