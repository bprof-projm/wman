using Microsoft.EntityFrameworkCore.Migrations;

namespace Wman.WebAPI.Migrations
{
    public partial class _42 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WmanUserWorkEvent_AspNetUsers_WorkEventId",
                table: "WmanUserWorkEvent");

            migrationBuilder.DropForeignKey(
                name: "FK_WmanUserWorkEvent_WorkEvent_WmanUserId",
                table: "WmanUserWorkEvent");

            migrationBuilder.AddForeignKey(
                name: "FK_WmanUserWorkEvent_AspNetUsers_WmanUserId",
                table: "WmanUserWorkEvent",
                column: "WmanUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_WmanUserWorkEvent_WorkEvent_WorkEventId",
                table: "WmanUserWorkEvent",
                column: "WorkEventId",
                principalTable: "WorkEvent",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WmanUserWorkEvent_AspNetUsers_WmanUserId",
                table: "WmanUserWorkEvent");

            migrationBuilder.DropForeignKey(
                name: "FK_WmanUserWorkEvent_WorkEvent_WorkEventId",
                table: "WmanUserWorkEvent");

            migrationBuilder.AddForeignKey(
                name: "FK_WmanUserWorkEvent_AspNetUsers_WorkEventId",
                table: "WmanUserWorkEvent",
                column: "WorkEventId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_WmanUserWorkEvent_WorkEvent_WmanUserId",
                table: "WmanUserWorkEvent",
                column: "WmanUserId",
                principalTable: "WorkEvent",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
