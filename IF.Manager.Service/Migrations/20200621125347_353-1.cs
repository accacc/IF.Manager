using Microsoft.EntityFrameworkCore.Migrations;

namespace IF.Manager.Persistence.EF.Migrations
{
    public partial class _3531 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "IFProjectId",
                table: "IFPageControl",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_IFPageControl_IFProjectId",
                table: "IFPageControl",
                column: "IFProjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_IFPageControl_IFProject_IFProjectId",
                table: "IFPageControl",
                column: "IFProjectId",
                principalTable: "IFProject",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_IFPageControl_IFProject_IFProjectId",
                table: "IFPageControl");

            migrationBuilder.DropIndex(
                name: "IX_IFPageControl_IFProjectId",
                table: "IFPageControl");

            migrationBuilder.DropColumn(
                name: "IFProjectId",
                table: "IFPageControl");
        }
    }
}
