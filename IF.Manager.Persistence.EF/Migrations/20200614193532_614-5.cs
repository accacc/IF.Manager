using Microsoft.EntityFrameworkCore.Migrations;

namespace IF.Manager.Persistence.EF.Migrations
{
    public partial class _6145 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CommandId",
                table: "IFPageControl",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "IFPageGrid_QueryId",
                table: "IFPageControl",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_IFPageControl_CommandId",
                table: "IFPageControl",
                column: "CommandId");

            migrationBuilder.CreateIndex(
                name: "IX_IFPageControl_IFPageGrid_QueryId",
                table: "IFPageControl",
                column: "IFPageGrid_QueryId");

            migrationBuilder.AddForeignKey(
                name: "FK_IFPageControl_IFCommand_CommandId",
                table: "IFPageControl",
                column: "CommandId",
                principalTable: "IFCommand",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_IFPageControl_IFQuery_IFPageGrid_QueryId",
                table: "IFPageControl",
                column: "IFPageGrid_QueryId",
                principalTable: "IFQuery",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_IFPageControl_IFCommand_CommandId",
                table: "IFPageControl");

            migrationBuilder.DropForeignKey(
                name: "FK_IFPageControl_IFQuery_IFPageGrid_QueryId",
                table: "IFPageControl");

            migrationBuilder.DropIndex(
                name: "IX_IFPageControl_CommandId",
                table: "IFPageControl");

            migrationBuilder.DropIndex(
                name: "IX_IFPageControl_IFPageGrid_QueryId",
                table: "IFPageControl");

            migrationBuilder.DropColumn(
                name: "CommandId",
                table: "IFPageControl");

            migrationBuilder.DropColumn(
                name: "IFPageGrid_QueryId",
                table: "IFPageControl");
        }
    }
}
