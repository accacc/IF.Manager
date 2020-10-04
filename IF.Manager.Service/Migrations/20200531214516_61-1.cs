using Microsoft.EntityFrameworkCore.Migrations;

namespace IF.Manager.Persistence.EF.Migrations
{
    public partial class _611 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_IFPageGrid_IFPageGridLayout_GridLayoutId",
                table: "IFPageGrid");

            migrationBuilder.DropForeignKey(
                name: "FK_IFPageGrid_IFQuery_QueryId",
                table: "IFPageGrid");

            migrationBuilder.DropForeignKey(
                name: "FK_IFPageGridHeaderAction_IFPageGrid_GridId",
                table: "IFPageGridHeaderAction");

            migrationBuilder.DropForeignKey(
                name: "FK_IFPageGridRowAction_IFPageGrid_GridId",
                table: "IFPageGridRowAction");

            migrationBuilder.DropTable(
                name: "IFPage");

            migrationBuilder.DropPrimaryKey(
                name: "PK_IFPageGrid",
                table: "IFPageGrid");

            migrationBuilder.RenameTable(
                name: "IFPageGrid",
                newName: "PageControls");

            migrationBuilder.RenameIndex(
                name: "IX_IFPageGrid_QueryId",
                table: "PageControls",
                newName: "IX_PageControls_QueryId");

            migrationBuilder.RenameIndex(
                name: "IX_IFPageGrid_GridLayoutId",
                table: "PageControls",
                newName: "IX_PageControls_GridLayoutId");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "PageControls",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "PageControls",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<int>(
                name: "PageLayoutId",
                table: "PageControls",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ProcessId",
                table: "PageControls",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ControlType",
                table: "PageControls",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "PageControls",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "IFPageGrid_Description",
                table: "PageControls",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "IFPageGrid_Name",
                table: "PageControls",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_PageControls",
                table: "PageControls",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "IFPageControlMap",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ParentId = table.Column<int>(nullable: true),
                    PageControlId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IFPageControlMap", x => x.Id);
                    table.ForeignKey(
                        name: "FK_IFPageControlMap_PageControls_PageControlId",
                        column: x => x.PageControlId,
                        principalTable: "PageControls",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PageControls_PageLayoutId",
                table: "PageControls",
                column: "PageLayoutId");

            migrationBuilder.CreateIndex(
                name: "IX_PageControls_ProcessId",
                table: "PageControls",
                column: "ProcessId");

            migrationBuilder.CreateIndex(
                name: "IX_IFPageControlMap_PageControlId",
                table: "IFPageControlMap",
                column: "PageControlId");

            migrationBuilder.AddForeignKey(
                name: "FK_IFPageGridHeaderAction_PageControls_GridId",
                table: "IFPageGridHeaderAction",
                column: "GridId",
                principalTable: "PageControls",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_IFPageGridRowAction_PageControls_GridId",
                table: "IFPageGridRowAction",
                column: "GridId",
                principalTable: "PageControls",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PageControls_IFPageLayout_PageLayoutId",
                table: "PageControls",
                column: "PageLayoutId",
                principalTable: "IFPageLayout",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PageControls_IFProcess_ProcessId",
                table: "PageControls",
                column: "ProcessId",
                principalTable: "IFProcess",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PageControls_IFPageGridLayout_GridLayoutId",
                table: "PageControls",
                column: "GridLayoutId",
                principalTable: "IFPageGridLayout",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PageControls_IFQuery_QueryId",
                table: "PageControls",
                column: "QueryId",
                principalTable: "IFQuery",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_IFPageGridHeaderAction_PageControls_GridId",
                table: "IFPageGridHeaderAction");

            migrationBuilder.DropForeignKey(
                name: "FK_IFPageGridRowAction_PageControls_GridId",
                table: "IFPageGridRowAction");

            migrationBuilder.DropForeignKey(
                name: "FK_PageControls_IFPageLayout_PageLayoutId",
                table: "PageControls");

            migrationBuilder.DropForeignKey(
                name: "FK_PageControls_IFProcess_ProcessId",
                table: "PageControls");

            migrationBuilder.DropForeignKey(
                name: "FK_PageControls_IFPageGridLayout_GridLayoutId",
                table: "PageControls");

            migrationBuilder.DropForeignKey(
                name: "FK_PageControls_IFQuery_QueryId",
                table: "PageControls");

            migrationBuilder.DropTable(
                name: "IFPageControlMap");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PageControls",
                table: "PageControls");

            migrationBuilder.DropIndex(
                name: "IX_PageControls_PageLayoutId",
                table: "PageControls");

            migrationBuilder.DropIndex(
                name: "IX_PageControls_ProcessId",
                table: "PageControls");

            migrationBuilder.DropColumn(
                name: "PageLayoutId",
                table: "PageControls");

            migrationBuilder.DropColumn(
                name: "ProcessId",
                table: "PageControls");

            migrationBuilder.DropColumn(
                name: "ControlType",
                table: "PageControls");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "PageControls");

            migrationBuilder.DropColumn(
                name: "IFPageGrid_Description",
                table: "PageControls");

            migrationBuilder.DropColumn(
                name: "IFPageGrid_Name",
                table: "PageControls");

            migrationBuilder.RenameTable(
                name: "PageControls",
                newName: "IFPageGrid");

            migrationBuilder.RenameIndex(
                name: "IX_PageControls_QueryId",
                table: "IFPageGrid",
                newName: "IX_IFPageGrid_QueryId");

            migrationBuilder.RenameIndex(
                name: "IX_PageControls_GridLayoutId",
                table: "IFPageGrid",
                newName: "IX_IFPageGrid_GridLayoutId");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "IFPageGrid",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "IFPageGrid",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_IFPageGrid",
                table: "IFPageGrid",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "IFPage",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PageLayoutId = table.Column<int>(type: "int", nullable: false),
                    ProcessId = table.Column<int>(type: "int", nullable: true)
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

            migrationBuilder.CreateIndex(
                name: "IX_IFPage_PageLayoutId",
                table: "IFPage",
                column: "PageLayoutId");

            migrationBuilder.CreateIndex(
                name: "IX_IFPage_ProcessId",
                table: "IFPage",
                column: "ProcessId");

            migrationBuilder.AddForeignKey(
                name: "FK_IFPageGrid_IFPageGridLayout_GridLayoutId",
                table: "IFPageGrid",
                column: "GridLayoutId",
                principalTable: "IFPageGridLayout",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_IFPageGrid_IFQuery_QueryId",
                table: "IFPageGrid",
                column: "QueryId",
                principalTable: "IFQuery",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_IFPageGridHeaderAction_IFPageGrid_GridId",
                table: "IFPageGridHeaderAction",
                column: "GridId",
                principalTable: "IFPageGrid",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_IFPageGridRowAction_IFPageGrid_GridId",
                table: "IFPageGridRowAction",
                column: "GridId",
                principalTable: "IFPageGrid",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
