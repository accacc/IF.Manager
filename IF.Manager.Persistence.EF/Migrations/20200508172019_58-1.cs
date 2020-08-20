using Microsoft.EntityFrameworkCore.Migrations;

namespace IF.Manager.Persistence.EF.Migrations
{
    public partial class _581 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ProcessId",
                table: "IFCommand",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_IFCommand_ProcessId",
                table: "IFCommand",
                column: "ProcessId");

            migrationBuilder.AddForeignKey(
                name: "FK_IFCommand_IFProcess_ProcessId",
                table: "IFCommand",
                column: "ProcessId",
                principalTable: "IFProcess",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_IFCommand_IFProcess_ProcessId",
                table: "IFCommand");

            migrationBuilder.DropIndex(
                name: "IX_IFCommand_ProcessId",
                table: "IFCommand");

            migrationBuilder.DropColumn(
                name: "ProcessId",
                table: "IFCommand");
        }
    }
}
