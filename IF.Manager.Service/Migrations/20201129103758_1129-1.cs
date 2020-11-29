using Microsoft.EntityFrameworkCore.Migrations;

namespace IF.Manager.Persistence.EF.Migrations
{
    public partial class _11291 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "IFKClass",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ParentId = table.Column<int>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    Type = table.Column<string>(nullable: false),
                    IsPrimitive = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IFKClass", x => x.Id);
                    table.ForeignKey(
                        name: "FK_IFKClass_IFKClass_ParentId",
                        column: x => x.ParentId,
                        principalTable: "IFKClass",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_IFKClass_ParentId",
                table: "IFKClass",
                column: "ParentId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "IFKClass");
        }
    }
}
