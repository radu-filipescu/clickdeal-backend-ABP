using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace clickdeal.Migrations
{
    /// <inheritdoc />
    public partial class addedordershistoryandpendingagain : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserOrderEntry_AppOrdersHistory_UserOrderId",
                table: "UserOrderEntry");

            migrationBuilder.DropColumn(
                name: "CreationTime",
                table: "AppOrdersHistory");

            migrationBuilder.DropColumn(
                name: "CreatorId",
                table: "AppOrdersHistory");

            migrationBuilder.DropColumn(
                name: "LastModificationTime",
                table: "AppOrdersHistory");

            migrationBuilder.DropColumn(
                name: "LastModifierId",
                table: "AppOrdersHistory");

            migrationBuilder.RenameColumn(
                name: "UserOrderId",
                table: "UserOrderEntry",
                newName: "PendingOrderId");

            migrationBuilder.RenameIndex(
                name: "IX_UserOrderEntry_UserOrderId",
                table: "UserOrderEntry",
                newName: "IX_UserOrderEntry_PendingOrderId");

            migrationBuilder.AddColumn<Guid>(
                name: "CustomerOrderId",
                table: "UserOrderEntry",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci");

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

            migrationBuilder.CreateTable(
                name: "AppPendingOrders",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    UserId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Status = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DeliveryCost = table.Column<double>(type: "double", nullable: false),
                    TotalCost = table.Column<double>(type: "double", nullable: false),
                    PaymentMethod = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Adress = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    BillingAdress = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CustomerName = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CustomerEmail = table.Column<string>(type: "varchar(128)", maxLength: 128, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    TrackingNumber = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ShippingMethod = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CustomerNotes = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    PromoCode = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ExtraProperties = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ConcurrencyStamp = table.Column<string>(type: "varchar(40)", maxLength: 40, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreationTime = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    CreatorId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    LastModificationTime = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    LastModifierId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppPendingOrders", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_UserOrderEntry_CustomerOrderId",
                table: "UserOrderEntry",
                column: "CustomerOrderId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserOrderEntry_AppOrdersHistory_CustomerOrderId",
                table: "UserOrderEntry",
                column: "CustomerOrderId",
                principalTable: "AppOrdersHistory",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserOrderEntry_AppPendingOrders_PendingOrderId",
                table: "UserOrderEntry",
                column: "PendingOrderId",
                principalTable: "AppPendingOrders",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserOrderEntry_AppOrdersHistory_CustomerOrderId",
                table: "UserOrderEntry");

            migrationBuilder.DropForeignKey(
                name: "FK_UserOrderEntry_AppPendingOrders_PendingOrderId",
                table: "UserOrderEntry");

            migrationBuilder.DropTable(
                name: "AppPendingOrders");

            migrationBuilder.DropIndex(
                name: "IX_UserOrderEntry_CustomerOrderId",
                table: "UserOrderEntry");

            migrationBuilder.DropColumn(
                name: "CustomerOrderId",
                table: "UserOrderEntry");

            migrationBuilder.RenameColumn(
                name: "PendingOrderId",
                table: "UserOrderEntry",
                newName: "UserOrderId");

            migrationBuilder.RenameIndex(
                name: "IX_UserOrderEntry_PendingOrderId",
                table: "UserOrderEntry",
                newName: "IX_UserOrderEntry_UserOrderId");

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

            migrationBuilder.AddColumn<DateTime>(
                name: "CreationTime",
                table: "AppOrdersHistory",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<Guid>(
                name: "CreatorId",
                table: "AppOrdersHistory",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci");

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModificationTime",
                table: "AppOrdersHistory",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "LastModifierId",
                table: "AppOrdersHistory",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci");

            migrationBuilder.AddForeignKey(
                name: "FK_UserOrderEntry_AppOrdersHistory_UserOrderId",
                table: "UserOrderEntry",
                column: "UserOrderId",
                principalTable: "AppOrdersHistory",
                principalColumn: "Id");
        }
    }
}
