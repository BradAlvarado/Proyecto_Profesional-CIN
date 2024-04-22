using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Sistema_CIN.Models;

namespace Sistema_CIN.Data
{
    public partial class SistemaCIN_dbContext : DbContext
    {
        public SistemaCIN_dbContext()
        {
        }

        public SistemaCIN_dbContext(DbContextOptions<SistemaCIN_dbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<BitacoraIngresoSalidas> BitacoraIngresoSalida { get; set; } = null!;
        public virtual DbSet<BitacoraMovimiento> BitacoraMovimientos { get; set; } = null!;
        public virtual DbSet<Encargados> Encargados { get; set; } = null!;
        public virtual DbSet<Modulo> Modulos { get; set; } = null!;
        public virtual DbSet<Operaciones> Operaciones { get; set; } = null!;
        public virtual DbSet<Personal> Personals { get; set; } = null!;
        public virtual DbSet<Pme> Pmes { get; set; } = null!;
        public virtual DbSet<Rol> Rols { get; set; } = null!;
        public virtual DbSet<RolOperacion> RolOperacions { get; set; } = null!;
        public virtual DbSet<Usuario> Usuarios { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {

            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BitacoraIngresoSalidas>(entity =>
            {
                entity.HasKey(e => e.IdBitacora)
                    .HasName("PK__Bitacora__7E4268B027D1242F");

                entity.ToTable("Bitacora_ingreso_salida");

                entity.Property(e => e.IdBitacora).HasColumnName("id_bitacora");

                entity.Property(e => e.EstadoActual).HasColumnName("estado_actual");

                entity.Property(e => e.FechaIngreso)
                    .HasColumnType("datetime")
                    .HasColumnName("fecha_ingreso");

                entity.Property(e => e.FechaSalida)
                    .HasColumnType("datetime")
                    .HasColumnName("fecha_salida");

                entity.Property(e => e.UsuarioB)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("usuario_b");
            });

            modelBuilder.Entity<BitacoraMovimiento>(entity =>
            {
                entity.HasKey(e => e.IdBitacora)
                    .HasName("PK__Bitacora__7E4268B0A9DE718E");

                entity.ToTable("Bitacora_movimientos");

                entity.Property(e => e.IdBitacora).HasColumnName("id_bitacora");

                entity.Property(e => e.Detalle)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("detalle");

                entity.Property(e => e.FechaMovimiento)
                    .HasColumnType("datetime")
                    .HasColumnName("fecha_movimiento");

                entity.Property(e => e.TipoMovimiento)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("tipo_movimiento");

                entity.Property(e => e.UsuarioB)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("usuario_b");
            });

            modelBuilder.Entity<Encargados>(entity =>
            {
                entity.HasKey(e => e.IdEncargado)
                    .HasName("PK__Encargad__770F28EA5E419044");

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
                    .HasColumnType("date")
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

                entity.Property(e => e.TelefonoE)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("telefono_e");

                entity.HasOne(d => d.IdPmeNavigation)
                    .WithMany(p => p.Encargados)
                    .HasForeignKey(d => d.IdPme)
                    .HasConstraintName("FK__Encargado__id_pm__6383C8BA");
            });

            modelBuilder.Entity<Modulo>(entity =>
            {
                entity.HasKey(e => e.IdModulo)
                    .HasName("PK__Modulos__B2584DFC5C3EA48E");

                entity.Property(e => e.IdModulo).HasColumnName("id_modulo");

                entity.Property(e => e.NombreModulo)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("nombre_modulo");
            });

            modelBuilder.Entity<Operaciones>(entity =>
            {
                entity.HasKey(e => e.IdOp)
                    .HasName("PK__Operacio__0148BB6A1578D6D8");

                entity.Property(e => e.IdOp).HasColumnName("id_op");

                entity.Property(e => e.IdModulo).HasColumnName("id_modulo");

                entity.Property(e => e.NombreOp)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("nombre_op");

                entity.HasOne(d => d.IdModuloNavigation)
                    .WithMany(p => p.Operaciones)
                    .HasForeignKey(d => d.IdModulo)
                    .HasConstraintName("FK__Operacion__id_mo__5FB337D6");
            });

            modelBuilder.Entity<Personal>(entity =>
            {
                entity.HasKey(e => e.IdPersonal)
                    .HasName("PK__Personal__418FB8087A4B3E5E");

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
                    .HasColumnType("date")
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

                entity.Property(e => e.TelefonoP)
                    .HasMaxLength(12)
                    .IsUnicode(false)
                    .HasColumnName("telefono_p");

                entity.HasOne(d => d.IdRolNavigation)
                    .WithMany(p => p.Personals)
                    .HasForeignKey(d => d.IdRol)
                    .HasConstraintName("FK__Personal__id_rol__619B8048");
            });

            modelBuilder.Entity<Pme>(entity =>
            {
                entity.HasKey(e => e.IdPme)
                    .HasName("PK__PME__6FC802361894BBFA");

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

                entity.Property(e => e.FechaEgresoPme)
                    .HasColumnType("datetime")
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
                    .HasConstraintName("FK__PME__id_encargad__628FA481");
            });

            modelBuilder.Entity<Rol>(entity =>
            {
                entity.HasKey(e => e.IdRol)
                    .HasName("PK__Rol__6ABCB5E0EB387742");

                entity.ToTable("Rol");

                entity.Property(e => e.IdRol).HasColumnName("id_rol");

                entity.Property(e => e.NombreRol)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("nombre_rol");
            });

            modelBuilder.Entity<RolOperacion>(entity =>
            {
                entity.HasKey(e => e.IdRolOp)
                    .HasName("PK__Rol_oper__771ED49BBD3CB9C4");

                entity.ToTable("Rol_operacion");

                entity.Property(e => e.IdRolOp).HasColumnName("id_rol_op");

                entity.Property(e => e.IdOp).HasColumnName("id_op");

                entity.Property(e => e.IdRol).HasColumnName("id_rol");

                entity.HasOne(d => d.IdOpNavigation)
                    .WithMany(p => p.RolOperacions)
                    .HasForeignKey(d => d.IdOp)
                    .HasConstraintName("FK__Rol_opera__id_op__5EBF139D");

                entity.HasOne(d => d.IdRolNavigation)
                    .WithMany(p => p.RolOperacions)
                    .HasForeignKey(d => d.IdRol)
                    .HasConstraintName("FK__Rol_opera__id_ro__5DCAEF64");
            });

            modelBuilder.Entity<Usuario>(entity =>
            {
                entity.HasKey(e => e.IdUsuario)
                    .HasName("PK__Usuarios__4E3E04AD3A152FF1");

                entity.Property(e => e.IdUsuario).HasColumnName("id_usuario");

                entity.Property(e => e.AccesoU).HasColumnName("acceso_u");

                entity.Property(e => e.Clave)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("clave");

                entity.Property(e => e.CorreoU)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("correo_u");

                entity.Property(e => e.EstadoU).HasColumnName("estado_u");

                entity.Property(e => e.FotoU)
                    .IsUnicode(false)
                    .HasColumnName("foto_u");

                entity.Property(e => e.IdRol).HasColumnName("id_rol");

                entity.Property(e => e.NombreU)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("nombre_u");

                entity.HasOne(d => d.IdRolNavigation)
                    .WithMany(p => p.Usuarios)
                    .HasForeignKey(d => d.IdRol)
                    .HasConstraintName("FK__Usuarios__id_rol__5CD6CB2B");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
