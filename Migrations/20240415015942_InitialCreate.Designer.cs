﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Sistema_CIN.Data;

#nullable disable

namespace Sistema_CIN.Migrations
{
    [DbContext(typeof(CIN_pruebaContext))]
    [Migration("20240415015942_InitialCreate")]
    partial class InitialCreate
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.29")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("Sistema_CIN.Models.BitacoraIngresoSalidas", b =>
                {
                    b.Property<int>("IdBitacora")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id_bitacora");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IdBitacora"), 1L, 1);

                    b.Property<int?>("EstadoActual")
                        .HasColumnType("int")
                        .HasColumnName("estado_actual");

                    b.Property<DateTime?>("FechaIngreso")
                        .HasColumnType("datetime")
                        .HasColumnName("fecha_ingreso");

                    b.Property<DateTime?>("FechaSalida")
                        .HasColumnType("datetime")
                        .HasColumnName("fecha_salida");

                    b.Property<string>("UsuarioB")
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)")
                        .HasColumnName("usuario_b");

                    b.HasKey("IdBitacora")
                        .HasName("PK__Bitacora__7E4268B00C364DEE");

                    b.ToTable("Bitacora_ingreso_salida", (string)null);
                });

            modelBuilder.Entity("Sistema_CIN.Models.BitacoraMovimiento", b =>
                {
                    b.Property<int>("IdBitacora")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id_bitacora");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IdBitacora"), 1L, 1);

                    b.Property<string>("Detalle")
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)")
                        .HasColumnName("detalle");

                    b.Property<DateTime?>("FechaMovimiento")
                        .HasColumnType("datetime")
                        .HasColumnName("fecha_movimiento");

                    b.Property<string>("TipoMovimiento")
                        .HasMaxLength(20)
                        .IsUnicode(false)
                        .HasColumnType("varchar(20)")
                        .HasColumnName("tipo_movimiento");

                    b.Property<string>("UsuarioB")
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)")
                        .HasColumnName("usuario_b");

                    b.HasKey("IdBitacora")
                        .HasName("PK__Bitacora__7E4268B0EBA23FAF");

                    b.ToTable("Bitacora_movimientos", (string)null);
                });

            modelBuilder.Entity("Sistema_CIN.Models.Encargados", b =>
                {
                    b.Property<int>("IdEncargado")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id_encargado");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IdEncargado"), 1L, 1);

                    b.Property<string>("ApellidosE")
                        .IsRequired()
                        .HasMaxLength(60)
                        .IsUnicode(false)
                        .HasColumnType("varchar(60)")
                        .HasColumnName("apellidos_e");

                    b.Property<string>("CedulaE")
                        .IsRequired()
                        .HasMaxLength(25)
                        .IsUnicode(false)
                        .HasColumnType("varchar(25)")
                        .HasColumnName("cedula_e");

                    b.Property<string>("CorreoE")
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)")
                        .HasColumnName("correo_e");

                    b.Property<string>("DireccionE")
                        .IsRequired()
                        .HasMaxLength(60)
                        .IsUnicode(false)
                        .HasColumnType("varchar(60)")
                        .HasColumnName("direccion_e");

                    b.Property<int>("Edad")
                        .HasColumnType("int")
                        .HasColumnName("edad");

                    b.Property<DateTime>("FechaNaceE")
                        .HasColumnType("date")
                        .HasColumnName("fecha_nace_e");

                    b.Property<int?>("IdPme")
                        .HasColumnType("int")
                        .HasColumnName("id_pme");

                    b.Property<string>("LugarTrabajoE")
                        .HasMaxLength(25)
                        .IsUnicode(false)
                        .HasColumnType("varchar(25)")
                        .HasColumnName("lugar_trabajo_e");

                    b.Property<string>("NombreE")
                        .IsRequired()
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)")
                        .HasColumnName("nombre_e");

                    b.Property<string>("TelefonoE")
                        .IsRequired()
                        .HasMaxLength(10)
                        .IsUnicode(false)
                        .HasColumnType("varchar(10)")
                        .HasColumnName("telefono_e");

                    b.HasKey("IdEncargado")
                        .HasName("PK__Encargad__770F28EAAA3893C6");

                    b.HasIndex("IdPme");

                    b.ToTable("Encargados");
                });

            modelBuilder.Entity("Sistema_CIN.Models.Modulos", b =>
                {
                    b.Property<int>("IdModulo")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id_modulo");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IdModulo"), 1L, 1);

                    b.Property<string>("NombreModulo")
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)")
                        .HasColumnName("nombre_modulo");

                    b.HasKey("IdModulo")
                        .HasName("PK__Modulos__B2584DFC8FD9616F");

                    b.ToTable("Modulos");
                });

            modelBuilder.Entity("Sistema_CIN.Models.Permisos", b =>
                {
                    b.Property<int?>("IdModulo")
                        .HasColumnType("int")
                        .HasColumnName("id_modulo");

                    b.Property<int?>("IdRol")
                        .HasColumnType("int")
                        .HasColumnName("id_rol");

                    b.Property<bool?>("Permitido")
                        .HasColumnType("bit")
                        .HasColumnName("permitido");

                    b.HasIndex("IdModulo");

                    b.HasIndex("IdRol");

                    b.ToTable("Permisos");
                });

            modelBuilder.Entity("Sistema_CIN.Models.Personal", b =>
                {
                    b.Property<int>("IdPersonal")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id_personal");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IdPersonal"), 1L, 1);

                    b.Property<string>("ApellidosP")
                        .IsRequired()
                        .HasMaxLength(60)
                        .IsUnicode(false)
                        .HasColumnType("varchar(60)")
                        .HasColumnName("apellidos_p");

                    b.Property<string>("CantonP")
                        .HasMaxLength(20)
                        .IsUnicode(false)
                        .HasColumnType("varchar(20)")
                        .HasColumnName("canton_p");

                    b.Property<string>("CedulaP")
                        .IsRequired()
                        .HasMaxLength(25)
                        .IsUnicode(false)
                        .HasColumnType("varchar(25)")
                        .HasColumnName("cedula_p");

                    b.Property<string>("CorreoP")
                        .IsRequired()
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)")
                        .HasColumnName("correo_p");

                    b.Property<string>("DistritoP")
                        .HasMaxLength(20)
                        .IsUnicode(false)
                        .HasColumnType("varchar(20)")
                        .HasColumnName("distrito_p");

                    b.Property<int>("EdadP")
                        .HasColumnType("int")
                        .HasColumnName("edad_p");

                    b.Property<DateTime?>("FechaNaceP")
                        .HasColumnType("date")
                        .HasColumnName("fecha_nace_p");

                    b.Property<string>("GeneroP")
                        .IsRequired()
                        .HasMaxLength(20)
                        .IsUnicode(false)
                        .HasColumnType("varchar(20)")
                        .HasColumnName("genero_p");

                    b.Property<int?>("IdRol")
                        .HasColumnType("int")
                        .HasColumnName("id_rol");

                    b.Property<string>("NombreP")
                        .IsRequired()
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)")
                        .HasColumnName("nombre_p");

                    b.Property<string>("ProvinciaP")
                        .IsRequired()
                        .HasMaxLength(20)
                        .IsUnicode(false)
                        .HasColumnType("varchar(20)")
                        .HasColumnName("provincia_p");

                    b.Property<string>("TelefonoP")
                        .IsRequired()
                        .HasMaxLength(20)
                        .IsUnicode(false)
                        .HasColumnType("varchar(20)")
                        .HasColumnName("telefono_p");

                    b.HasKey("IdPersonal")
                        .HasName("PK__Personal__418FB808ACF63FB8");

                    b.HasIndex("IdRol");

                    b.ToTable("Personal", (string)null);
                });

            modelBuilder.Entity("Sistema_CIN.Models.Pme", b =>
                {
                    b.Property<int>("IdPme")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id_pme");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IdPme"), 1L, 1);

                    b.Property<string>("ApellidosPme")
                        .IsRequired()
                        .HasMaxLength(60)
                        .IsUnicode(false)
                        .HasColumnType("varchar(60)")
                        .HasColumnName("apellidos_pme");

                    b.Property<string>("CantonPme")
                        .HasMaxLength(20)
                        .IsUnicode(false)
                        .HasColumnType("varchar(20)")
                        .HasColumnName("canton_pme");

                    b.Property<string>("CedulaPme")
                        .IsRequired()
                        .HasMaxLength(25)
                        .IsUnicode(false)
                        .HasColumnType("varchar(25)")
                        .HasColumnName("cedula_pme");

                    b.Property<string>("CondiciónMigratoriaPme")
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)")
                        .HasColumnName("condición_migratoria_pme");

                    b.Property<string>("DistritoPme")
                        .HasMaxLength(20)
                        .IsUnicode(false)
                        .HasColumnType("varchar(20)")
                        .HasColumnName("distrito_pme");

                    b.Property<int>("EdadPme")
                        .HasColumnType("int")
                        .HasColumnName("edad_pme");

                    b.Property<DateTime?>("FechaEgresoPme")
                        .HasColumnType("datetime")
                        .HasColumnName("fecha_egreso_pme");

                    b.Property<DateTime>("FechaIngresoPme")
                        .HasColumnType("date")
                        .HasColumnName("fecha_ingreso_pme");

                    b.Property<DateTime>("FechaNacimientoPme")
                        .HasColumnType("date")
                        .HasColumnName("fecha_nacimiento_pme");

                    b.Property<string>("GeneroPme")
                        .IsRequired()
                        .HasMaxLength(20)
                        .IsUnicode(false)
                        .HasColumnType("varchar(20)")
                        .HasColumnName("genero_pme");

                    b.Property<int?>("IdEncargado")
                        .HasColumnType("int")
                        .HasColumnName("id_encargado");

                    b.Property<string>("NacionalidadPme")
                        .IsRequired()
                        .HasMaxLength(20)
                        .IsUnicode(false)
                        .HasColumnType("varchar(20)")
                        .HasColumnName("nacionalidad_pme");

                    b.Property<string>("NivelEducativoPme")
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)")
                        .HasColumnName("nivel_educativo_pme");

                    b.Property<string>("NombrePme")
                        .IsRequired()
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)")
                        .HasColumnName("nombre_pme");

                    b.Property<string>("PolizaSeguro")
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)")
                        .HasColumnName("poliza_seguro");

                    b.Property<string>("ProvinciaPme")
                        .IsRequired()
                        .HasMaxLength(20)
                        .IsUnicode(false)
                        .HasColumnType("varchar(20)")
                        .HasColumnName("provincia_pme");

                    b.Property<bool?>("SubvencionPme")
                        .HasColumnType("bit")
                        .HasColumnName("subvencion_pme");

                    b.HasKey("IdPme")
                        .HasName("PK__PME__6FC802360690C129");

                    b.HasIndex("IdEncargado");

                    b.ToTable("PME", (string)null);
                });

            modelBuilder.Entity("Sistema_CIN.Models.Roles", b =>
                {
                    b.Property<int>("IdRol")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id_rol");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IdRol"), 1L, 1);

                    b.Property<string>("NombreRol")
                        .IsRequired()
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)")
                        .HasColumnName("nombre_rol");

                    b.HasKey("IdRol")
                        .HasName("PK__Roles__6ABCB5E0D21197B9");

                    b.ToTable("Roles");
                });

            modelBuilder.Entity("Sistema_CIN.Models.Usuario", b =>
                {
                    b.Property<int>("IdUsuario")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id_usuario");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IdUsuario"), 1L, 1);

                    b.Property<bool?>("AccesoU")
                        .HasColumnType("bit")
                        .HasColumnName("acceso_u");

                    b.Property<string>("Clave")
                        .IsRequired()
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)")
                        .HasColumnName("clave");

                    b.Property<string>("CorreoU")
                        .IsRequired()
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)")
                        .HasColumnName("correo_u");

                    b.Property<bool?>("EstadoU")
                        .HasColumnType("bit")
                        .HasColumnName("estado_u");

                    b.Property<string>("FotoU")
                        .HasMaxLength(300)
                        .IsUnicode(false)
                        .HasColumnType("varchar(300)")
                        .HasColumnName("foto_u");

                    b.Property<int?>("IdRol")
                        .HasColumnType("int")
                        .HasColumnName("id_rol");

                    b.Property<string>("NombreU")
                        .IsRequired()
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)")
                        .HasColumnName("nombre_u");

                    b.Property<string>("Token")
                        .HasMaxLength(100)
                        .IsUnicode(false)
                        .HasColumnType("varchar(100)")
                        .HasColumnName("token");

                    b.HasKey("IdUsuario")
                        .HasName("PK__Usuarios__4E3E04ADFFBD1DE5");

                    b.HasIndex("IdRol");

                    b.ToTable("Usuarios");
                });

            modelBuilder.Entity("Sistema_CIN.Models.Encargados", b =>
                {
                    b.HasOne("Sistema_CIN.Models.Pme", "IdPmeNavigation")
                        .WithMany("Encargados")
                        .HasForeignKey("IdPme")
                        .HasConstraintName("FK__Encargado__id_pm__5BE2A6F2");

                    b.Navigation("IdPmeNavigation");
                });

            modelBuilder.Entity("Sistema_CIN.Models.Permisos", b =>
                {
                    b.HasOne("Sistema_CIN.Models.Modulos", "IdModuloNavigation")
                        .WithMany()
                        .HasForeignKey("IdModulo")
                        .HasConstraintName("FK__Permisos__id_mod__693CA210");

                    b.HasOne("Sistema_CIN.Models.Roles", "IdRolNavigation")
                        .WithMany()
                        .HasForeignKey("IdRol")
                        .HasConstraintName("FK__Permisos__id_rol__68487DD7");

                    b.Navigation("IdModuloNavigation");

                    b.Navigation("IdRolNavigation");
                });

            modelBuilder.Entity("Sistema_CIN.Models.Personal", b =>
                {
                    b.HasOne("Sistema_CIN.Models.Roles", "IdRolNavigation")
                        .WithMany("Personals")
                        .HasForeignKey("IdRol")
                        .HasConstraintName("FK__Personal__id_rol__5AEE82B9");

                    b.Navigation("IdRolNavigation");
                });

            modelBuilder.Entity("Sistema_CIN.Models.Pme", b =>
                {
                    b.HasOne("Sistema_CIN.Models.Encargados", "IdEncargadoNavigation")
                        .WithMany("Pmes")
                        .HasForeignKey("IdEncargado")
                        .HasConstraintName("FK__PME__id_encargad__5CD6CB2B");

                    b.Navigation("IdEncargadoNavigation");
                });

            modelBuilder.Entity("Sistema_CIN.Models.Usuario", b =>
                {
                    b.HasOne("Sistema_CIN.Models.Roles", "IdRolNavigation")
                        .WithMany("Usuarios")
                        .HasForeignKey("IdRol")
                        .HasConstraintName("FK__Usuarios__id_rol__5812160E");

                    b.Navigation("IdRolNavigation");
                });

            modelBuilder.Entity("Sistema_CIN.Models.Encargados", b =>
                {
                    b.Navigation("Pmes");
                });

            modelBuilder.Entity("Sistema_CIN.Models.Pme", b =>
                {
                    b.Navigation("Encargados");
                });

            modelBuilder.Entity("Sistema_CIN.Models.Roles", b =>
                {
                    b.Navigation("Personals");

                    b.Navigation("Usuarios");
                });
#pragma warning restore 612, 618
        }
    }
}
