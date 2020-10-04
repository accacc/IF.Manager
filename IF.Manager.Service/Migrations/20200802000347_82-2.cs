using Microsoft.EntityFrameworkCore.Migrations;

namespace IF.Manager.Persistence.EF.Migrations
{
    public partial class _822 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_IFPageControl_IFModel_IFPageListView_IFModelId",
                table: "IFPageControl");

            migrationBuilder.DropForeignKey(
                name: "FK_IFPageControl_IFQuery_IFQueryId",
                table: "IFPageControl");

            migrationBuilder.DropIndex(
                name: "IX_IFPageControl_IFPageListView_IFModelId",
                table: "IFPageControl");

            migrationBuilder.DropColumn(
                name: "IFPageListView_IFModelId",
                table: "IFPageControl");

            migrationBuilder.AddColumn<int>(
                name: "IFPageNameValueControl_IFQueryId",
                table: "IFPageControl",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_IFPageControl_IFPageNameValueControl_IFQueryId",
                table: "IFPageControl",
                column: "IFPageNameValueControl_IFQueryId");

            migrationBuilder.AddForeignKey(
                name: "FK_IFPageControl_IFQuery_IFQueryId",
                table: "IFPageControl",
                column: "IFQueryId",
                principalTable: "IFQuery",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_IFPageControl_IFQuery_IFPageNameValueControl_IFQueryId",
                table: "IFPageControl",
                column: "IFPageNameValueControl_IFQueryId",
                principalTable: "IFQuery",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_IFPageControl_IFQuery_IFQueryId",
                table: "IFPageControl");

            migrationBuilder.DropForeignKey(
                name: "FK_IFPageControl_IFQuery_IFPageNameValueControl_IFQueryId",
                table: "IFPageControl");

            migrationBuilder.DropIndex(
                name: "IX_IFPageControl_IFPageNameValueControl_IFQueryId",
                table: "IFPageControl");

            migrationBuilder.DropColumn(
                name: "IFPageNameValueControl_IFQueryId",
                table: "IFPageControl");

            migrationBuilder.AddColumn<int>(
                name: "IFPageListView_IFModelId",
                table: "IFPageControl",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_IFPageControl_IFPageListView_IFModelId",
                table: "IFPageControl",
                column: "IFPageListView_IFModelId");

            migrationBuilder.AddForeignKey(
                name: "FK_IFPageControl_IFModel_IFPageListView_IFModelId",
                table: "IFPageControl",
                column: "IFPageListView_IFModelId",
                principalTable: "IFModel",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_IFPageControl_IFQuery_IFQueryId",
                table: "IFPageControl",
                column: "IFQueryId",
                principalTable: "IFQuery",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
