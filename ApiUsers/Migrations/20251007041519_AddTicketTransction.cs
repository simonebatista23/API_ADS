using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ApiUsers.Migrations
{
    /// <inheritdoc />
    public partial class AddTicketTransction : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ticketstransction",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Id_user_source = table.Column<int>(type: "int", nullable: true),
                    Id_user_target = table.Column<int>(type: "int", nullable: true),
                    Id_ticket = table.Column<int>(type: "int", nullable: true),
                    Body = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Created_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Attach_url = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ticketstransction", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Ticket_TicketTransction",
                        column: x => x.Id_ticket,
                        principalTable: "tickets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserSource_TicketTransction",
                        column: x => x.Id_user_source,
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_UserTarget_TicketTransction",
                        column: x => x.Id_user_target,
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ticketstransction_Id_ticket",
                table: "ticketstransction",
                column: "Id_ticket");

            migrationBuilder.CreateIndex(
                name: "IX_ticketstransction_Id_user_source",
                table: "ticketstransction",
                column: "Id_user_source");

            migrationBuilder.CreateIndex(
                name: "IX_ticketstransction_Id_user_target",
                table: "ticketstransction",
                column: "Id_user_target");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ticketstransction");
        }
    }
}
