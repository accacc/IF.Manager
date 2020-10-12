using Microsoft.EntityFrameworkCore.Migrations;

namespace IF.Manager.Persistence.EF.Migrations
{
    public partial class _10121 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_IFCommand_IFFormModel_FormModelId",
                table: "IFCommand");

            migrationBuilder.DropForeignKey(
                name: "FK_IFCommandFilterItem_IFFormModelProperty_FormModelPropertyId",
                table: "IFCommandFilterItem");

            migrationBuilder.DropForeignKey(
                name: "FK_IFCommandFilterItem_IFFormModel_IFFormModelId",
                table: "IFCommandFilterItem");

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
                name: "FK_IFPageFormItemModelProperty_IFPageControl_ObjectId1",
                table: "IFPageFormItemModelProperty");

            migrationBuilder.DropForeignKey(
                name: "FK_IFPageFormItemModelProperty_IFModelProperty_ValueIFModelPropertyId",
                table: "IFPageFormItemModelProperty");

            migrationBuilder.DropForeignKey(
                name: "FK_IFQuery_IFFormModel_FormModelId",
                table: "IFQuery");

            migrationBuilder.DropForeignKey(
                name: "FK_IFQueryFilterItem_IFFormModelProperty_FormModelPropertyId",
                table: "IFQueryFilterItem");

            migrationBuilder.DropForeignKey(
                name: "FK_IFQueryFilterItem_IFFormModel_IFFormModelId",
                table: "IFQueryFilterItem");

            migrationBuilder.DropTable(
                name: "IFFormModelProperty");

            migrationBuilder.DropTable(
                name: "IFFormModel");

            migrationBuilder.DropIndex(
                name: "IX_IFQueryFilterItem_FormModelPropertyId",
                table: "IFQueryFilterItem");

            migrationBuilder.DropIndex(
                name: "IX_IFQueryFilterItem_IFFormModelId",
                table: "IFQueryFilterItem");

            migrationBuilder.DropIndex(
                name: "IX_IFQuery_FormModelId",
                table: "IFQuery");

            migrationBuilder.DropIndex(
                name: "IX_IFCommandFilterItem_FormModelPropertyId",
                table: "IFCommandFilterItem");

            migrationBuilder.DropIndex(
                name: "IX_IFCommandFilterItem_IFFormModelId",
                table: "IFCommandFilterItem");

            migrationBuilder.DropIndex(
                name: "IX_IFCommand_FormModelId",
                table: "IFCommand");

            migrationBuilder.DropPrimaryKey(
                name: "PK_IFPageFormItemModelProperty",
                table: "IFPageFormItemModelProperty");

            migrationBuilder.DropColumn(
                name: "IFFormModelId",
                table: "IFQueryFilterItem");

            migrationBuilder.DropColumn(
                name: "FormModelId",
                table: "IFQuery");

            migrationBuilder.DropColumn(
                name: "IFFormModelId",
                table: "IFCommandFilterItem");

            migrationBuilder.DropColumn(
                name: "FormModelId",
                table: "IFCommand");

            migrationBuilder.RenameTable(
                name: "IFPageFormItemModelProperty",
                newName: "IFPageControlItemModelProperty");

            migrationBuilder.RenameIndex(
                name: "IX_IFPageFormItemModelProperty_ValueIFModelPropertyId",
                table: "IFPageControlItemModelProperty",
                newName: "IX_IFPageControlItemModelProperty_ValueIFModelPropertyId");

            migrationBuilder.RenameIndex(
                name: "IX_IFPageFormItemModelProperty_ObjectId",
                table: "IFPageControlItemModelProperty",
                newName: "IX_IFPageControlItemModelProperty_ObjectId");

            migrationBuilder.RenameIndex(
                name: "IX_IFPageFormItemModelProperty_NameIFModelPropertyId",
                table: "IFPageControlItemModelProperty",
                newName: "IX_IFPageControlItemModelProperty_NameIFModelPropertyId");

            migrationBuilder.RenameIndex(
                name: "IX_IFPageFormItemModelProperty_IFQueryId",
                table: "IFPageControlItemModelProperty",
                newName: "IX_IFPageControlItemModelProperty_IFQueryId");

            migrationBuilder.RenameIndex(
                name: "IX_IFPageFormItemModelProperty_IFPageFormItemId",
                table: "IFPageControlItemModelProperty",
                newName: "IX_IFPageControlItemModelProperty_IFPageFormItemId");

            migrationBuilder.RenameIndex(
                name: "IX_IFPageFormItemModelProperty_IFModelPropertyId",
                table: "IFPageControlItemModelProperty",
                newName: "IX_IFPageControlItemModelProperty_IFModelPropertyId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_IFPageControlItemModelProperty",
                table: "IFPageControlItemModelProperty",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_IFPageControlItemModelProperty_IFModelProperty_IFModelPropertyId",
                table: "IFPageControlItemModelProperty",
                column: "IFModelPropertyId",
                principalTable: "IFModelProperty",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_IFPageControlItemModelProperty_IFPageFormItem_IFPageFormItemId",
                table: "IFPageControlItemModelProperty",
                column: "IFPageFormItemId",
                principalTable: "IFPageFormItem",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_IFPageControlItemModelProperty_IFQuery_IFQueryId",
                table: "IFPageControlItemModelProperty",
                column: "IFQueryId",
                principalTable: "IFQuery",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_IFPageControlItemModelProperty_IFModelProperty_NameIFModelPropertyId",
                table: "IFPageControlItemModelProperty",
                column: "NameIFModelPropertyId",
                principalTable: "IFModelProperty",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_IFPageControlItemModelProperty_IFPageControl_ObjectId",
                table: "IFPageControlItemModelProperty",
                column: "ObjectId",
                principalTable: "IFPageControl",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_IFPageControlItemModelProperty_IFPageControl_ObjectId1",
                table: "IFPageControlItemModelProperty",
                column: "ObjectId",
                principalTable: "IFPageControl",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_IFPageControlItemModelProperty_IFModelProperty_ValueIFModelPropertyId",
                table: "IFPageControlItemModelProperty",
                column: "ValueIFModelPropertyId",
                principalTable: "IFModelProperty",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_IFPageControlItemModelProperty_IFModelProperty_IFModelPropertyId",
                table: "IFPageControlItemModelProperty");

            migrationBuilder.DropForeignKey(
                name: "FK_IFPageControlItemModelProperty_IFPageFormItem_IFPageFormItemId",
                table: "IFPageControlItemModelProperty");

            migrationBuilder.DropForeignKey(
                name: "FK_IFPageControlItemModelProperty_IFQuery_IFQueryId",
                table: "IFPageControlItemModelProperty");

            migrationBuilder.DropForeignKey(
                name: "FK_IFPageControlItemModelProperty_IFModelProperty_NameIFModelPropertyId",
                table: "IFPageControlItemModelProperty");

            migrationBuilder.DropForeignKey(
                name: "FK_IFPageControlItemModelProperty_IFPageControl_ObjectId",
                table: "IFPageControlItemModelProperty");

            migrationBuilder.DropForeignKey(
                name: "FK_IFPageControlItemModelProperty_IFPageControl_ObjectId1",
                table: "IFPageControlItemModelProperty");

            migrationBuilder.DropForeignKey(
                name: "FK_IFPageControlItemModelProperty_IFModelProperty_ValueIFModelPropertyId",
                table: "IFPageControlItemModelProperty");

            migrationBuilder.DropPrimaryKey(
                name: "PK_IFPageControlItemModelProperty",
                table: "IFPageControlItemModelProperty");

            migrationBuilder.RenameTable(
                name: "IFPageControlItemModelProperty",
                newName: "IFPageFormItemModelProperty");

            migrationBuilder.RenameIndex(
                name: "IX_IFPageControlItemModelProperty_ValueIFModelPropertyId",
                table: "IFPageFormItemModelProperty",
                newName: "IX_IFPageFormItemModelProperty_ValueIFModelPropertyId");

            migrationBuilder.RenameIndex(
                name: "IX_IFPageControlItemModelProperty_ObjectId",
                table: "IFPageFormItemModelProperty",
                newName: "IX_IFPageFormItemModelProperty_ObjectId");

            migrationBuilder.RenameIndex(
                name: "IX_IFPageControlItemModelProperty_NameIFModelPropertyId",
                table: "IFPageFormItemModelProperty",
                newName: "IX_IFPageFormItemModelProperty_NameIFModelPropertyId");

            migrationBuilder.RenameIndex(
                name: "IX_IFPageControlItemModelProperty_IFQueryId",
                table: "IFPageFormItemModelProperty",
                newName: "IX_IFPageFormItemModelProperty_IFQueryId");

            migrationBuilder.RenameIndex(
                name: "IX_IFPageControlItemModelProperty_IFPageFormItemId",
                table: "IFPageFormItemModelProperty",
                newName: "IX_IFPageFormItemModelProperty_IFPageFormItemId");

            migrationBuilder.RenameIndex(
                name: "IX_IFPageControlItemModelProperty_IFModelPropertyId",
                table: "IFPageFormItemModelProperty",
                newName: "IX_IFPageFormItemModelProperty_IFModelPropertyId");

            migrationBuilder.AddColumn<int>(
                name: "IFFormModelId",
                table: "IFQueryFilterItem",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "FormModelId",
                table: "IFQuery",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "IFFormModelId",
                table: "IFCommandFilterItem",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "FormModelId",
                table: "IFCommand",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_IFPageFormItemModelProperty",
                table: "IFPageFormItemModelProperty",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "IFFormModel",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IFFormModel", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "IFFormModelProperty",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FormModelId = table.Column<int>(type: "int", nullable: false),
                    IsNullable = table.Column<bool>(type: "bit", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IFFormModelProperty", x => x.Id);
                    table.ForeignKey(
                        name: "FK_IFFormModelProperty_IFFormModel_FormModelId",
                        column: x => x.FormModelId,
                        principalTable: "IFFormModel",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_IFQueryFilterItem_FormModelPropertyId",
                table: "IFQueryFilterItem",
                column: "FormModelPropertyId");

            migrationBuilder.CreateIndex(
                name: "IX_IFQueryFilterItem_IFFormModelId",
                table: "IFQueryFilterItem",
                column: "IFFormModelId");

            migrationBuilder.CreateIndex(
                name: "IX_IFQuery_FormModelId",
                table: "IFQuery",
                column: "FormModelId");

            migrationBuilder.CreateIndex(
                name: "IX_IFCommandFilterItem_FormModelPropertyId",
                table: "IFCommandFilterItem",
                column: "FormModelPropertyId");

            migrationBuilder.CreateIndex(
                name: "IX_IFCommandFilterItem_IFFormModelId",
                table: "IFCommandFilterItem",
                column: "IFFormModelId");

            migrationBuilder.CreateIndex(
                name: "IX_IFCommand_FormModelId",
                table: "IFCommand",
                column: "FormModelId");

            migrationBuilder.CreateIndex(
                name: "IX_IFFormModelProperty_FormModelId",
                table: "IFFormModelProperty",
                column: "FormModelId");

            migrationBuilder.AddForeignKey(
                name: "FK_IFCommand_IFFormModel_FormModelId",
                table: "IFCommand",
                column: "FormModelId",
                principalTable: "IFFormModel",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_IFCommandFilterItem_IFFormModelProperty_FormModelPropertyId",
                table: "IFCommandFilterItem",
                column: "FormModelPropertyId",
                principalTable: "IFFormModelProperty",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_IFCommandFilterItem_IFFormModel_IFFormModelId",
                table: "IFCommandFilterItem",
                column: "IFFormModelId",
                principalTable: "IFFormModel",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

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
                name: "FK_IFPageFormItemModelProperty_IFPageControl_ObjectId1",
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

            migrationBuilder.AddForeignKey(
                name: "FK_IFQuery_IFFormModel_FormModelId",
                table: "IFQuery",
                column: "FormModelId",
                principalTable: "IFFormModel",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_IFQueryFilterItem_IFFormModelProperty_FormModelPropertyId",
                table: "IFQueryFilterItem",
                column: "FormModelPropertyId",
                principalTable: "IFFormModelProperty",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_IFQueryFilterItem_IFFormModel_IFFormModelId",
                table: "IFQueryFilterItem",
                column: "IFFormModelId",
                principalTable: "IFFormModel",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
