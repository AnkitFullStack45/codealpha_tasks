using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ECommerceStore.Migrations
{
    /// <inheritdoc />
    public partial class AppDbContext : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "CategoryId", "CategoryName" },
                values: new object[] { 1, "Laptops" });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "Brand", "CategoryId", "Description", "Graphics", "ImageUrl", "Name", "Price", "Processor", "RAM", "Storage" },
                values: new object[,]
                {
                    { 1, "HP", 1, "Intel Core i7 | 16GB RAM | 512GB SSD", "", "/Images/victus.jpg", "HP Victus Gaming Laptop", 65000m, "Intel Core i7", "16GB", "512GB SSD" },
                    { 2, "HP", 1, "Intel Core Ultra 9 285H, 16GB DDR5, 1TB SSD", "", "/Images/omnibook.jpg", "HP OmniBook OLED", 150000m, "Intel Core Ultra 9 285H", "16GB DDR5", "1TB SSD" },
                    { 3, "HP", 1, "Intel Core 5, 16GB RAM, 512GB SSD", "", "/Images/pavvilon.jpg", "HP 15 (i5 14th Gen)", 75000m, "Intel Core 5", "16GB", "512GB SSD" },
                    { 4, "HP", 1, "AMD Ryzen 3 7320U, 8GB RAM, 512GB SSD", "", "/Images/ryzen3.jpg", "HP 15, AMD Ryzen 3", 50000m, "AMD Ryzen 3 7320U", "8GB LPDDR5", "512GB SSD" },
                    { 5, "Dell", 1, "Intel Core i5, 16GB RAM, 512GB SSD", "", "/Images/inspiron.webp", "Dell 15 (Previously Inspiron) Laptop", 58000m, "Intel Core i5", "16GB", "512GB SSD" },
                    { 6, "Dell", 1, "Intel Core i7, 16GB RAM, 512GB SSD", "", "/Images/dell.webp", "Dell 15 AI Powered Laptop", 76000m, "Intel Core i7", "16GB", "512GB SSD" },
                    { 7, "Dell", 1, "R5-7520U Processor, 8GB LPDDR5 RAM, 512GB", "", "/Images/dell2.jpg", "Dell 15 (Previously Inspiron)", 86000m, "R5-7520U", "8GB LPDDR5", "512GB" },
                    { 8, "Dell", 1, "13th Gen Intel Core i7-1355U, 16GB DDR4 RAM, 512GB SSD", "", "/Images/dell3.webp", "Dell 15 Laptop DC15250-15.6", 85890m, "13th Gen Intel Core i7-1355U", "16GB DDR4", "512GB SSD" },
                    { 9, "Lenovo", 1, "Ryzen 5, 16GB RAM, 512GB SSD", "", "/Images/lenovo1.webp", "Lenovo V15 G4 AMD Athlon", 52000m, "Ryzen 5", "16GB", "512GB SSD" },
                    { 10, "Lenovo", 1, "13th Gen Intel Core i3-1315U", "", "/Images/lenovo2.webp", "Lenovo IdeaPad Slim 3", 53590m, "13th Gen Intel Core i3-1315U", "", "" },
                    { 11, "ASUS", 1, "Intel Core i5, 8GB RAM, 512GB SSD", "", "/Images/asus.webp", "ASUS VivoBook 15", 49000m, "Intel Core i5", "8GB", "512GB SSD" },
                    { 12, "ASUS", 1, "AMD Ryzen 7, RTX 3050 4GB, 16GB RAM", "RTX 3050 4GB", "/Images/asus2.webp", "ASUS TUF A15", 73490m, "AMD Ryzen 7", "16GB", "" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: 1);
        }
    }
}
