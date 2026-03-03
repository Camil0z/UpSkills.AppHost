using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace UpSkills.Api.Migrations
{
    /// <inheritdoc />
    public partial class FirstMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "public");

            migrationBuilder.EnsureSchema(
                name: "users");

            migrationBuilder.CreateTable(
                name: "Pais",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Nombre = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pais", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Rol",
                schema: "users",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Nombre = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rol", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Usuario",
                schema: "users",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Activo = table.Column<bool>(type: "boolean", nullable: false),
                    PrimerNombre = table.Column<string>(type: "text", nullable: false),
                    SegundoNombre = table.Column<string>(type: "text", nullable: true),
                    PrimerApellido = table.Column<string>(type: "text", nullable: false),
                    SegundoApellido = table.Column<string>(type: "text", nullable: true),
                    Correo = table.Column<string>(type: "text", nullable: false),
                    Contrasena = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    PaisId = table.Column<long>(type: "bigint", nullable: true),
                    RolId = table.Column<long>(type: "bigint", nullable: false),
                    CreadoEn = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuario", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Usuario_Pais_PaisId",
                        column: x => x.PaisId,
                        principalSchema: "public",
                        principalTable: "Pais",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Usuario_Rol_RolId",
                        column: x => x.RolId,
                        principalSchema: "users",
                        principalTable: "Rol",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Pais_Nombre",
                schema: "public",
                table: "Pais",
                column: "Nombre",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Rol_Nombre",
                schema: "users",
                table: "Rol",
                column: "Nombre",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Usuario_Correo",
                schema: "users",
                table: "Usuario",
                column: "Correo",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Usuario_PaisId",
                schema: "users",
                table: "Usuario",
                column: "PaisId");

            migrationBuilder.CreateIndex(
                name: "IX_Usuario_RolId",
                schema: "users",
                table: "Usuario",
                column: "RolId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Usuario",
                schema: "users");

            migrationBuilder.DropTable(
                name: "Pais",
                schema: "public");

            migrationBuilder.DropTable(
                name: "Rol",
                schema: "users");
        }
    }
}
