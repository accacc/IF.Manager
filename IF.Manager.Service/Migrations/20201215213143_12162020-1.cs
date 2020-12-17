using Microsoft.EntityFrameworkCore.Migrations;

namespace IF.Manager.Persistence.EF.Migrations
{
    public partial class _121620201 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ParentId",
                table: "IFQueryFilterItem",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_IFQueryFilterItem_ParentId",
                table: "IFQueryFilterItem",
                column: "ParentId");

            migrationBuilder.AddForeignKey(
                name: "FK_IFQueryFilterItem_IFQueryFilterItem_ParentId",
                table: "IFQueryFilterItem",
                column: "ParentId",
                principalTable: "IFQueryFilterItem",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_IFQueryFilterItem_IFQueryFilterItem_ParentId",
                table: "IFQueryFilterItem");

            migrationBuilder.DropIndex(
                name: "IX_IFQueryFilterItem_ParentId",
                table: "IFQueryFilterItem");

            migrationBuilder.DropColumn(
                name: "ParentId",
                table: "IFQueryFilterItem");
        }
    }
}
