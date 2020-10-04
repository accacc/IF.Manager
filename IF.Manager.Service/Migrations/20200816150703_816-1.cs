using Microsoft.EntityFrameworkCore.Migrations;

namespace IF.Manager.Persistence.EF.Migrations
{
    public partial class _8161 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "IFPageListView_IFQueryId",
                table: "IFPageControl",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_IFPageControl_IFPageListView_IFQueryId",
                table: "IFPageControl",
                column: "IFPageListView_IFQueryId");

            migrationBuilder.AddForeignKey(
                name: "FK_IFPageControl_IFQuery_IFPageListView_IFQueryId",
                table: "IFPageControl",
                column: "IFPageListView_IFQueryId",
                principalTable: "IFQuery",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_IFPageControl_IFQuery_IFPageListView_IFQueryId",
                table: "IFPageControl");

            migrationBuilder.DropIndex(
                name: "IX_IFPageControl_IFPageListView_IFQueryId",
                table: "IFPageControl");

            migrationBuilder.DropColumn(
                name: "IFPageListView_IFQueryId",
                table: "IFPageControl");
        }
    }
}
