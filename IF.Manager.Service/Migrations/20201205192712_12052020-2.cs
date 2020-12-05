using Microsoft.EntityFrameworkCore.Migrations;

namespace IF.Manager.Persistence.EF.Migrations
{
    public partial class _120520202 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "IFKClass");

            migrationBuilder.CreateTable(
                name: "IFClass",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ParentId = table.Column<int>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    Type = table.Column<string>(nullable: false),
                    GenericType = table.Column<string>(nullable: true),
                    IsNullable = table.Column<bool>(nullable: false),
                    IsPrimitive = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IFClass", x => x.Id);
                    table.ForeignKey(
                        name: "FK_IFClass_IFClass_ParentId",
                        column: x => x.ParentId,
                        principalTable: "IFClass",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "IFClassMapper",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IFClassMapper", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_IFClass_ParentId",
                table: "IFClass",
                column: "ParentId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "IFClass");

            migrationBuilder.DropTable(
                name: "IFClassMapper");

            migrationBuilder.CreateTable(
                name: "IFKClass",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    GenericType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsNullable = table.Column<bool>(type: "bit", nullable: false),
                    IsPrimitive = table.Column<bool>(type: "bit", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ParentId = table.Column<int>(type: "int", nullable: true),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false)
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
    }
}
