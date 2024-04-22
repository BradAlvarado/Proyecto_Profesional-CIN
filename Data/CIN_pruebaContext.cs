using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

using Sistema_CIN.Models;

namespace Sistema_CIN.Data
{
    public partial class CIN_pruebaContext : DbContext
    {
       
        public CIN_pruebaContext(DbContextOptions<CIN_pruebaContext> options)
            : base(options)
        {
        }

        public virtual DbSet<BitacoraIngresoSalidas> BitacoraIngresoSalida { get; set; } = null!;
        public virtual DbSet<BitacoraMovimiento> BitacoraMovimientos { get; set; } = null!;
        public virtual DbSet<Encargados> Encargados { get; set; } = null!;
        public virtual DbSet<Modulos> Modulos { get; set; } = null!;
        public virtual DbSet<Permisos> Permisos { get; set; } = null!;
        public virtual DbSet<Personal> Personals { get; set; } = null!;
        public virtual DbSet<Pme> Pmes { get; set; } = null!;
        public virtual DbSet<Roles> Roles { get; set; } = null!;
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
                    .HasName("PK__Bitacora__7E4268B00C364DEE");

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
                    .HasName("PK__Bitacora__7E4268B0EBA23FAF");

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
                    .HasName("PK__Encargad__770F28EAAA3893C6");

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
                    .HasConstraintName("FK__Encargado__id_pm__5BE2A6F2");
            });

            modelBuilder.Entity<Modulos>(entity =>
            {
                entity.HasKey(e => e.IdModulo)
                    .HasName("PK__Modulos__B2584DFC8FD9616F");

                entity.Property(e => e.IdModulo).HasColumnName("id_modulo");

                entity.Property(e => e.NombreModulo)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("nombre_modulo");
            });

            modelBuilder.Entity<Permisos>(entity =>
            {
                entity.HasNoKey();

                entity.Property(e => e.IdModulo).HasColumnName("id_modulo");

                entity.Property(e => e.IdRol).HasColumnName("id_rol");

                entity.Property(e => e.Permitido).HasColumnName("permitido");

                entity.HasOne(d => d.IdModuloNavigation)
                    .WithMany()
                    .HasForeignKey(d => d.IdModulo)
                    .HasConstraintName("FK__Permisos__id_mod__693CA210");

                entity.HasOne(d => d.IdRolNavigation)
                    .WithMany()
                    .HasForeignKey(d => d.IdRol)
                    .HasConstraintName("FK__Permisos__id_rol__68487DD7");
            });

            modelBuilder.Entity<Personal>(entity =>
            {
                entity.HasKey(e => e.IdPersonal)
                    .HasName("PK__Personal__418FB808ACF63FB8");

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

                entity.Property(e => e.TelefonoP)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("telefono_p");

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

                entity.HasOne(d => d.IdRolNavigation)
                    .WithMany(p => p.Personals)
                    .HasForeignKey(d => d.IdRol)
                    .HasConstraintName("FK__Personal__id_rol__5AEE82B9");
            });

            modelBuilder.Entity<Pme>(entity =>
            {
                entity.HasKey(e => e.IdPme)
                    .HasName("PK__PME__6FC802360690C129");

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
                    .HasConstraintName("FK__PME__id_encargad__5CD6CB2B");
            });

            modelBuilder.Entity<Roles>(entity =>
            {
                entity.HasKey(e => e.IdRol)
                    .HasName("PK__Roles__6ABCB5E0D21197B9");

                entity.Property(e => e.IdRol).HasColumnName("id_rol");

                entity.Property(e => e.NombreRol)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("nombre_rol");
            });

            modelBuilder.Entity<Usuario>(entity =>
            {
                entity.HasKey(e => e.IdUsuario)
                    .HasName("PK__Usuarios__4E3E04ADFFBD1DE5");

                entity.Property(e => e.IdUsuario).HasColumnName("id_usuario");

                entity.Property(e => e.AccesoU).HasColumnName("acceso_u");

                entity.Property(e => e.Clave)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("clave");

                entity.Property(e => e.CorreoU)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("correo_u");

                entity.Property(e => e.EstadoU).HasColumnName("estado_u");

                entity.Property(e => e.FotoU)
                    .HasMaxLength(300)
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
                    .HasConstraintName("FK__Usuarios__id_rol__5812160E");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
