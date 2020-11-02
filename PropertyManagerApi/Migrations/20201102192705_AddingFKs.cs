using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PropertyManagerApi.Migrations
{
    public partial class AddingFKs : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Portfolios_AspNetUsers_OwnerId",
                table: "Portfolios");

            migrationBuilder.DropForeignKey(
                name: "FK_Properties_Addresses_AddressId",
                table: "Properties");

            migrationBuilder.DropForeignKey(
                name: "FK_Properties_Portfolios_PortfolioId",
                table: "Properties");

            migrationBuilder.AlterColumn<Guid>(
                name: "PortfolioId",
                table: "Properties",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "AddressId",
                table: "Properties",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "OwnerId",
                table: "Portfolios",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Portfolios_AspNetUsers_OwnerId",
                table: "Portfolios",
                column: "OwnerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Properties_Addresses_AddressId",
                table: "Properties",
                column: "AddressId",
                principalTable: "Addresses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Properties_Portfolios_PortfolioId",
                table: "Properties",
                column: "PortfolioId",
                principalTable: "Portfolios",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Portfolios_AspNetUsers_OwnerId",
                table: "Portfolios");

            migrationBuilder.DropForeignKey(
                name: "FK_Properties_Addresses_AddressId",
                table: "Properties");

            migrationBuilder.DropForeignKey(
                name: "FK_Properties_Portfolios_PortfolioId",
                table: "Properties");

            migrationBuilder.AlterColumn<Guid>(
                name: "PortfolioId",
                table: "Properties",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid));

            migrationBuilder.AlterColumn<Guid>(
                name: "AddressId",
                table: "Properties",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid));

            migrationBuilder.AlterColumn<Guid>(
                name: "OwnerId",
                table: "Portfolios",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid));

            migrationBuilder.AddForeignKey(
                name: "FK_Portfolios_AspNetUsers_OwnerId",
                table: "Portfolios",
                column: "OwnerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Properties_Addresses_AddressId",
                table: "Properties",
                column: "AddressId",
                principalTable: "Addresses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Properties_Portfolios_PortfolioId",
                table: "Properties",
                column: "PortfolioId",
                principalTable: "Portfolios",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
