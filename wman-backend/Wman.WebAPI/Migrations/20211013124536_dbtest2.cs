using Microsoft.EntityFrameworkCore.Migrations;

namespace Wman.WebAPI.Migrations
{
    public partial class dbtest2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "WmanUserWorkEvent",
                columns: table => new
                {
                    AssignedUsersId = table.Column<int>(type: "int", nullable: false),
                    WorkEventsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WmanUserWorkEvent", x => new { x.AssignedUsersId, x.WorkEventsId });
                    table.ForeignKey(
                        name: "FK_WmanUserWorkEvent_AspNetUsers_AssignedUsersId",
                        column: x => x.AssignedUsersId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WmanUserWorkEvent_WorkEvent_WorkEventsId",
                        column: x => x.WorkEventsId,
                        principalTable: "WorkEvent",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_WmanUserWorkEvent_WorkEventsId",
                table: "WmanUserWorkEvent",
                column: "WorkEventsId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WmanUserWorkEvent");
        }
    }
}
