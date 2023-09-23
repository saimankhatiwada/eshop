using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class StrongTypedId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_reviews_products_product_id",
                table: "reviews");

            migrationBuilder.DropForeignKey(
                name: "fk_reviews_user_user_id",
                table: "reviews");

            migrationBuilder.AddForeignKey(
                name: "fk_reviews_products_product_id1",
                table: "reviews",
                column: "product_id",
                principalTable: "products",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_reviews_user_user_temp_id",
                table: "reviews",
                column: "user_id",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_reviews_products_product_id1",
                table: "reviews");

            migrationBuilder.DropForeignKey(
                name: "fk_reviews_user_user_temp_id",
                table: "reviews");

            migrationBuilder.AddForeignKey(
                name: "fk_reviews_products_product_id",
                table: "reviews",
                column: "product_id",
                principalTable: "products",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_reviews_user_user_id",
                table: "reviews",
                column: "user_id",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
