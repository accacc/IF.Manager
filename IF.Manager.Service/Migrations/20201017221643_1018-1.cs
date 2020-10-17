using Microsoft.EntityFrameworkCore.Migrations;

namespace IF.Manager.Persistence.EF.Migrations
{
    public partial class _10181 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "IFFilterPageFormId",
                table: "IFPageControl",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_IFPageControl_IFFilterPageFormId",
                table: "IFPageControl",
                column: "IFFilterPageFormId");

            migrationBuilder.AddForeignKey(
                name: "FK_IFPageControl_IFPageControl_IFFilterPageFormId",
                table: "IFPageControl",
                column: "IFFilterPageFormId",
                principalTable: "IFPageControl",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_IFPageControl_IFPageControl_IFFilterPageFormId",
                table: "IFPageControl");

            migrationBuilder.DropIndex(
                name: "IX_IFPageControl_IFFilterPageFormId",
                table: "IFPageControl");

            migrationBuilder.DropColumn(
                name: "IFFilterPageFormId",
                table: "IFPageControl");
        }
    }
}
