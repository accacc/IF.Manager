using Microsoft.EntityFrameworkCore.Migrations;

namespace IF.Manager.Persistence.EF.Migrations
{
    public partial class _6161 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "IFPageFormItem",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: false),
                    Description = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IFPageFormItem", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "IFPageFormItemModelProperty",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IFPageFormItemId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IFPageFormItemModelProperty", x => x.Id);
                    table.ForeignKey(
                        name: "FK_IFPageFormItemModelProperty_IFPageFormItem_IFPageFormItemId",
                        column: x => x.IFPageFormItemId,
                        principalTable: "IFPageFormItem",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_IFPageFormItemModelProperty_IFPageFormItemId",
                table: "IFPageFormItemModelProperty",
                column: "IFPageFormItemId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "IFPageFormItemModelProperty");

            migrationBuilder.DropTable(
                name: "IFPageFormItem");
        }
    }
}
