using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RWS_LBE_ACS.Migrations
{
    /// <inheritdoc />
    public partial class AddMissingChanges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "audit_logs",
                columns: table => new
                {
                    ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    actor_id = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    method = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    path = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    status_code = table.Column<long>(type: "bigint", nullable: false),
                    client_ip = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    user_agent = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    request_body = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    response_body = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    latency_ms = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_audit_logs", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "sys_channel",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    app_id = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    app_key = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    sig_method = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    create_time = table.Column<DateTime>(type: "datetime2", nullable: false),
                    update_time = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_sys_channel", x => x.id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "audit_logs");

            migrationBuilder.DropTable(
                name: "sys_channel");
        }
    }
}
