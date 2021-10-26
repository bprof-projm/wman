using Microsoft.EntityFrameworkCore.Migrations;

namespace Wman.WebAPI.Migrations
{
    public partial class dbtest : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WmanUserWorkEvent");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "WmanUserWorkEvent",
                columns: table => new
                {
                    WorkEventId = table.Column<int>(type: "int", nullable: false),
                    WmanUserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WmanUserWorkEvent", x => new { x.WorkEventId, x.WmanUserId });
                    table.ForeignKey(
                        name: "FK_WmanUserWorkEvent_AspNetUsers_WmanUserId",
                        column: x => x.WmanUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WmanUserWorkEvent_WorkEvent_WorkEventId",
                        column: x => x.WorkEventId,
                        principalTable: "WorkEvent",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_WmanUserWorkEvent_WmanUserId",
                table: "WmanUserWorkEvent",
                column: "WmanUserId");
        }
    }
}
