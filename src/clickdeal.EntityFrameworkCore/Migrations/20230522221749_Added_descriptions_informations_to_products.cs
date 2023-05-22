using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace clickdeal.Migrations
{
    /// <inheritdoc />
    public partial class Addeddescriptionsinformationstoproducts : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Description",
                table: "AppProducts",
                newName: "Information");

            migrationBuilder.AlterColumn<Guid>(
                name: "ProductId",
                table: "AppProductsStock",
                type: "char(128)",
                maxLength: 128,
                nullable: false,
                collation: "ascii_general_ci",
                oldClrType: typeof(string),
                oldType: "char(128)",
                oldMaxLength: 128)
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "DescriptionLong",
                table: "AppProducts",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "DescriptionShort",
                table: "AppProducts",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DescriptionLong",
                table: "AppProducts");

            migrationBuilder.DropColumn(
                name: "DescriptionShort",
                table: "AppProducts");

            migrationBuilder.RenameColumn(
                name: "Information",
                table: "AppProducts",
                newName: "Description");

            migrationBuilder.AlterColumn<string>(
                name: "ProductId",
                table: "AppProductsStock",
                type: "char(128)",
                maxLength: 128,
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "char(128)",
                oldMaxLength: 128)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("Relational:Collation", "ascii_general_ci");
        }
    }
}
