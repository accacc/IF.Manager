using Microsoft.EntityFrameworkCore.Migrations;

namespace IF.Manager.Persistence.EF.Migrations
{
    public partial class _011520211 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ParentId",
                table: "IFCommand",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_IFCommand_ParentId",
                table: "IFCommand",
                column: "ParentId");

            migrationBuilder.AddForeignKey(
                name: "FK_IFCommand_IFCommand_ParentId",
                table: "IFCommand",
                column: "ParentId",
                principalTable: "IFCommand",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_IFCommand_IFCommand_ParentId",
                table: "IFCommand");

            migrationBuilder.DropIndex(
                name: "IX_IFCommand_ParentId",
                table: "IFCommand");

            migrationBuilder.DropColumn(
                name: "ParentId",
                table: "IFCommand");
        }
    }
}
