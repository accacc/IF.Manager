using Microsoft.EntityFrameworkCore.Migrations;

namespace IF.Manager.Persistence.EF.Migrations
{
    public partial class _6163 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "IFModelPropertyId",
                table: "IFPageFormItemModelProperty",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "IFPageFormId",
                table: "IFPageFormItemModelProperty",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_IFPageFormItemModelProperty_IFModelPropertyId",
                table: "IFPageFormItemModelProperty",
                column: "IFModelPropertyId");

            migrationBuilder.CreateIndex(
                name: "IX_IFPageFormItemModelProperty_IFPageFormId",
                table: "IFPageFormItemModelProperty",
                column: "IFPageFormId");

            migrationBuilder.AddForeignKey(
                name: "FK_IFPageFormItemModelProperty_IFModelProperty_IFModelPropertyId",
                table: "IFPageFormItemModelProperty",
                column: "IFModelPropertyId",
                principalTable: "IFModelProperty",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_IFPageFormItemModelProperty_IFPageControl_IFPageFormId",
                table: "IFPageFormItemModelProperty",
                column: "IFPageFormId",
                principalTable: "IFPageControl",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_IFPageFormItemModelProperty_IFModelProperty_IFModelPropertyId",
                table: "IFPageFormItemModelProperty");

            migrationBuilder.DropForeignKey(
                name: "FK_IFPageFormItemModelProperty_IFPageControl_IFPageFormId",
                table: "IFPageFormItemModelProperty");

            migrationBuilder.DropIndex(
                name: "IX_IFPageFormItemModelProperty_IFModelPropertyId",
                table: "IFPageFormItemModelProperty");

            migrationBuilder.DropIndex(
                name: "IX_IFPageFormItemModelProperty_IFPageFormId",
                table: "IFPageFormItemModelProperty");

            migrationBuilder.DropColumn(
                name: "IFModelPropertyId",
                table: "IFPageFormItemModelProperty");

            migrationBuilder.DropColumn(
                name: "IFPageFormId",
                table: "IFPageFormItemModelProperty");
        }
    }
}
