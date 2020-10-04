using Microsoft.EntityFrameworkCore.Migrations;

namespace IF.Manager.Persistence.EF.Migrations
{
    public partial class _7121 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_IFPageControlMap_IFPageControl_PageControlId",
                table: "IFPageControlMap");

            migrationBuilder.DropIndex(
                name: "IX_IFPageControlMap_PageControlId",
                table: "IFPageControlMap");

            migrationBuilder.DropColumn(
                name: "PageControlId",
                table: "IFPageControlMap");

            migrationBuilder.AddColumn<int>(
                name: "IFPageControlId",
                table: "IFPageControlMap",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "IFPageControlMapId",
                table: "IFPageControl",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "IFPageNavigation_IFProjectId",
                table: "IFPageControl",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_IFPageControlMap_IFPageControlId",
                table: "IFPageControlMap",
                column: "IFPageControlId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_IFPageControl_IFPageNavigation_IFProjectId",
                table: "IFPageControl",
                column: "IFPageNavigation_IFProjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_IFPageControl_IFProject_IFPageNavigation_IFProjectId",
                table: "IFPageControl",
                column: "IFPageNavigation_IFProjectId",
                principalTable: "IFProject",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_IFPageControlMap_IFPageControl_IFPageControlId",
                table: "IFPageControlMap",
                column: "IFPageControlId",
                principalTable: "IFPageControl",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_IFPageControl_IFProject_IFPageNavigation_IFProjectId",
                table: "IFPageControl");

            migrationBuilder.DropForeignKey(
                name: "FK_IFPageControlMap_IFPageControl_IFPageControlId",
                table: "IFPageControlMap");

            migrationBuilder.DropIndex(
                name: "IX_IFPageControlMap_IFPageControlId",
                table: "IFPageControlMap");

            migrationBuilder.DropIndex(
                name: "IX_IFPageControl_IFPageNavigation_IFProjectId",
                table: "IFPageControl");

            migrationBuilder.DropColumn(
                name: "IFPageControlId",
                table: "IFPageControlMap");

            migrationBuilder.DropColumn(
                name: "IFPageControlMapId",
                table: "IFPageControl");

            migrationBuilder.DropColumn(
                name: "IFPageNavigation_IFProjectId",
                table: "IFPageControl");

            migrationBuilder.AddColumn<int>(
                name: "PageControlId",
                table: "IFPageControlMap",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_IFPageControlMap_PageControlId",
                table: "IFPageControlMap",
                column: "PageControlId");

            migrationBuilder.AddForeignKey(
                name: "FK_IFPageControlMap_IFPageControl_PageControlId",
                table: "IFPageControlMap",
                column: "PageControlId",
                principalTable: "IFPageControl",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
