using Microsoft.EntityFrameworkCore.Migrations;

namespace IF.Manager.Persistence.EF.Migrations
{
    public partial class _6142 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SortOrder",
                table: "IFPageControl",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "FormLayoutId",
                table: "IFPageControl",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "IFPageFormLayout",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: false),
                    Description = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IFPageFormLayout", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_IFPageControl_FormLayoutId",
                table: "IFPageControl",
                column: "FormLayoutId");

            migrationBuilder.AddForeignKey(
                name: "FK_IFPageControl_IFPageFormLayout_FormLayoutId",
                table: "IFPageControl",
                column: "FormLayoutId",
                principalTable: "IFPageFormLayout",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_IFPageControl_IFPageFormLayout_FormLayoutId",
                table: "IFPageControl");

            migrationBuilder.DropTable(
                name: "IFPageFormLayout");

            migrationBuilder.DropIndex(
                name: "IX_IFPageControl_FormLayoutId",
                table: "IFPageControl");

            migrationBuilder.DropColumn(
                name: "SortOrder",
                table: "IFPageControl");

            migrationBuilder.DropColumn(
                name: "FormLayoutId",
                table: "IFPageControl");
        }
    }
}
