using Microsoft.EntityFrameworkCore.Migrations;

namespace IF.Manager.Persistence.EF.Migrations
{
    public partial class _122420004 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "IFClassMapping",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IFClassMapperId = table.Column<int>(nullable: false),
                    FromPropertyId = table.Column<int>(nullable: true),
                    ToPropertyId = table.Column<int>(nullable: true),
                    IsList = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IFClassMapping", x => x.Id);
                    table.ForeignKey(
                        name: "FK_IFClassMapping_IFClass_FromPropertyId",
                        column: x => x.FromPropertyId,
                        principalTable: "IFClass",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_IFClassMapping_IFClassMapper_IFClassMapperId",
                        column: x => x.IFClassMapperId,
                        principalTable: "IFClassMapper",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_IFClassMapping_IFClass_ToPropertyId",
                        column: x => x.ToPropertyId,
                        principalTable: "IFClass",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_IFClassMapping_FromPropertyId",
                table: "IFClassMapping",
                column: "FromPropertyId");

            migrationBuilder.CreateIndex(
                name: "IX_IFClassMapping_IFClassMapperId",
                table: "IFClassMapping",
                column: "IFClassMapperId");

            migrationBuilder.CreateIndex(
                name: "IX_IFClassMapping_ToPropertyId",
                table: "IFClassMapping",
                column: "ToPropertyId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "IFClassMapping");
        }
    }
}
