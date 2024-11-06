using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EcommerceApi.Migrations
{
    public partial class addUseridColumnInOrders : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Userid",
                table: "orders",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_orders_Userid",
                table: "orders",
                column: "Userid");

            migrationBuilder.AddForeignKey(
                name: "FK_orders_users_Userid",
                table: "orders",
                column: "Userid",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_orders_users_Userid",
                table: "orders");

            migrationBuilder.DropIndex(
                name: "IX_orders_Userid",
                table: "orders");

            migrationBuilder.DropColumn(
                name: "Userid",
                table: "orders");
        }
    }
}
