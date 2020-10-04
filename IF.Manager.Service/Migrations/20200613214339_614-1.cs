using Microsoft.EntityFrameworkCore.Migrations;

namespace IF.Manager.Persistence.EF.Migrations
{
    public partial class _6141 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "IFPageGridHeaderAction");

            migrationBuilder.DropTable(
                name: "IFPageGridRowAction");

            migrationBuilder.DropColumn(
                name: "IFPageGrid_Description",
                table: "IFPageControl");

            migrationBuilder.DropColumn(
                name: "IFPageGrid_Name",
                table: "IFPageControl");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "IFPageControl",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "IFPageControl",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Style",
                table: "IFPageControl",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Text",
                table: "IFPageControl",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Url",
                table: "IFPageControl",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "WidgetRenderType",
                table: "IFPageControl",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "WidgetType",
                table: "IFPageControl",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Style",
                table: "IFPageControl");

            migrationBuilder.DropColumn(
                name: "Text",
                table: "IFPageControl");

            migrationBuilder.DropColumn(
                name: "Url",
                table: "IFPageControl");

            migrationBuilder.DropColumn(
                name: "WidgetRenderType",
                table: "IFPageControl");

            migrationBuilder.DropColumn(
                name: "WidgetType",
                table: "IFPageControl");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "IFPageControl",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "IFPageControl",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AddColumn<string>(
                name: "IFPageGrid_Description",
                table: "IFPageControl",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "IFPageGrid_Name",
                table: "IFPageControl",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "IFPageGridHeaderAction",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    GridId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Style = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Text = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Url = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    WidgetRenderType = table.Column<int>(type: "int", nullable: false),
                    WidgetType = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IFPageGridHeaderAction", x => x.Id);
                    table.ForeignKey(
                        name: "FK_IFPageGridHeaderAction_IFPageControl_GridId",
                        column: x => x.GridId,
                        principalTable: "IFPageControl",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "IFPageGridRowAction",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    GridId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Style = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Text = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Url = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    WidgetRenderType = table.Column<int>(type: "int", nullable: false),
                    WidgetType = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IFPageGridRowAction", x => x.Id);
                    table.ForeignKey(
                        name: "FK_IFPageGridRowAction_IFPageControl_GridId",
                        column: x => x.GridId,
                        principalTable: "IFPageControl",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_IFPageGridHeaderAction_GridId",
                table: "IFPageGridHeaderAction",
                column: "GridId");

            migrationBuilder.CreateIndex(
                name: "IX_IFPageGridRowAction_GridId",
                table: "IFPageGridRowAction",
                column: "GridId");
        }
    }
}
