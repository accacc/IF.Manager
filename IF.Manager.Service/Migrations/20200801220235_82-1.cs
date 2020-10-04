using Microsoft.EntityFrameworkCore.Migrations;

namespace IF.Manager.Persistence.EF.Migrations
{
    public partial class _821 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "IFPageListView_FormLayoutId",
                table: "IFPageControl",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "IFPageListView_IFModelId",
                table: "IFPageControl",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_IFPageControlMap_ParentId",
                table: "IFPageControlMap",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_IFPageControl_IFPageListView_FormLayoutId",
                table: "IFPageControl",
                column: "IFPageListView_FormLayoutId");

            migrationBuilder.CreateIndex(
                name: "IX_IFPageControl_IFPageListView_IFModelId",
                table: "IFPageControl",
                column: "IFPageListView_IFModelId");

            migrationBuilder.AddForeignKey(
                name: "FK_IFPageControl_IFPageFormLayout_IFPageListView_FormLayoutId",
                table: "IFPageControl",
                column: "IFPageListView_FormLayoutId",
                principalTable: "IFPageFormLayout",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_IFPageControl_IFModel_IFPageListView_IFModelId",
                table: "IFPageControl",
                column: "IFPageListView_IFModelId",
                principalTable: "IFModel",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_IFPageControlMap_IFPageControlMap_ParentId",
                table: "IFPageControlMap",
                column: "ParentId",
                principalTable: "IFPageControlMap",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_IFPageControl_IFPageFormLayout_IFPageListView_FormLayoutId",
                table: "IFPageControl");

            migrationBuilder.DropForeignKey(
                name: "FK_IFPageControl_IFModel_IFPageListView_IFModelId",
                table: "IFPageControl");

            migrationBuilder.DropForeignKey(
                name: "FK_IFPageControlMap_IFPageControlMap_ParentId",
                table: "IFPageControlMap");

            migrationBuilder.DropIndex(
                name: "IX_IFPageControlMap_ParentId",
                table: "IFPageControlMap");

            migrationBuilder.DropIndex(
                name: "IX_IFPageControl_IFPageListView_FormLayoutId",
                table: "IFPageControl");

            migrationBuilder.DropIndex(
                name: "IX_IFPageControl_IFPageListView_IFModelId",
                table: "IFPageControl");

            migrationBuilder.DropColumn(
                name: "IFPageListView_FormLayoutId",
                table: "IFPageControl");

            migrationBuilder.DropColumn(
                name: "IFPageListView_IFModelId",
                table: "IFPageControl");
        }
    }
}
