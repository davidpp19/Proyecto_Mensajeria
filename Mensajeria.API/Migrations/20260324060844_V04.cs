using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Mensajeria.API.Migrations
{
    /// <inheritdoc />
    public partial class V04 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Conversacion",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Tipo_Conversacion = table.Column<bool>(type: "boolean", nullable: false),
                    Fecha_Creacion_Conversacion = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    Titulo_Conversacion = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Conversacion", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EstadoMensaje",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Descripcion_Estado = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EstadoMensaje", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Rol",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Nombre_Rol = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rol", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Usuario",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Nombre_Usuario = table.Column<string>(type: "text", nullable: false),
                    Correo_Usuario = table.Column<string>(type: "text", nullable: false),
                    Contrasena_Usuario = table.Column<string>(type: "text", nullable: false),
                    Fecha_Creacion_Usuario = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    Estado_Usuario = table.Column<bool>(type: "boolean", nullable: false),
                    RolId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuario", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Usuario_Rol_RolId",
                        column: x => x.RolId,
                        principalTable: "Rol",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Mensaje",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Contenido_Mensaje = table.Column<string>(type: "text", nullable: false),
                    Hora_Envio_Mensaje = table.Column<TimeOnly>(type: "time without time zone", nullable: false),
                    ConversacionId = table.Column<int>(type: "integer", nullable: false),
                    RemitenteId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Mensaje", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Mensaje_Conversacion_ConversacionId",
                        column: x => x.ConversacionId,
                        principalTable: "Conversacion",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Mensaje_Usuario_RemitenteId",
                        column: x => x.RemitenteId,
                        principalTable: "Usuario",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ParticipanteConversacion",
                columns: table => new
                {
                    ConversacionId = table.Column<int>(type: "integer", nullable: false),
                    UsuarioId = table.Column<int>(type: "integer", nullable: false),
                    Fecha_Ingreso_Participante = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ParticipanteConversacion", x => new { x.ConversacionId, x.UsuarioId });
                    table.ForeignKey(
                        name: "FK_ParticipanteConversacion_Conversacion_ConversacionId",
                        column: x => x.ConversacionId,
                        principalTable: "Conversacion",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ParticipanteConversacion_Usuario_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuario",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MensajeLectura",
                columns: table => new
                {
                    MensajeId = table.Column<int>(type: "integer", nullable: false),
                    UsuarioId = table.Column<int>(type: "integer", nullable: false),
                    Fecha_Lectura = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    EstadoMensajeId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MensajeLectura", x => new { x.MensajeId, x.UsuarioId });
                    table.ForeignKey(
                        name: "FK_MensajeLectura_EstadoMensaje_EstadoMensajeId",
                        column: x => x.EstadoMensajeId,
                        principalTable: "EstadoMensaje",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MensajeLectura_Mensaje_MensajeId",
                        column: x => x.MensajeId,
                        principalTable: "Mensaje",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MensajeLectura_Usuario_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuario",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Mensaje_ConversacionId",
                table: "Mensaje",
                column: "ConversacionId");

            migrationBuilder.CreateIndex(
                name: "IX_Mensaje_RemitenteId",
                table: "Mensaje",
                column: "RemitenteId");

            migrationBuilder.CreateIndex(
                name: "IX_MensajeLectura_EstadoMensajeId",
                table: "MensajeLectura",
                column: "EstadoMensajeId");

            migrationBuilder.CreateIndex(
                name: "IX_MensajeLectura_UsuarioId",
                table: "MensajeLectura",
                column: "UsuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_ParticipanteConversacion_UsuarioId",
                table: "ParticipanteConversacion",
                column: "UsuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_Usuario_RolId",
                table: "Usuario",
                column: "RolId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MensajeLectura");

            migrationBuilder.DropTable(
                name: "ParticipanteConversacion");

            migrationBuilder.DropTable(
                name: "EstadoMensaje");

            migrationBuilder.DropTable(
                name: "Mensaje");

            migrationBuilder.DropTable(
                name: "Conversacion");

            migrationBuilder.DropTable(
                name: "Usuario");

            migrationBuilder.DropTable(
                name: "Rol");
        }
    }
}
