using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace clickdeal.Migrations
{
    /// <inheritdoc />
    public partial class addedisVisible : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Information",
                table: "AppProducts");

            migrationBuilder.DropColumn(
                name: "PhotoPath",
                table: "AppProducts");

            migrationBuilder.RenameColumn(
                name: "PhotoPathSmall",
                table: "AppProducts",
                newName: "PhotoPaths");

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

            migrationBuilder.AddColumn<int>(
                name: "Quantity",
                table: "AppProducts",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "VisibleOnWebsite",
                table: "AppProducts",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Quantity",
                table: "AppProducts");

            migrationBuilder.DropColumn(
                name: "VisibleOnWebsite",
                table: "AppProducts");

            migrationBuilder.RenameColumn(
                name: "PhotoPaths",
                table: "AppProducts",
                newName: "PhotoPathSmall");

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

            migrationBuilder.AddColumn<string>(
                name: "Information",
                table: "AppProducts",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "PhotoPath",
                table: "AppProducts",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");
        }
    }
}
