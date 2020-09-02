using Microsoft.EntityFrameworkCore.Migrations;

namespace IF.Manager.Persistence.EF.Migrations
{
    public partial class _921 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_IFPageFormItemModelProperty_IFPageControl_IFPageFormId",
                table: "IFPageFormItemModelProperty");

            migrationBuilder.DropIndex(
                name: "IX_IFPageFormItemModelProperty_IFPageFormId",
                table: "IFPageFormItemModelProperty");

            migrationBuilder.DropColumn(
                name: "IFPageFormId",
                table: "IFPageFormItemModelProperty");

            migrationBuilder.DropColumn(
                name: "NamePropertyId",
                table: "IFPageControl");

            migrationBuilder.DropColumn(
                name: "ValuePropertyId",
                table: "IFPageControl");

            migrationBuilder.AddColumn<int>(
                name: "ObjectId",
                table: "IFPageFormItemModelProperty",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "IFPageFormItemModelPropertyId",
                table: "IFPageControl",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "NameIFModelPropertyId",
                table: "IFPageControl",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ValueIFModelPropertyId",
                table: "IFPageControl",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ValueModelPropertyId",
                table: "IFPageControl",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_IFPageFormItemModelProperty_ObjectId",
                table: "IFPageFormItemModelProperty",
                column: "ObjectId");

            migrationBuilder.CreateIndex(
                name: "IX_IFPageControl_IFPageFormItemModelPropertyId",
                table: "IFPageControl",
                column: "IFPageFormItemModelPropertyId");

            migrationBuilder.CreateIndex(
                name: "IX_IFPageControl_NameIFModelPropertyId",
                table: "IFPageControl",
                column: "NameIFModelPropertyId");

            migrationBuilder.CreateIndex(
                name: "IX_IFPageControl_ValueModelPropertyId",
                table: "IFPageControl",
                column: "ValueModelPropertyId");

            migrationBuilder.AddForeignKey(
                name: "FK_IFPageControl_IFPageFormItemModelProperty_IFPageFormItemModelPropertyId",
                table: "IFPageControl",
                column: "IFPageFormItemModelPropertyId",
                principalTable: "IFPageFormItemModelProperty",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_IFPageControl_IFModelProperty_NameIFModelPropertyId",
                table: "IFPageControl",
                column: "NameIFModelPropertyId",
                principalTable: "IFModelProperty",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_IFPageControl_IFModelProperty_ValueModelPropertyId",
                table: "IFPageControl",
                column: "ValueModelPropertyId",
                principalTable: "IFModelProperty",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_IFPageFormItemModelProperty_IFPageControl_ObjectId",
                table: "IFPageFormItemModelProperty",
                column: "ObjectId",
                principalTable: "IFPageControl",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_IFPageControl_IFPageFormItemModelProperty_IFPageFormItemModelPropertyId",
                table: "IFPageControl");

            migrationBuilder.DropForeignKey(
                name: "FK_IFPageControl_IFModelProperty_NameIFModelPropertyId",
                table: "IFPageControl");

            migrationBuilder.DropForeignKey(
                name: "FK_IFPageControl_IFModelProperty_ValueModelPropertyId",
                table: "IFPageControl");

            migrationBuilder.DropForeignKey(
                name: "FK_IFPageFormItemModelProperty_IFPageControl_ObjectId",
                table: "IFPageFormItemModelProperty");

            migrationBuilder.DropIndex(
                name: "IX_IFPageFormItemModelProperty_ObjectId",
                table: "IFPageFormItemModelProperty");

            migrationBuilder.DropIndex(
                name: "IX_IFPageControl_IFPageFormItemModelPropertyId",
                table: "IFPageControl");

            migrationBuilder.DropIndex(
                name: "IX_IFPageControl_NameIFModelPropertyId",
                table: "IFPageControl");

            migrationBuilder.DropIndex(
                name: "IX_IFPageControl_ValueModelPropertyId",
                table: "IFPageControl");

            migrationBuilder.DropColumn(
                name: "ObjectId",
                table: "IFPageFormItemModelProperty");

            migrationBuilder.DropColumn(
                name: "IFPageFormItemModelPropertyId",
                table: "IFPageControl");

            migrationBuilder.DropColumn(
                name: "NameIFModelPropertyId",
                table: "IFPageControl");

            migrationBuilder.DropColumn(
                name: "ValueIFModelPropertyId",
                table: "IFPageControl");

            migrationBuilder.DropColumn(
                name: "ValueModelPropertyId",
                table: "IFPageControl");

            migrationBuilder.AddColumn<int>(
                name: "IFPageFormId",
                table: "IFPageFormItemModelProperty",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "NamePropertyId",
                table: "IFPageControl",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ValuePropertyId",
                table: "IFPageControl",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_IFPageFormItemModelProperty_IFPageFormId",
                table: "IFPageFormItemModelProperty",
                column: "IFPageFormId");

            migrationBuilder.AddForeignKey(
                name: "FK_IFPageFormItemModelProperty_IFPageControl_IFPageFormId",
                table: "IFPageFormItemModelProperty",
                column: "IFPageFormId",
                principalTable: "IFPageControl",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
