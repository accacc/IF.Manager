using Microsoft.EntityFrameworkCore.Migrations;

namespace IF.Manager.Persistence.EF.Migrations
{
    public partial class _1042 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PageFormItemModelProperties_IFModelProperty_IFModelPropertyId",
                table: "PageFormItemModelProperties");

            migrationBuilder.DropForeignKey(
                name: "FK_PageFormItemModelProperties_IFPageFormItem_IFPageFormItemId",
                table: "PageFormItemModelProperties");

            migrationBuilder.DropForeignKey(
                name: "FK_PageFormItemModelProperties_IFQuery_IFQueryId",
                table: "PageFormItemModelProperties");

            migrationBuilder.DropForeignKey(
                name: "FK_PageFormItemModelProperties_IFModelProperty_NameIFModelPropertyId",
                table: "PageFormItemModelProperties");

            migrationBuilder.DropForeignKey(
                name: "FK_PageFormItemModelProperties_IFPageControl_ObjectId",
                table: "PageFormItemModelProperties");

            migrationBuilder.DropForeignKey(
                name: "FK_PageFormItemModelProperties_IFModelProperty_ValueIFModelPropertyId",
                table: "PageFormItemModelProperties");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PageFormItemModelProperties",
                table: "PageFormItemModelProperties");

            migrationBuilder.RenameTable(
                name: "PageFormItemModelProperties",
                newName: "IFPageFormItemModelProperty");

            migrationBuilder.RenameIndex(
                name: "IX_PageFormItemModelProperties_ValueIFModelPropertyId",
                table: "IFPageFormItemModelProperty",
                newName: "IX_IFPageFormItemModelProperty_ValueIFModelPropertyId");

            migrationBuilder.RenameIndex(
                name: "IX_PageFormItemModelProperties_ObjectId",
                table: "IFPageFormItemModelProperty",
                newName: "IX_IFPageFormItemModelProperty_ObjectId");

            migrationBuilder.RenameIndex(
                name: "IX_PageFormItemModelProperties_NameIFModelPropertyId",
                table: "IFPageFormItemModelProperty",
                newName: "IX_IFPageFormItemModelProperty_NameIFModelPropertyId");

            migrationBuilder.RenameIndex(
                name: "IX_PageFormItemModelProperties_IFQueryId",
                table: "IFPageFormItemModelProperty",
                newName: "IX_IFPageFormItemModelProperty_IFQueryId");

            migrationBuilder.RenameIndex(
                name: "IX_PageFormItemModelProperties_IFPageFormItemId",
                table: "IFPageFormItemModelProperty",
                newName: "IX_IFPageFormItemModelProperty_IFPageFormItemId");

            migrationBuilder.RenameIndex(
                name: "IX_PageFormItemModelProperties_IFModelPropertyId",
                table: "IFPageFormItemModelProperty",
                newName: "IX_IFPageFormItemModelProperty_IFModelPropertyId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_IFPageFormItemModelProperty",
                table: "IFPageFormItemModelProperty",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_IFPageFormItemModelProperty_IFModelProperty_IFModelPropertyId",
                table: "IFPageFormItemModelProperty",
                column: "IFModelPropertyId",
                principalTable: "IFModelProperty",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_IFPageFormItemModelProperty_IFPageFormItem_IFPageFormItemId",
                table: "IFPageFormItemModelProperty",
                column: "IFPageFormItemId",
                principalTable: "IFPageFormItem",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_IFPageFormItemModelProperty_IFQuery_IFQueryId",
                table: "IFPageFormItemModelProperty",
                column: "IFQueryId",
                principalTable: "IFQuery",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_IFPageFormItemModelProperty_IFModelProperty_NameIFModelPropertyId",
                table: "IFPageFormItemModelProperty",
                column: "NameIFModelPropertyId",
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

            migrationBuilder.AddForeignKey(
                name: "FK_IFPageFormItemModelProperty_IFModelProperty_ValueIFModelPropertyId",
                table: "IFPageFormItemModelProperty",
                column: "ValueIFModelPropertyId",
                principalTable: "IFModelProperty",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_IFPageFormItemModelProperty_IFModelProperty_IFModelPropertyId",
                table: "IFPageFormItemModelProperty");

            migrationBuilder.DropForeignKey(
                name: "FK_IFPageFormItemModelProperty_IFPageFormItem_IFPageFormItemId",
                table: "IFPageFormItemModelProperty");

            migrationBuilder.DropForeignKey(
                name: "FK_IFPageFormItemModelProperty_IFQuery_IFQueryId",
                table: "IFPageFormItemModelProperty");

            migrationBuilder.DropForeignKey(
                name: "FK_IFPageFormItemModelProperty_IFModelProperty_NameIFModelPropertyId",
                table: "IFPageFormItemModelProperty");

            migrationBuilder.DropForeignKey(
                name: "FK_IFPageFormItemModelProperty_IFPageControl_ObjectId",
                table: "IFPageFormItemModelProperty");

            migrationBuilder.DropForeignKey(
                name: "FK_IFPageFormItemModelProperty_IFModelProperty_ValueIFModelPropertyId",
                table: "IFPageFormItemModelProperty");

            migrationBuilder.DropPrimaryKey(
                name: "PK_IFPageFormItemModelProperty",
                table: "IFPageFormItemModelProperty");

            migrationBuilder.RenameTable(
                name: "IFPageFormItemModelProperty",
                newName: "PageFormItemModelProperties");

            migrationBuilder.RenameIndex(
                name: "IX_IFPageFormItemModelProperty_ValueIFModelPropertyId",
                table: "PageFormItemModelProperties",
                newName: "IX_PageFormItemModelProperties_ValueIFModelPropertyId");

            migrationBuilder.RenameIndex(
                name: "IX_IFPageFormItemModelProperty_ObjectId",
                table: "PageFormItemModelProperties",
                newName: "IX_PageFormItemModelProperties_ObjectId");

            migrationBuilder.RenameIndex(
                name: "IX_IFPageFormItemModelProperty_NameIFModelPropertyId",
                table: "PageFormItemModelProperties",
                newName: "IX_PageFormItemModelProperties_NameIFModelPropertyId");

            migrationBuilder.RenameIndex(
                name: "IX_IFPageFormItemModelProperty_IFQueryId",
                table: "PageFormItemModelProperties",
                newName: "IX_PageFormItemModelProperties_IFQueryId");

            migrationBuilder.RenameIndex(
                name: "IX_IFPageFormItemModelProperty_IFPageFormItemId",
                table: "PageFormItemModelProperties",
                newName: "IX_PageFormItemModelProperties_IFPageFormItemId");

            migrationBuilder.RenameIndex(
                name: "IX_IFPageFormItemModelProperty_IFModelPropertyId",
                table: "PageFormItemModelProperties",
                newName: "IX_PageFormItemModelProperties_IFModelPropertyId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PageFormItemModelProperties",
                table: "PageFormItemModelProperties",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PageFormItemModelProperties_IFModelProperty_IFModelPropertyId",
                table: "PageFormItemModelProperties",
                column: "IFModelPropertyId",
                principalTable: "IFModelProperty",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PageFormItemModelProperties_IFPageFormItem_IFPageFormItemId",
                table: "PageFormItemModelProperties",
                column: "IFPageFormItemId",
                principalTable: "IFPageFormItem",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PageFormItemModelProperties_IFQuery_IFQueryId",
                table: "PageFormItemModelProperties",
                column: "IFQueryId",
                principalTable: "IFQuery",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PageFormItemModelProperties_IFModelProperty_NameIFModelPropertyId",
                table: "PageFormItemModelProperties",
                column: "NameIFModelPropertyId",
                principalTable: "IFModelProperty",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PageFormItemModelProperties_IFPageControl_ObjectId",
                table: "PageFormItemModelProperties",
                column: "ObjectId",
                principalTable: "IFPageControl",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PageFormItemModelProperties_IFModelProperty_ValueIFModelPropertyId",
                table: "PageFormItemModelProperties",
                column: "ValueIFModelPropertyId",
                principalTable: "IFModelProperty",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
