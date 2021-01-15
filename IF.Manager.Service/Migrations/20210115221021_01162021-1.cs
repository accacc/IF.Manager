using Microsoft.EntityFrameworkCore.Migrations;

namespace IF.Manager.Persistence.EF.Migrations
{
    public partial class _011620211 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "IFCommandMulti");

            migrationBuilder.AddColumn<int>(
                name: "IFMapperId",
                table: "IFCommand",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ParentId",
                table: "IFCommand",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_IFCommand_IFMapperId",
                table: "IFCommand",
                column: "IFMapperId");

            migrationBuilder.CreateIndex(
                name: "IX_IFCommand_ParentId",
                table: "IFCommand",
                column: "ParentId");

            migrationBuilder.AddForeignKey(
                name: "FK_IFCommand_IFCommand_IFMapperId",
                table: "IFCommand",
                column: "IFMapperId",
                principalTable: "IFCommand",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_IFCommand_IFCommand_ParentId",
                table: "IFCommand",
                column: "ParentId",
                principalTable: "IFCommand",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_IFCommand_IFCommand_IFMapperId",
                table: "IFCommand");

            migrationBuilder.DropForeignKey(
                name: "FK_IFCommand_IFCommand_ParentId",
                table: "IFCommand");

            migrationBuilder.DropIndex(
                name: "IX_IFCommand_IFMapperId",
                table: "IFCommand");

            migrationBuilder.DropIndex(
                name: "IX_IFCommand_ParentId",
                table: "IFCommand");

            migrationBuilder.DropColumn(
                name: "IFMapperId",
                table: "IFCommand");

            migrationBuilder.DropColumn(
                name: "ParentId",
                table: "IFCommand");

            migrationBuilder.CreateTable(
                name: "IFCommandMulti",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IFCommandId = table.Column<int>(type: "int", nullable: false),
                    IFMapperId = table.Column<int>(type: "int", nullable: true),
                    ParentId = table.Column<int>(type: "int", nullable: true),
                    Sequence = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IFCommandMulti", x => x.Id);
                    table.ForeignKey(
                        name: "FK_IFCommandMulti_IFCommand_IFCommandId",
                        column: x => x.IFCommandId,
                        principalTable: "IFCommand",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_IFCommandMulti_IFCommand_IFMapperId",
                        column: x => x.IFMapperId,
                        principalTable: "IFCommand",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_IFCommandMulti_IFCommandMulti_ParentId",
                        column: x => x.ParentId,
                        principalTable: "IFCommandMulti",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_IFCommandMulti_IFCommandId",
                table: "IFCommandMulti",
                column: "IFCommandId");

            migrationBuilder.CreateIndex(
                name: "IX_IFCommandMulti_IFMapperId",
                table: "IFCommandMulti",
                column: "IFMapperId");

            migrationBuilder.CreateIndex(
                name: "IX_IFCommandMulti_ParentId",
                table: "IFCommandMulti",
                column: "ParentId");
        }
    }
}
