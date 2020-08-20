using Microsoft.EntityFrameworkCore.Migrations;

namespace IF.Manager.Persistence.EF.Migrations
{
    public partial class _871 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "IFPageControlId",
                table: "IFPageControl",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CssClass",
                table: "IFPageControl",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_IFPageControl_IFPageControlId",
                table: "IFPageControl",
                column: "IFPageControlId");

            migrationBuilder.AddForeignKey(
                name: "FK_IFPageControl_IFPageControl_IFPageControlId",
                table: "IFPageControl",
                column: "IFPageControlId",
                principalTable: "IFPageControl",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_IFPageControl_IFPageControl_IFPageControlId",
                table: "IFPageControl");

            migrationBuilder.DropIndex(
                name: "IX_IFPageControl_IFPageControlId",
                table: "IFPageControl");

            migrationBuilder.DropColumn(
                name: "IFPageControlId",
                table: "IFPageControl");

            migrationBuilder.DropColumn(
                name: "CssClass",
                table: "IFPageControl");
        }
    }
}
