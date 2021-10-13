using Microsoft.EntityFrameworkCore.Migrations;

namespace Wman.WebAPI.Migrations
{
    public partial class dbfix : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Label_Label_LabelId",
                table: "Label");

            migrationBuilder.DropForeignKey(
                name: "FK_Label_WorkEvent_WorkEventId",
                table: "Label");

            migrationBuilder.DropIndex(
                name: "IX_Label_LabelId",
                table: "Label");

            migrationBuilder.DropIndex(
                name: "IX_Label_WorkEventId",
                table: "Label");

            migrationBuilder.DropColumn(
                name: "LabelId",
                table: "Label");

            migrationBuilder.DropColumn(
                name: "WorkEventId",
                table: "Label");

            migrationBuilder.CreateTable(
                name: "LabelWorkEvent",
                columns: table => new
                {
                    LabelsId = table.Column<int>(type: "int", nullable: false),
                    WorkEventsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LabelWorkEvent", x => new { x.LabelsId, x.WorkEventsId });
                    table.ForeignKey(
                        name: "FK_LabelWorkEvent_Label_LabelsId",
                        column: x => x.LabelsId,
                        principalTable: "Label",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LabelWorkEvent_WorkEvent_WorkEventsId",
                        column: x => x.WorkEventsId,
                        principalTable: "WorkEvent",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LabelWorkEvent_WorkEventsId",
                table: "LabelWorkEvent",
                column: "WorkEventsId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LabelWorkEvent");

            migrationBuilder.AddColumn<int>(
                name: "LabelId",
                table: "Label",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "WorkEventId",
                table: "Label",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Label_LabelId",
                table: "Label",
                column: "LabelId");

            migrationBuilder.CreateIndex(
                name: "IX_Label_WorkEventId",
                table: "Label",
                column: "WorkEventId");

            migrationBuilder.AddForeignKey(
                name: "FK_Label_Label_LabelId",
                table: "Label",
                column: "LabelId",
                principalTable: "Label",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Label_WorkEvent_WorkEventId",
                table: "Label",
                column: "WorkEventId",
                principalTable: "WorkEvent",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
