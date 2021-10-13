using Microsoft.EntityFrameworkCore.Migrations;

namespace Wman.WebAPI.Migrations
{
    public partial class dbrework : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WorkEventLabel");

            migrationBuilder.DropTable(
                name: "WorkEventPicture");

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

            migrationBuilder.CreateTable(
                name: "PicturesWorkEvent",
                columns: table => new
                {
                    ProofOfWorkPicId = table.Column<int>(type: "int", nullable: false),
                    WorkEventsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PicturesWorkEvent", x => new { x.ProofOfWorkPicId, x.WorkEventsId });
                    table.ForeignKey(
                        name: "FK_PicturesWorkEvent_Picture_ProofOfWorkPicId",
                        column: x => x.ProofOfWorkPicId,
                        principalTable: "Picture",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PicturesWorkEvent_WorkEvent_WorkEventsId",
                        column: x => x.WorkEventsId,
                        principalTable: "WorkEvent",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Label_LabelId",
                table: "Label",
                column: "LabelId");

            migrationBuilder.CreateIndex(
                name: "IX_Label_WorkEventId",
                table: "Label",
                column: "WorkEventId");

            migrationBuilder.CreateIndex(
                name: "IX_PicturesWorkEvent_WorkEventsId",
                table: "PicturesWorkEvent",
                column: "WorkEventsId");

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Label_Label_LabelId",
                table: "Label");

            migrationBuilder.DropForeignKey(
                name: "FK_Label_WorkEvent_WorkEventId",
                table: "Label");

            migrationBuilder.DropTable(
                name: "PicturesWorkEvent");

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
                name: "WorkEventLabel",
                columns: table => new
                {
                    WorkEventId = table.Column<int>(type: "int", nullable: false),
                    LabelId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkEventLabel", x => new { x.WorkEventId, x.LabelId });
                    table.ForeignKey(
                        name: "FK_WorkEventLabel_Label_WorkEventId",
                        column: x => x.WorkEventId,
                        principalTable: "Label",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WorkEventLabel_WorkEvent_LabelId",
                        column: x => x.LabelId,
                        principalTable: "WorkEvent",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WorkEventPicture",
                columns: table => new
                {
                    WorkEventId = table.Column<int>(type: "int", nullable: false),
                    PictureId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkEventPicture", x => new { x.WorkEventId, x.PictureId });
                    table.ForeignKey(
                        name: "FK_WorkEventPicture_Picture_WorkEventId",
                        column: x => x.WorkEventId,
                        principalTable: "Picture",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WorkEventPicture_WorkEvent_PictureId",
                        column: x => x.PictureId,
                        principalTable: "WorkEvent",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_WorkEventLabel_LabelId",
                table: "WorkEventLabel",
                column: "LabelId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkEventPicture_PictureId",
                table: "WorkEventPicture",
                column: "PictureId");
        }
    }
}
