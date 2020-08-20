using Microsoft.EntityFrameworkCore.Migrations;

namespace IF.Manager.Persistence.EF.Migrations
{
    public partial class _5232 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "IFPageLayout",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: false),
                    Description = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IFPageLayout", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "IFPage",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: false),
                    Description = table.Column<string>(nullable: false),
                    ProcessId = table.Column<int>(nullable: true),
                    PageLayoutId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IFPage", x => x.Id);
                    table.ForeignKey(
                        name: "FK_IFPage_IFPageLayout_PageLayoutId",
                        column: x => x.PageLayoutId,
                        principalTable: "IFPageLayout",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_IFPage_IFProcess_ProcessId",
                        column: x => x.ProcessId,
                        principalTable: "IFProcess",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "IFPageGridLayout",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: false),
                    Description = table.Column<string>(nullable: false),
                    LayoutId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IFPageGridLayout", x => x.Id);
                    table.ForeignKey(
                        name: "FK_IFPageGridLayout_IFPageLayout_LayoutId",
                        column: x => x.LayoutId,
                        principalTable: "IFPageLayout",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "IFPageGrid",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: false),
                    Description = table.Column<string>(nullable: false),
                    ClientId = table.Column<string>(nullable: true),
                    QueryId = table.Column<int>(nullable: true),
                    GridLayoutId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IFPageGrid", x => x.Id);
                    table.ForeignKey(
                        name: "FK_IFPageGrid_IFPageGridLayout_GridLayoutId",
                        column: x => x.GridLayoutId,
                        principalTable: "IFPageGridLayout",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_IFPageGrid_IFQuery_QueryId",
                        column: x => x.QueryId,
                        principalTable: "IFQuery",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "IFPageGridHeaderAction",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: false),
                    Description = table.Column<string>(nullable: false),
                    Url = table.Column<string>(nullable: true),
                    Text = table.Column<string>(nullable: true),
                    Style = table.Column<string>(nullable: true),
                    WidgetType = table.Column<int>(nullable: false),
                    WidgetRenderType = table.Column<int>(nullable: false),
                    GridId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IFPageGridHeaderAction", x => x.Id);
                    table.ForeignKey(
                        name: "FK_IFPageGridHeaderAction_IFPageGrid_GridId",
                        column: x => x.GridId,
                        principalTable: "IFPageGrid",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "IFPageGridRowAction",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: false),
                    Description = table.Column<string>(nullable: false),
                    Url = table.Column<string>(nullable: true),
                    Text = table.Column<string>(nullable: true),
                    Style = table.Column<string>(nullable: true),
                    WidgetType = table.Column<int>(nullable: false),
                    WidgetRenderType = table.Column<int>(nullable: false),
                    GridId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IFPageGridRowAction", x => x.Id);
                    table.ForeignKey(
                        name: "FK_IFPageGridRowAction_IFPageGrid_GridId",
                        column: x => x.GridId,
                        principalTable: "IFPageGrid",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_IFPage_PageLayoutId",
                table: "IFPage",
                column: "PageLayoutId");

            migrationBuilder.CreateIndex(
                name: "IX_IFPage_ProcessId",
                table: "IFPage",
                column: "ProcessId");

            migrationBuilder.CreateIndex(
                name: "IX_IFPageGrid_GridLayoutId",
                table: "IFPageGrid",
                column: "GridLayoutId");

            migrationBuilder.CreateIndex(
                name: "IX_IFPageGrid_QueryId",
                table: "IFPageGrid",
                column: "QueryId");

            migrationBuilder.CreateIndex(
                name: "IX_IFPageGridHeaderAction_GridId",
                table: "IFPageGridHeaderAction",
                column: "GridId");

            migrationBuilder.CreateIndex(
                name: "IX_IFPageGridLayout_LayoutId",
                table: "IFPageGridLayout",
                column: "LayoutId");

            migrationBuilder.CreateIndex(
                name: "IX_IFPageGridRowAction_GridId",
                table: "IFPageGridRowAction",
                column: "GridId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "IFPage");

            migrationBuilder.DropTable(
                name: "IFPageGridHeaderAction");

            migrationBuilder.DropTable(
                name: "IFPageGridRowAction");

            migrationBuilder.DropTable(
                name: "IFPageGrid");

            migrationBuilder.DropTable(
                name: "IFPageGridLayout");

            migrationBuilder.DropTable(
                name: "IFPageLayout");
        }
    }
}
