using Microsoft.EntityFrameworkCore.Migrations;

namespace IF.Manager.Persistence.EF.Migrations
{
    public partial class _5161 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_IFQueryFilterItem_IFCommand_IFCommandId",
                table: "IFQueryFilterItem");

            migrationBuilder.DropIndex(
                name: "IX_IFQueryFilterItem_IFCommandId",
                table: "IFQueryFilterItem");

            migrationBuilder.DropColumn(
                name: "IFCommandId",
                table: "IFQueryFilterItem");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "IFCommandId",
                table: "IFQueryFilterItem",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_IFQueryFilterItem_IFCommandId",
                table: "IFQueryFilterItem",
                column: "IFCommandId");

            migrationBuilder.AddForeignKey(
                name: "FK_IFQueryFilterItem_IFCommand_IFCommandId",
                table: "IFQueryFilterItem",
                column: "IFCommandId",
                principalTable: "IFCommand",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
