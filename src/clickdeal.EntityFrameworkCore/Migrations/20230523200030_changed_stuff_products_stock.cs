using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace clickdeal.Migrations
{
    /// <inheritdoc />
    public partial class changedstuffproductsstock : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ReservedUnits",
                table: "AppProductsStock");

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

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "AppProductsStock",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "ProductSpecs",
                table: "AppProductsStock",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "AppProductsStock");

            migrationBuilder.DropColumn(
                name: "ProductSpecs",
                table: "AppProductsStock");

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

            migrationBuilder.AddColumn<int>(
                name: "ReservedUnits",
                table: "AppProductsStock",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
