using Microsoft.EntityFrameworkCore.Migrations;

namespace IF.Manager.Persistence.EF.Migrations
{
    public partial class _6162 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "IFPageForm_IFModelId",
                table: "IFPageControl",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_IFPageControl_IFPageForm_IFModelId",
                table: "IFPageControl",
                column: "IFPageForm_IFModelId");

            migrationBuilder.AddForeignKey(
                name: "FK_IFPageControl_IFModel_IFPageForm_IFModelId",
                table: "IFPageControl",
                column: "IFPageForm_IFModelId",
                principalTable: "IFModel",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_IFPageControl_IFModel_IFPageForm_IFModelId",
                table: "IFPageControl");

            migrationBuilder.DropIndex(
                name: "IX_IFPageControl_IFPageForm_IFModelId",
                table: "IFPageControl");

            migrationBuilder.DropColumn(
                name: "IFPageForm_IFModelId",
                table: "IFPageControl");
        }
    }
}
