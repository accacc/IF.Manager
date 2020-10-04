using Microsoft.EntityFrameworkCore.Migrations;

namespace IF.Manager.Persistence.EF.Migrations
{
    public partial class _7282 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "IFPageActionRouteValue",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IFPageParameterId = table.Column<int>(nullable: false),
                    IFModelPropertyId = table.Column<int>(nullable: false),
                    IFPageActionId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IFPageActionRouteValue", x => x.Id);
                    table.ForeignKey(
                        name: "FK_IFPageActionRouteValue_IFModelProperty_IFModelPropertyId",
                        column: x => x.IFModelPropertyId,
                        principalTable: "IFModelProperty",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_IFPageActionRouteValue_IFPageControl_IFPageActionId",
                        column: x => x.IFPageActionId,
                        principalTable: "IFPageControl",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_IFPageActionRouteValue_IFPageParameter_IFPageParameterId",
                        column: x => x.IFPageParameterId,
                        principalTable: "IFPageParameter",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_IFPageActionRouteValue_IFModelPropertyId",
                table: "IFPageActionRouteValue",
                column: "IFModelPropertyId");

            migrationBuilder.CreateIndex(
                name: "IX_IFPageActionRouteValue_IFPageActionId",
                table: "IFPageActionRouteValue",
                column: "IFPageActionId");

            migrationBuilder.CreateIndex(
                name: "IX_IFPageActionRouteValue_IFPageParameterId",
                table: "IFPageActionRouteValue",
                column: "IFPageParameterId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "IFPageActionRouteValue");
        }
    }
}
