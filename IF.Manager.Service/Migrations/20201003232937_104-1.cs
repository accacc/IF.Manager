using Microsoft.EntityFrameworkCore.Migrations;

namespace IF.Manager.Persistence.EF.Migrations
{
    public partial class _1041 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_IFPageFormItemModelProperty_IFModelProperty_IFModelPropertyId",
                table: "IFPageFormItemModelProperty");

            migrationBuilder.DropForeignKey(
                name: "FK_IFPageFormItemModelProperty_IFPageFormItem_IFPageFormItemId",
                table: "IFPageFormItemModelProperty");

            migrationBuilder.DropForeignKey(
                name: "FK_IFPageFormItemModelProperty_IFPageControl_ObjectId",
                table: "IFPageFormItemModelProperty");

            migrationBuilder.DropTable(
                name: "IFPageControlItemModelProperty");

            migrationBuilder.DropPrimaryKey(
                name: "PK_IFPageFormItemModelProperty",
                table: "IFPageFormItemModelProperty");

            migrationBuilder.RenameTable(
                name: "IFPageFormItemModelProperty",
                newName: "PageFormItemModelProperties");

            migrationBuilder.RenameIndex(
                name: "IX_IFPageFormItemModelProperty_ObjectId",
                table: "PageFormItemModelProperties",
                newName: "IX_PageFormItemModelProperties_ObjectId");

            migrationBuilder.RenameIndex(
                name: "IX_IFPageFormItemModelProperty_IFPageFormItemId",
                table: "PageFormItemModelProperties",
                newName: "IX_PageFormItemModelProperties_IFPageFormItemId");

            migrationBuilder.RenameIndex(
                name: "IX_IFPageFormItemModelProperty_IFModelPropertyId",
                table: "PageFormItemModelProperties",
                newName: "IX_PageFormItemModelProperties_IFModelPropertyId");

            migrationBuilder.AddColumn<int>(
                name: "IFQueryId",
                table: "PageFormItemModelProperties",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "NameIFModelPropertyId",
                table: "PageFormItemModelProperties",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ValueIFModelPropertyId",
                table: "PageFormItemModelProperties",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_PageFormItemModelProperties",
                table: "PageFormItemModelProperties",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_PageFormItemModelProperties_IFQueryId",
                table: "PageFormItemModelProperties",
                column: "IFQueryId");

            migrationBuilder.CreateIndex(
                name: "IX_PageFormItemModelProperties_NameIFModelPropertyId",
                table: "PageFormItemModelProperties",
                column: "NameIFModelPropertyId");

            migrationBuilder.CreateIndex(
                name: "IX_PageFormItemModelProperties_ValueIFModelPropertyId",
                table: "PageFormItemModelProperties",
                column: "ValueIFModelPropertyId");

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

        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.DropIndex(
                name: "IX_PageFormItemModelProperties_IFQueryId",
                table: "PageFormItemModelProperties");

            migrationBuilder.DropIndex(
                name: "IX_PageFormItemModelProperties_NameIFModelPropertyId",
                table: "PageFormItemModelProperties");

            migrationBuilder.DropIndex(
                name: "IX_PageFormItemModelProperties_ValueIFModelPropertyId",
                table: "PageFormItemModelProperties");

            migrationBuilder.DropColumn(
                name: "IFQueryId",
                table: "PageFormItemModelProperties");

            migrationBuilder.DropColumn(
                name: "NameIFModelPropertyId",
                table: "PageFormItemModelProperties");

            migrationBuilder.DropColumn(
                name: "ValueIFModelPropertyId",
                table: "PageFormItemModelProperties");

            migrationBuilder.RenameTable(
                name: "PageFormItemModelProperties",
                newName: "IFPageFormItemModelProperty");

            migrationBuilder.RenameIndex(
                name: "IX_PageFormItemModelProperties_ObjectId",
                table: "IFPageFormItemModelProperty",
                newName: "IX_IFPageFormItemModelProperty_ObjectId");

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

            migrationBuilder.CreateTable(
                name: "IFPageControlItemModelProperty",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IFPageFormId = table.Column<int>(type: "int", nullable: true),
                    IFPageFormItemModelPropertyId = table.Column<int>(type: "int", nullable: false),
                    IFQueryId = table.Column<int>(type: "int", nullable: false),
                    NameIFModelPropertyId = table.Column<int>(type: "int", nullable: true),
                    ObjectId = table.Column<int>(type: "int", nullable: true),
                    ValueIFModelPropertyId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IFPageControlItemModelProperty", x => x.Id);
                    table.ForeignKey(
                        name: "FK_IFPageControlItemModelProperty_IFPageControl_IFPageFormId",
                        column: x => x.IFPageFormId,
                        principalTable: "IFPageControl",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_IFPageControlItemModelProperty_IFQuery_IFQueryId",
                        column: x => x.IFQueryId,
                        principalTable: "IFQuery",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_IFPageControlItemModelProperty_IFModelProperty_NameIFModelPropertyId",
                        column: x => x.NameIFModelPropertyId,
                        principalTable: "IFModelProperty",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_IFPageControlItemModelProperty_IFPageFormItemModelProperty_ObjectId",
                        column: x => x.ObjectId,
                        principalTable: "IFPageFormItemModelProperty",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_IFPageControlItemModelProperty_IFModelProperty_ValueIFModelPropertyId",
                        column: x => x.ValueIFModelPropertyId,
                        principalTable: "IFModelProperty",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_IFPageControlItemModelProperty_IFPageFormId",
                table: "IFPageControlItemModelProperty",
                column: "IFPageFormId");

            migrationBuilder.CreateIndex(
                name: "IX_IFPageControlItemModelProperty_IFQueryId",
                table: "IFPageControlItemModelProperty",
                column: "IFQueryId");

            migrationBuilder.CreateIndex(
                name: "IX_IFPageControlItemModelProperty_NameIFModelPropertyId",
                table: "IFPageControlItemModelProperty",
                column: "NameIFModelPropertyId");

            migrationBuilder.CreateIndex(
                name: "IX_IFPageControlItemModelProperty_ObjectId",
                table: "IFPageControlItemModelProperty",
                column: "ObjectId");

            migrationBuilder.CreateIndex(
                name: "IX_IFPageControlItemModelProperty_ValueIFModelPropertyId",
                table: "IFPageControlItemModelProperty",
                column: "ValueIFModelPropertyId");

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
                name: "FK_IFPageFormItemModelProperty_IFPageControl_ObjectId",
                table: "IFPageFormItemModelProperty",
                column: "ObjectId",
                principalTable: "IFPageControl",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
