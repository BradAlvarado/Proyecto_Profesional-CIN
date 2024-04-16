using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sistema_CIN.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Bitacora_ingreso_salida",
                columns: table => new
                {
                    id_bitacora = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    usuario_b = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    fecha_ingreso = table.Column<DateTime>(type: "datetime", nullable: true),
                    fecha_salida = table.Column<DateTime>(type: "datetime", nullable: true),
                    estado_actual = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Bitacora__7E4268B00C364DEE", x => x.id_bitacora);
                });

            migrationBuilder.CreateTable(
                name: "Bitacora_movimientos",
                columns: table => new
                {
                    id_bitacora = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    usuario_b = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    fecha_movimiento = table.Column<DateTime>(type: "datetime", nullable: true),
                    tipo_movimiento = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: true),
                    detalle = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Bitacora__7E4268B0EBA23FAF", x => x.id_bitacora);
                });

            migrationBuilder.CreateTable(
                name: "Modulos",
                columns: table => new
                {
                    id_modulo = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nombre_modulo = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Modulos__B2584DFC8FD9616F", x => x.id_modulo);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    id_rol = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nombre_rol = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Roles__6ABCB5E0D21197B9", x => x.id_rol);
                });

            migrationBuilder.CreateTable(
                name: "Permisos",
                columns: table => new
                {
                    id_rol = table.Column<int>(type: "int", nullable: true),
                    id_modulo = table.Column<int>(type: "int", nullable: true),
                    permitido = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.ForeignKey(
                        name: "FK__Permisos__id_mod__693CA210",
                        column: x => x.id_modulo,
                        principalTable: "Modulos",
                        principalColumn: "id_modulo");
                    table.ForeignKey(
                        name: "FK__Permisos__id_rol__68487DD7",
                        column: x => x.id_rol,
                        principalTable: "Roles",
                        principalColumn: "id_rol");
                });

            migrationBuilder.CreateTable(
                name: "Personal",
                columns: table => new
                {
                    id_personal = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    cedula_p = table.Column<string>(type: "varchar(25)", unicode: false, maxLength: 25, nullable: false),
                    nombre_p = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    apellidos_p = table.Column<string>(type: "varchar(60)", unicode: false, maxLength: 60, nullable: false),
                    correo_p = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    telefono_p = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: false),
                    fecha_nace_p = table.Column<DateTime>(type: "date", nullable: true),
                    edad_p = table.Column<int>(type: "int", nullable: false),
                    genero_p = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: false),
                    provincia_p = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: false),
                    canton_p = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: true),
                    distrito_p = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: true),
                    id_rol = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Personal__418FB808ACF63FB8", x => x.id_personal);
                    table.ForeignKey(
                        name: "FK__Personal__id_rol__5AEE82B9",
                        column: x => x.id_rol,
                        principalTable: "Roles",
                        principalColumn: "id_rol");
                });

            migrationBuilder.CreateTable(
                name: "Usuarios",
                columns: table => new
                {
                    id_usuario = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    foto_u = table.Column<string>(type: "varchar(300)", unicode: false, maxLength: 300, nullable: true),
                    nombre_u = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    correo_u = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    clave = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    token = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true),
                    estado_u = table.Column<bool>(type: "bit", nullable: true),
                    acceso_u = table.Column<bool>(type: "bit", nullable: true),
                    id_rol = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Usuarios__4E3E04ADFFBD1DE5", x => x.id_usuario);
                    table.ForeignKey(
                        name: "FK__Usuarios__id_rol__5812160E",
                        column: x => x.id_rol,
                        principalTable: "Roles",
                        principalColumn: "id_rol");
                });

            migrationBuilder.CreateTable(
                name: "Encargados",
                columns: table => new
                {
                    id_encargado = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    cedula_e = table.Column<string>(type: "varchar(25)", unicode: false, maxLength: 25, nullable: false),
                    nombre_e = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    apellidos_e = table.Column<string>(type: "varchar(60)", unicode: false, maxLength: 60, nullable: false),
                    fecha_nace_e = table.Column<DateTime>(type: "date", nullable: false),
                    edad = table.Column<int>(type: "int", nullable: false),
                    correo_e = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    direccion_e = table.Column<string>(type: "varchar(60)", unicode: false, maxLength: 60, nullable: false),
                    telefono_e = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: false),
                    lugar_trabajo_e = table.Column<string>(type: "varchar(25)", unicode: false, maxLength: 25, nullable: true),
                    id_pme = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Encargad__770F28EAAA3893C6", x => x.id_encargado);
                });

            migrationBuilder.CreateTable(
                name: "PME",
                columns: table => new
                {
                    id_pme = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    cedula_pme = table.Column<string>(type: "varchar(25)", unicode: false, maxLength: 25, nullable: false),
                    poliza_seguro = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    nombre_pme = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    apellidos_pme = table.Column<string>(type: "varchar(60)", unicode: false, maxLength: 60, nullable: false),
                    fecha_nacimiento_pme = table.Column<DateTime>(type: "date", nullable: false),
                    edad_pme = table.Column<int>(type: "int", nullable: false),
                    genero_pme = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: false),
                    provincia_pme = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: false),
                    canton_pme = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: true),
                    distrito_pme = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: true),
                    nacionalidad_pme = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: false),
                    subvencion_pme = table.Column<bool>(type: "bit", nullable: true),
                    fecha_ingreso_pme = table.Column<DateTime>(type: "date", nullable: false),
                    fecha_egreso_pme = table.Column<DateTime>(type: "datetime", nullable: true),
                    condición_migratoria_pme = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    nivel_educativo_pme = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    id_encargado = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__PME__6FC802360690C129", x => x.id_pme);
                    table.ForeignKey(
                        name: "FK__PME__id_encargad__5CD6CB2B",
                        column: x => x.id_encargado,
                        principalTable: "Encargados",
                        principalColumn: "id_encargado");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Encargados_id_pme",
                table: "Encargados",
                column: "id_pme");

            migrationBuilder.CreateIndex(
                name: "IX_Permisos_id_modulo",
                table: "Permisos",
                column: "id_modulo");

            migrationBuilder.CreateIndex(
                name: "IX_Permisos_id_rol",
                table: "Permisos",
                column: "id_rol");

            migrationBuilder.CreateIndex(
                name: "IX_Personal_id_rol",
                table: "Personal",
                column: "id_rol");

            migrationBuilder.CreateIndex(
                name: "IX_PME_id_encargado",
                table: "PME",
                column: "id_encargado");

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_id_rol",
                table: "Usuarios",
                column: "id_rol");

            migrationBuilder.AddForeignKey(
                name: "FK__Encargado__id_pm__5BE2A6F2",
                table: "Encargados",
                column: "id_pme",
                principalTable: "PME",
                principalColumn: "id_pme");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK__Encargado__id_pm__5BE2A6F2",
                table: "Encargados");

            migrationBuilder.DropTable(
                name: "Bitacora_ingreso_salida");

            migrationBuilder.DropTable(
                name: "Bitacora_movimientos");

            migrationBuilder.DropTable(
                name: "Permisos");

            migrationBuilder.DropTable(
                name: "Personal");

            migrationBuilder.DropTable(
                name: "Usuarios");

            migrationBuilder.DropTable(
                name: "Modulos");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "PME");

            migrationBuilder.DropTable(
                name: "Encargados");
        }
    }
}
