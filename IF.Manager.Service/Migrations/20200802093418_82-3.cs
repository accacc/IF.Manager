using Microsoft.EntityFrameworkCore.Migrations;

namespace IF.Manager.Persistence.EF.Migrations
{
    public partial class _823 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "IFPageParameterId",
                table: "IFQueryFilterItem",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_IFQueryFilterItem_IFPageParameterId",
                table: "IFQueryFilterItem",
                column: "IFPageParameterId");

            migrationBuilder.AddForeignKey(
                name: "FK_IFQueryFilterItem_IFPageParameter_IFPageParameterId",
                table: "IFQueryFilterItem",
                column: "IFPageParameterId",
                principalTable: "IFPageParameter",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_IFQueryFilterItem_IFPageParameter_IFPageParameterId",
                table: "IFQueryFilterItem");

            migrationBuilder.DropIndex(
                name: "IX_IFQueryFilterItem_IFPageParameterId",
                table: "IFQueryFilterItem");

            migrationBuilder.DropColumn(
                name: "IFPageParameterId",
                table: "IFQueryFilterItem");
        }
    }
}
