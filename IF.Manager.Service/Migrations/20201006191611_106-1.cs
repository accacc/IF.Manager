using Microsoft.EntityFrameworkCore.Migrations;

namespace IF.Manager.Persistence.EF.Migrations
{
    public partial class _1061 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "IFPageGridId",
                table: "IFPageFormItemModelProperty",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_IFPageFormItemModelProperty_IFPageGridId",
                table: "IFPageFormItemModelProperty",
                column: "IFPageGridId");

            migrationBuilder.AddForeignKey(
                name: "FK_IFPageFormItemModelProperty_IFPageControl_IFPageGridId",
                table: "IFPageFormItemModelProperty",
                column: "IFPageGridId",
                principalTable: "IFPageControl",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_IFPageFormItemModelProperty_IFPageControl_IFPageGridId",
                table: "IFPageFormItemModelProperty");

            migrationBuilder.DropIndex(
                name: "IX_IFPageFormItemModelProperty_IFPageGridId",
                table: "IFPageFormItemModelProperty");

            migrationBuilder.DropColumn(
                name: "IFPageGridId",
                table: "IFPageFormItemModelProperty");
        }
    }
}
