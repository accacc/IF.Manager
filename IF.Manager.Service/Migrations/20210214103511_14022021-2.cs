using Microsoft.EntityFrameworkCore.Migrations;

namespace IF.Manager.Persistence.EF.Migrations
{
    public partial class _140220212 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_IFCommand_IFClassMapping_IFClassMappingId",
                table: "IFCommand");

            migrationBuilder.DropIndex(
                name: "IX_IFCommand_IFClassMappingId",
                table: "IFCommand");

            migrationBuilder.DropColumn(
                name: "IFClassMappingId",
                table: "IFCommand");

            migrationBuilder.AddColumn<int>(
                name: "IFClassMapperId",
                table: "IFCommand",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_IFCommand_IFClassMapperId",
                table: "IFCommand",
                column: "IFClassMapperId");

            migrationBuilder.AddForeignKey(
                name: "FK_IFCommand_IFClassMapper_IFClassMapperId",
                table: "IFCommand",
                column: "IFClassMapperId",
                principalTable: "IFClassMapper",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_IFCommand_IFClassMapper_IFClassMapperId",
                table: "IFCommand");

            migrationBuilder.DropIndex(
                name: "IX_IFCommand_IFClassMapperId",
                table: "IFCommand");

            migrationBuilder.DropColumn(
                name: "IFClassMapperId",
                table: "IFCommand");

            migrationBuilder.AddColumn<int>(
                name: "IFClassMappingId",
                table: "IFCommand",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_IFCommand_IFClassMappingId",
                table: "IFCommand",
                column: "IFClassMappingId");

            migrationBuilder.AddForeignKey(
                name: "FK_IFCommand_IFClassMapping_IFClassMappingId",
                table: "IFCommand",
                column: "IFClassMappingId",
                principalTable: "IFClassMapping",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
