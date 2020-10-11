using Microsoft.EntityFrameworkCore.Migrations;

namespace IF.Manager.Persistence.EF.Migrations
{
    public partial class _10101 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.AddForeignKey(
                name: "FK_IFPageFormItemModelProperty_IFPageControl_ObjectId1",
                table: "IFPageFormItemModelProperty",
                column: "ObjectId",
                principalTable: "IFPageControl",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_IFPageFormItemModelProperty_IFPageControl_ObjectId1",
                table: "IFPageFormItemModelProperty");

            migrationBuilder.AddColumn<int>(
                name: "IFPageGridId",
                table: "IFPageFormItemModelProperty",
                type: "int",
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
    }
}
