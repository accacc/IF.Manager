using Microsoft.EntityFrameworkCore.Migrations;

namespace IF.Manager.Persistence.EF.Migrations
{
    public partial class _6146 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_IFPageControl_IFPageFormLayout_FormLayoutId",
                table: "IFPageControl");

            migrationBuilder.AddColumn<int>(
                name: "IFModelId",
                table: "IFPageControl",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_IFPageControl_IFModelId",
                table: "IFPageControl",
                column: "IFModelId");

            migrationBuilder.AddForeignKey(
                name: "FK_IFPageControl_IFPageFormLayout_FormLayoutId",
                table: "IFPageControl",
                column: "FormLayoutId",
                principalTable: "IFPageFormLayout",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_IFPageControl_IFModel_IFModelId",
                table: "IFPageControl",
                column: "IFModelId",
                principalTable: "IFModel",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_IFPageControl_IFPageFormLayout_FormLayoutId",
                table: "IFPageControl");

            migrationBuilder.DropForeignKey(
                name: "FK_IFPageControl_IFModel_IFModelId",
                table: "IFPageControl");

            migrationBuilder.DropIndex(
                name: "IX_IFPageControl_IFModelId",
                table: "IFPageControl");

            migrationBuilder.DropColumn(
                name: "IFModelId",
                table: "IFPageControl");

            migrationBuilder.AddForeignKey(
                name: "FK_IFPageControl_IFPageFormLayout_FormLayoutId",
                table: "IFPageControl",
                column: "FormLayoutId",
                principalTable: "IFPageFormLayout",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
