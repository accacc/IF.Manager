using Microsoft.EntityFrameworkCore.Migrations;

namespace IF.Manager.Persistence.EF.Migrations
{
    public partial class _771 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "IFQueryId",
                table: "IFPageControl",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "NamePropertyId",
                table: "IFPageControl",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ValuePropertyId",
                table: "IFPageControl",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_IFPageControl_IFQueryId",
                table: "IFPageControl",
                column: "IFQueryId");

            migrationBuilder.AddForeignKey(
                name: "FK_IFPageControl_IFQuery_IFQueryId",
                table: "IFPageControl",
                column: "IFQueryId",
                principalTable: "IFQuery",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_IFPageControl_IFQuery_IFQueryId",
                table: "IFPageControl");

            migrationBuilder.DropIndex(
                name: "IX_IFPageControl_IFQueryId",
                table: "IFPageControl");

            migrationBuilder.DropColumn(
                name: "IFQueryId",
                table: "IFPageControl");

            migrationBuilder.DropColumn(
                name: "NamePropertyId",
                table: "IFPageControl");

            migrationBuilder.DropColumn(
                name: "ValuePropertyId",
                table: "IFPageControl");
        }
    }
}
