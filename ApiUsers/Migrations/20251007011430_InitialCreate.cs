using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ApiUsers.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "depts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AcceptTicket = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_depts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "profiles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Desc = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_profiles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "statusticket",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Desc = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_statusticket", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "statususers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Desc = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_statususers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "category",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Desc = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IdDept = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_category", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Category_Dept",
                        column: x => x.IdDept,
                        principalTable: "depts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Pwd = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IdDept = table.Column<int>(type: "int", nullable: true),
                    IdStatus = table.Column<int>(type: "int", nullable: true),
                    IdProfile = table.Column<int>(type: "int", nullable: true),
                    Blocked = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_users", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Users_Dept",
                        column: x => x.IdDept,
                        principalTable: "depts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Users_Profile",
                        column: x => x.IdProfile,
                        principalTable: "profiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Users_Statususer",
                        column: x => x.IdStatus,
                        principalTable: "statususers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "tickets",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Id_user = table.Column<int>(type: "int", nullable: true),
                    Id_dept_target = table.Column<int>(type: "int", nullable: true),
                    Id_category = table.Column<int>(type: "int", nullable: true),
                    Id_status = table.Column<int>(type: "int", nullable: true),
                    Desc = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Open_datetime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Priority_level = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tickets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Category_Category",
                        column: x => x.Id_category,
                        principalTable: "category",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Dept_DeptTicket",
                        column: x => x.Id_dept_target,
                        principalTable: "depts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Status_StatusTicket",
                        column: x => x.Id_status,
                        principalTable: "statusticket",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_user_UserTicket",
                        column: x => x.Id_user,
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateIndex(
                name: "IX_category_IdDept",
                table: "category",
                column: "IdDept");

            migrationBuilder.CreateIndex(
                name: "IX_tickets_Id_category",
                table: "tickets",
                column: "Id_category");

            migrationBuilder.CreateIndex(
                name: "IX_tickets_Id_dept_target",
                table: "tickets",
                column: "Id_dept_target");

            migrationBuilder.CreateIndex(
                name: "IX_tickets_Id_status",
                table: "tickets",
                column: "Id_status");

            migrationBuilder.CreateIndex(
                name: "IX_tickets_Id_user",
                table: "tickets",
                column: "Id_user");

            migrationBuilder.CreateIndex(
                name: "IX_users_IdDept",
                table: "users",
                column: "IdDept");

            migrationBuilder.CreateIndex(
                name: "IX_users_IdProfile",
                table: "users",
                column: "IdProfile");

            migrationBuilder.CreateIndex(
                name: "IX_users_IdStatus",
                table: "users",
                column: "IdStatus");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tickets");

            migrationBuilder.DropTable(
                name: "category");

            migrationBuilder.DropTable(
                name: "statusticket");

            migrationBuilder.DropTable(
                name: "users");

            migrationBuilder.DropTable(
                name: "depts");

            migrationBuilder.DropTable(
                name: "profiles");

            migrationBuilder.DropTable(
                name: "statususers");
        }
    }
}
