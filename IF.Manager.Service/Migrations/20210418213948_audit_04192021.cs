using Microsoft.EntityFrameworkCore.Migrations;

namespace IF.Manager.Service.Migrations
{
    public partial class audit_04192021 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsAudited",
                table: "IFEntity");

            migrationBuilder.AddColumn<int>(
                name: "AuditType",
                table: "IFEntity",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AuditType",
                table: "IFEntity");

            migrationBuilder.AddColumn<bool>(
                name: "IsAudited",
                table: "IFEntity",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
