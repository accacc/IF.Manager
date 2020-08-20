using Microsoft.EntityFrameworkCore.Migrations;

namespace IF.Manager.Persistence.EF.Migrations
{
    public partial class _612 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_IFPageControlMap_PageControls_PageControlId",
                table: "IFPageControlMap");

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

            migrationBuilder.DropPrimaryKey(
                name: "PK_PageControls",
                table: "PageControls");

            migrationBuilder.RenameTable(
                name: "PageControls",
                newName: "IFPageControl");

            migrationBuilder.RenameIndex(
                name: "IX_PageControls_QueryId",
                table: "IFPageControl",
                newName: "IX_IFPageControl_QueryId");

            migrationBuilder.RenameIndex(
                name: "IX_PageControls_GridLayoutId",
                table: "IFPageControl",
                newName: "IX_IFPageControl_GridLayoutId");

            migrationBuilder.RenameIndex(
                name: "IX_PageControls_ProcessId",
                table: "IFPageControl",
                newName: "IX_IFPageControl_ProcessId");

            migrationBuilder.RenameIndex(
                name: "IX_PageControls_PageLayoutId",
                table: "IFPageControl",
                newName: "IX_IFPageControl_PageLayoutId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_IFPageControl",
                table: "IFPageControl",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_IFPageControl_IFPageLayout_PageLayoutId",
                table: "IFPageControl",
                column: "PageLayoutId",
                principalTable: "IFPageLayout",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_IFPageControl_IFProcess_ProcessId",
                table: "IFPageControl",
                column: "ProcessId",
                principalTable: "IFProcess",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_IFPageControl_IFPageGridLayout_GridLayoutId",
                table: "IFPageControl",
                column: "GridLayoutId",
                principalTable: "IFPageGridLayout",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_IFPageControl_IFQuery_QueryId",
                table: "IFPageControl",
                column: "QueryId",
                principalTable: "IFQuery",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_IFPageControlMap_IFPageControl_PageControlId",
                table: "IFPageControlMap",
                column: "PageControlId",
                principalTable: "IFPageControl",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_IFPageGridHeaderAction_IFPageControl_GridId",
                table: "IFPageGridHeaderAction",
                column: "GridId",
                principalTable: "IFPageControl",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_IFPageGridRowAction_IFPageControl_GridId",
                table: "IFPageGridRowAction",
                column: "GridId",
                principalTable: "IFPageControl",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_IFPageControl_IFPageLayout_PageLayoutId",
                table: "IFPageControl");

            migrationBuilder.DropForeignKey(
                name: "FK_IFPageControl_IFProcess_ProcessId",
                table: "IFPageControl");

            migrationBuilder.DropForeignKey(
                name: "FK_IFPageControl_IFPageGridLayout_GridLayoutId",
                table: "IFPageControl");

            migrationBuilder.DropForeignKey(
                name: "FK_IFPageControl_IFQuery_QueryId",
                table: "IFPageControl");

            migrationBuilder.DropForeignKey(
                name: "FK_IFPageControlMap_IFPageControl_PageControlId",
                table: "IFPageControlMap");

            migrationBuilder.DropForeignKey(
                name: "FK_IFPageGridHeaderAction_IFPageControl_GridId",
                table: "IFPageGridHeaderAction");

            migrationBuilder.DropForeignKey(
                name: "FK_IFPageGridRowAction_IFPageControl_GridId",
                table: "IFPageGridRowAction");

            migrationBuilder.DropPrimaryKey(
                name: "PK_IFPageControl",
                table: "IFPageControl");

            migrationBuilder.RenameTable(
                name: "IFPageControl",
                newName: "PageControls");

            migrationBuilder.RenameIndex(
                name: "IX_IFPageControl_QueryId",
                table: "PageControls",
                newName: "IX_PageControls_QueryId");

            migrationBuilder.RenameIndex(
                name: "IX_IFPageControl_GridLayoutId",
                table: "PageControls",
                newName: "IX_PageControls_GridLayoutId");

            migrationBuilder.RenameIndex(
                name: "IX_IFPageControl_ProcessId",
                table: "PageControls",
                newName: "IX_PageControls_ProcessId");

            migrationBuilder.RenameIndex(
                name: "IX_IFPageControl_PageLayoutId",
                table: "PageControls",
                newName: "IX_PageControls_PageLayoutId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PageControls",
                table: "PageControls",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_IFPageControlMap_PageControls_PageControlId",
                table: "IFPageControlMap",
                column: "PageControlId",
                principalTable: "PageControls",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

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
    }
}
