using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace clickdeal.Migrations
{
    /// <inheritdoc />
    public partial class addedSmartBillIdtoreviews : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SmartbillId",
                table: "AppReviews",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SmartbillId",
                table: "AppReviews");
        }
    }
}
