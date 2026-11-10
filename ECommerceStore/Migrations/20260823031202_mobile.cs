using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ECommerceStore.Migrations
{
    /// <inheritdoc />
    public partial class mobile : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "CategoryId", "CategoryName" },
                values: new object[] { 2, "Mobiles" });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "Brand", "CategoryId", "Description", "Graphics", "ImageUrl", "Name", "Price", "Processor", "RAM", "Storage" },
                values: new object[,]
                {
                    { 13, "Apple", 2, "Apple iPhone 15 with powerful performance and excellent camera.", "Apple GPU", "/Images/mobiles/iphone15.jpg", "iPhone 15", 69999m, "A16 Bionic", "6GB", "128GB" },
                    { 14, "Apple", 2, "Premium Apple smartphone with Pro performance and titanium design.", "Apple GPU", "/Images/mobiles/iphone15pro.jpg", "iPhone 15 Pro", 119999m, "A17 Pro", "8GB", "256GB" },
                    { 15, "Samsung", 2, "Flagship Samsung smartphone with premium display and camera.", "Adreno GPU", "/Images/mobiles/galaxy-s24.jpg", "Samsung Galaxy S24", 74999m, "Snapdragon 8 Gen 3", "8GB", "256GB" },
                    { 16, "Samsung", 2, "Premium Samsung flagship with S Pen and advanced camera system.", "Adreno GPU", "/Images/mobiles/s24-ultra.jpg", "Samsung Galaxy S24 Ultra", 129999m, "Snapdragon 8 Gen 3", "12GB", "256GB" },
                    { 17, "Samsung", 2, "Mid-range Samsung smartphone with AMOLED display.", "Xclipse GPU", "/Images/mobiles/galaxy-a55.jpg", "Samsung Galaxy A55", 39999m, "Exynos 1480", "8GB", "128GB" },
                    { 18, "OnePlus", 2, "High-performance OnePlus smartphone with fast charging.", "Adreno GPU", "/Images/mobiles/oneplus12.jpg", "OnePlus 12", 64999m, "Snapdragon 8 Gen 3", "12GB", "256GB" },
                    { 19, "OnePlus", 2, "Powerful smartphone designed for gaming and everyday performance.", "Adreno GPU", "/Images/mobiles/oneplus12r.jpg", "OnePlus 12R", 42999m, "Snapdragon 8 Gen 2", "8GB", "128GB" },
                    { 20, "OnePlus", 2, "Affordable OnePlus smartphone with large battery.", "Adreno GPU", "/Images/mobiles/nord-ce4.jpg", "OnePlus Nord CE 4", 24999m, "Snapdragon 7 Gen 3", "8GB", "128GB" },
                    { 21, "Google", 2, "Google smartphone with excellent computational photography.", "Mali GPU", "/Images/mobiles/pixel8.jpg", "Google Pixel 8", 75999m, "Google Tensor G3", "8GB", "128GB" },
                    { 22, "Google", 2, "Premium Google Pixel smartphone with advanced AI features.", "Mali GPU", "/Images/mobiles/pixel8pro.jpg", "Google Pixel 8 Pro", 99999m, "Google Tensor G3", "12GB", "256GB" },
                    { 23, "Nothing", 2, "Unique smartphone featuring the distinctive Glyph interface.", "Adreno GPU", "/Images/mobiles/nothing-phone2.jpg", "Nothing Phone 2", 39999m, "Snapdragon 8+ Gen 1", "8GB", "128GB" },
                    { 24, "Nothing", 2, "Stylish mid-range smartphone with clean software experience.", "Mali GPU", "/Images/mobiles/nothing-phone2a.jpg", "Nothing Phone 2a", 23999m, "MediaTek Dimensity 7200 Pro", "8GB", "128GB" },
                    { 25, "Xiaomi", 2, "Compact flagship smartphone with Leica camera system.", "Adreno GPU", "/Images/mobiles/xiaomi14.jpg", "Xiaomi 14", 69999m, "Snapdragon 8 Gen 3", "12GB", "512GB" },
                    { 26, "Redmi", 2, "Feature-rich smartphone with high-resolution camera.", "Adreno GPU", "/Images/mobiles/redmi-note13pro.jpg", "Redmi Note 13 Pro", 25999m, "Snapdragon 7s Gen 2", "8GB", "128GB" },
                    { 27, "Realme", 2, "Performance-focused smartphone with fast charging.", "Adreno GPU", "/Images/mobiles/realme-gt6.jpg", "Realme GT 6", 40999m, "Snapdragon 8s Gen 3", "8GB", "256GB" },
                    { 28, "Realme", 2, "Premium mid-range smartphone with telephoto camera.", "Adreno GPU", "/Images/mobiles/realme12proplus.jpg", "Realme 12 Pro+", 29999m, "Snapdragon 7s Gen 2", "8GB", "256GB" },
                    { 29, "Vivo", 2, "Slim Vivo smartphone with an impressive AMOLED display.", "Adreno GPU", "/Images/mobiles/vivo-v30.jpg", "Vivo V30", 33999m, "Snapdragon 7 Gen 3", "8GB", "128GB" },
                    { 30, "OPPO", 2, "Stylish smartphone with portrait photography capabilities.", "Mali GPU", "/Images/mobiles/oppo-reno11pro.jpg", "OPPO Reno 11 Pro", 39999m, "MediaTek Dimensity 8200", "12GB", "256GB" },
                    { 31, "Motorola", 2, "Premium Motorola smartphone with curved display.", "Adreno GPU", "/Images/mobiles/moto-edge50pro.jpg", "Motorola Edge 50 Pro", 31999m, "Snapdragon 7 Gen 3", "8GB", "256GB" },
                    { 32, "Poco", 2, "Powerful performance smartphone designed for gaming.", "Mali GPU", "/Images/mobiles/poco-x6pro.jpg", "Poco X6 Pro", 26999m, "MediaTek Dimensity 8300 Ultra", "8GB", "256GB" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 18);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 19);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 20);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 21);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 22);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 23);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 24);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 25);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 26);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 27);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 28);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 29);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 30);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 31);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 32);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: 2);
        }
    }
}
