using Microsoft.EntityFrameworkCore.Migrations;

namespace IF.Manager.Service.Migrations
{
    public partial class _300620211 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "CommandAudit",
                table: "IFProject",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "CommandErrorHandler",
                table: "IFProject",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "CommandPerformanceCounter",
                table: "IFProject",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "JsonAppType",
                table: "IFProject",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "QueryAudit",
                table: "IFProject",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "QueryErrorHandler",
                table: "IFProject",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "QueryPerformanceCounter",
                table: "IFProject",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "SystemDbConnectionString",
                table: "IFProject",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SystemDbType",
                table: "IFProject",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CommandAudit",
                table: "IFProject");

            migrationBuilder.DropColumn(
                name: "CommandErrorHandler",
                table: "IFProject");

            migrationBuilder.DropColumn(
                name: "CommandPerformanceCounter",
                table: "IFProject");

            migrationBuilder.DropColumn(
                name: "JsonAppType",
                table: "IFProject");

            migrationBuilder.DropColumn(
                name: "QueryAudit",
                table: "IFProject");

            migrationBuilder.DropColumn(
                name: "QueryErrorHandler",
                table: "IFProject");

            migrationBuilder.DropColumn(
                name: "QueryPerformanceCounter",
                table: "IFProject");

            migrationBuilder.DropColumn(
                name: "SystemDbConnectionString",
                table: "IFProject");

            migrationBuilder.DropColumn(
                name: "SystemDbType",
                table: "IFProject");
        }
    }
}
