using Microsoft.EntityFrameworkCore.Migrations;

namespace IF.Manager.Persistence.EF.Migrations
{
    public partial class _11051 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "IFCustomClass",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: false),
                    Description = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IFCustomClass", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "IFCustomClassProperty",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: false),
                    IFCustomClassId = table.Column<int>(nullable: false),
                    Type = table.Column<string>(nullable: true),
                    IsNullable = table.Column<bool>(nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IFCustomClassProperty", x => x.Id);
                    table.ForeignKey(
                        name: "FK_IFCustomClassProperty_IFCustomClass_IFCustomClassId",
                        column: x => x.IFCustomClassId,
                        principalTable: "IFCustomClass",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_IFCustomClassProperty_IFCustomClassId",
                table: "IFCustomClassProperty",
                column: "IFCustomClassId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "IFCustomClassProperty");

            migrationBuilder.DropTable(
                name: "IFCustomClass");
        }
    }
}
