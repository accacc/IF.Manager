using Microsoft.EntityFrameworkCore.Migrations;

namespace IF.Manager.Persistence.EF.Migrations
{
    public partial class _14010120211 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FromClassId",
                table: "IFClassMapper");

            migrationBuilder.DropColumn(
                name: "FromType",
                table: "IFClassMapper");

            migrationBuilder.DropColumn(
                name: "ToClassId",
                table: "IFClassMapper");

            migrationBuilder.DropColumn(
                name: "ToType",
                table: "IFClassMapper");

            migrationBuilder.AddColumn<int>(
                name: "IFClassId",
                table: "IFClassMapper",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "IFModelId",
                table: "IFClassMapper",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_IFClassMapper_IFClassId",
                table: "IFClassMapper",
                column: "IFClassId");

            migrationBuilder.CreateIndex(
                name: "IX_IFClassMapper_IFModelId",
                table: "IFClassMapper",
                column: "IFModelId");

            migrationBuilder.AddForeignKey(
                name: "FK_IFClassMapper_IFClass_IFClassId",
                table: "IFClassMapper",
                column: "IFClassId",
                principalTable: "IFClass",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_IFClassMapper_IFModel_IFModelId",
                table: "IFClassMapper",
                column: "IFModelId",
                principalTable: "IFModel",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_IFClassMapper_IFClass_IFClassId",
                table: "IFClassMapper");

            migrationBuilder.DropForeignKey(
                name: "FK_IFClassMapper_IFModel_IFModelId",
                table: "IFClassMapper");

            migrationBuilder.DropIndex(
                name: "IX_IFClassMapper_IFClassId",
                table: "IFClassMapper");

            migrationBuilder.DropIndex(
                name: "IX_IFClassMapper_IFModelId",
                table: "IFClassMapper");

            migrationBuilder.DropColumn(
                name: "IFClassId",
                table: "IFClassMapper");

            migrationBuilder.DropColumn(
                name: "IFModelId",
                table: "IFClassMapper");

            migrationBuilder.AddColumn<int>(
                name: "FromClassId",
                table: "IFClassMapper",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "FromType",
                table: "IFClassMapper",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ToClassId",
                table: "IFClassMapper",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ToType",
                table: "IFClassMapper",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
