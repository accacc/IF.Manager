using Microsoft.EntityFrameworkCore.Migrations;

namespace IF.Manager.Persistence.EF.Migrations
{
    public partial class _9151 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "IFPageControlItemModelProperty",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IFPageFormItemModelPropertyId = table.Column<int>(nullable: false),
                    IFPageFormId = table.Column<int>(nullable: true),
                    ObjectId = table.Column<int>(nullable: true),
                    IFQueryId = table.Column<int>(nullable: false),
                    NameIFModelPropertyId = table.Column<int>(nullable: true),
                    ValueIFModelPropertyId = table.Column<int>(nullable: true)
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
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "IFPageControlItemModelProperty");
        }
    }
}
