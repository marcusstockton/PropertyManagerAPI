using Microsoft.EntityFrameworkCore.Migrations;

namespace PropertyManagerApi.Migrations
{
    public partial class TenantUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Profile_Url",
                table: "Tenants",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Profile_Url",
                table: "Tenants");
        }
    }
}
