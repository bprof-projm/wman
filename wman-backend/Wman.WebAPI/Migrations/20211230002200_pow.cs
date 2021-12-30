using Microsoft.EntityFrameworkCore.Migrations;

namespace Wman.WebAPI.Migrations
{
    public partial class pow : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PicturesWorkEvent");

            migrationBuilder.AddColumn<int>(
                name: "PicturesId",
                table: "WorkEvent",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ProofOfWork",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CloudPhotoID = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Url = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    WorkEventID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProofOfWork", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProofOfWork_WorkEvent_WorkEventID",
                        column: x => x.WorkEventID,
                        principalTable: "WorkEvent",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_WorkEvent_PicturesId",
                table: "WorkEvent",
                column: "PicturesId");

            migrationBuilder.CreateIndex(
                name: "IX_ProofOfWork_WorkEventID",
                table: "ProofOfWork",
                column: "WorkEventID");

            migrationBuilder.AddForeignKey(
                name: "FK_WorkEvent_Picture_PicturesId",
                table: "WorkEvent",
                column: "PicturesId",
                principalTable: "Picture",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WorkEvent_Picture_PicturesId",
                table: "WorkEvent");

            migrationBuilder.DropTable(
                name: "ProofOfWork");

            migrationBuilder.DropIndex(
                name: "IX_WorkEvent_PicturesId",
                table: "WorkEvent");

            migrationBuilder.DropColumn(
                name: "PicturesId",
                table: "WorkEvent");

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
                name: "IX_PicturesWorkEvent_WorkEventsId",
                table: "PicturesWorkEvent",
                column: "WorkEventsId");
        }
    }
}
