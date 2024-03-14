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

        public virtual DbSet<EncargadoPme> EncargadoPmes { get; set; } = null!;
        public virtual DbSet<Modulo> Modulos { get; set; } = null!;
        public virtual DbSet<Permiso> Permisos { get; set; } = null!;
        public virtual DbSet<Personal> Personals { get; set; } = null!;
        public virtual DbSet<Pme> Pmes { get; set; } = null!;
        public virtual DbSet<Role> Roles { get; set; } = null!;
        public virtual DbSet<Usuario> Usuarios { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {

            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<EncargadoPme>(entity =>
            {
                entity.HasKey(e => e.CedulaE)
                    .HasName("PK__Encargad__BBA1521043550603");

                entity.ToTable("Encargado_PME");

                entity.Property(e => e.CedulaE)
                    .HasMaxLength(25)
                    .IsUnicode(false)
                    .HasColumnName("cedula_e");

                entity.Property(e => e.ApellidosE)
                    .HasMaxLength(60)
                    .IsUnicode(false)
                    .HasColumnName("apellidos_e");

                entity.Property(e => e.CorreoE)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("correo_e");

                entity.Property(e => e.DireccionE)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("direccion_e");

                entity.Property(e => e.EncargadoDeE)
                    .HasMaxLength(25)
                    .IsUnicode(false)
                    .HasColumnName("encargado_de_e");

                entity.Property(e => e.FechaNaceE)
                    .HasColumnType("date")
                    .HasColumnName("fecha_nace_e");

                entity.Property(e => e.LugarTrabajoE)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("lugar_trabajo_e");

                entity.Property(e => e.NombreE)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("nombre_e");

                entity.Property(e => e.TelefonoE)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("telefono_e");

                entity.HasOne(d => d.EncargadoDeENavigation)
                    .WithMany(p => p.EncargadoPmes)
                    .HasForeignKey(d => d.EncargadoDeE)
                    .HasConstraintName("FK_PME");
            });

            modelBuilder.Entity<Modulo>(entity =>
            {
                entity.HasKey(e => e.IdModulo)
                    .HasName("PK__Modulos__B2584DFC6276AD87");

                entity.Property(e => e.IdModulo).HasColumnName("id_modulo");

                entity.Property(e => e.NombreModulo)
                    .HasMaxLength(100)
                    .HasColumnName("nombre_modulo");
            });

            modelBuilder.Entity<Permiso>(entity =>
            {
                entity.HasKey(e => e.IdPermiso)
                    .HasName("PK__Permisos__228F224F679653A8");

                entity.Property(e => e.IdPermiso).HasColumnName("id_permiso");

                entity.Property(e => e.IdModulo).HasColumnName("id_modulo");

                entity.Property(e => e.IdUsuario).HasColumnName("id_usuario");

                entity.Property(e => e.Permitido).HasColumnName("permitido");

                entity.HasOne(d => d.IdModuloNavigation)
                    .WithMany(p => p.Permisos)
                    .HasForeignKey(d => d.IdModulo)
                    .HasConstraintName("FK__Permisos__id_mod__5812160E");

                entity.HasOne(d => d.IdUsuarioNavigation)
                    .WithMany(p => p.Permisos)
                    .HasForeignKey(d => d.IdUsuario)
                    .HasConstraintName("FK__Permisos__id_usu__571DF1D5");
            });

            modelBuilder.Entity<Personal>(entity =>
            {
                entity.HasKey(e => e.IdP)
                    .HasName("PK__Personal__9DB7D2E5913ED517");

                entity.ToTable("Personal");

                entity.Property(e => e.IdP)
                    .ValueGeneratedNever()
                    .HasColumnName("id_p");

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

                entity.Property(e => e.GeneroP)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("genero_p");

                entity.Property(e => e.IdRol)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("id_rol");

                entity.Property(e => e.NombreP)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("nombre_p");

                entity.Property(e => e.ProvinciaP)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("provincia_p");
            });

            modelBuilder.Entity<Pme>(entity =>
            {
                entity.HasKey(e => e.CedulaPme)
                    .HasName("PK__PME__F59B2E2A33CD710B");

                entity.ToTable("PME");

                entity.Property(e => e.CedulaPme)
                    .HasMaxLength(25)
                    .IsUnicode(false)
                    .HasColumnName("cedula_pme");

                entity.Property(e => e.ApellidosPme)
                    .HasMaxLength(60)
                    .IsUnicode(false)
                    .HasColumnName("apellidos_pme");

                entity.Property(e => e.CantonPme)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("canton_pme");

                entity.Property(e => e.CedulaE)
                    .HasMaxLength(25)
                    .IsUnicode(false)
                    .HasColumnName("cedula_e");

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
                    .HasColumnType("date")
                    .HasColumnName("fecha_egreso_pme");

                entity.Property(e => e.FechaIngresoPme)
                    .HasColumnType("date")
                    .HasColumnName("fecha_ingreso_pme");

                entity.Property(e => e.FechaNacimientoPme)
                    .HasColumnType("date")
                    .HasColumnName("fecha_nacimiento_pme");

                entity.Property(e => e.GeneroPme)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("genero_pme");

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

                entity.HasOne(d => d.EncargadoPmeNavigation)
                    .WithMany(p => p.Pmes)
                    .HasForeignKey(d => d.EncargadoPme)
                    .HasConstraintName("FK_Encargado");
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.HasKey(e => e.IdRol)
                    .HasName("PK__Roles__6ABCB5E0CB51A33F");

                entity.Property(e => e.IdRol).HasColumnName("id_rol");

                entity.Property(e => e.NombreRol)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("nombre_rol");
            });

            modelBuilder.Entity<Usuario>(entity =>
            {
                entity.HasKey(e => e.Id_Usuario)
                    .HasName("PK__Usuarios__4E3E04ADE9E86304");

                entity.Property(e => e.Id_Usuario).HasColumnName("id_usuario");

                entity.Property(e => e.Clave)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("clave");

                entity.Property(e => e.Correo_U)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("correo_u");

                entity.Property(e => e.Id_Rol).HasColumnName("id_rol");

                entity.Property(e => e.Imagen_U).HasColumnName("imagen_u");

                entity.Property(e => e.Nombre_U)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("nombre_u");

                entity.HasOne(d => d.IdRolNavigation)
                    .WithMany(p => p.Usuarios)
                    .HasForeignKey(d => d.Id_Rol)
                    .HasConstraintName("FK__Usuarios__id_rol__5441852A");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
