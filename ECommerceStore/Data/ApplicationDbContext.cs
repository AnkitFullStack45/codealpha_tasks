using ECommerceStore.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
namespace ECommerceStore.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(
            DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }

        public DbSet<Category> Categories { get; set; }

        public DbSet<Order> orders { get; set; }

        public DbSet<OrderItem> orderItems { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // CATEGORIES
            modelBuilder.Entity<Category>().HasData(

                new Category
                {
                    CategoryId = 1,
                    CategoryName = "Laptops"
                },

                new Category
                {
                    CategoryId = 2,
                    CategoryName = "Mobiles"
                }
            );


            // PRODUCTS

            modelBuilder.Entity<Product>().HasData(

                // LAPTOPS

                new Product
                {
                    Id = 1,
                    Name = "HP Victus Gaming Laptop",
                    Brand = "HP",
                    Price = 65000,
                    Description = "Intel Core i7 | 16GB RAM | 512GB SSD",
                    ImageUrl = "/Images/victus.jpg",
                    Processor = "Intel Core i7",
                    RAM = "16GB",
                    Storage = "512GB SSD",
                    CategoryId = 1
                },

                new Product
                {
                    Id = 2,
                    Name = "HP OmniBook OLED",
                    Brand = "HP",
                    Price = 150000,
                    Description = "Intel Core Ultra 9 285H, 16GB DDR5, 1TB SSD",
                    ImageUrl = "/Images/omnibook.jpg",
                    Processor = "Intel Core Ultra 9 285H",
                    RAM = "16GB DDR5",
                    Storage = "1TB SSD",
                    CategoryId = 1
                },

                new Product
                {
                    Id = 3,
                    Name = "HP 15 (i5 14th Gen)",
                    Brand = "HP",
                    Price = 75000,
                    Description = "Intel Core 5, 16GB RAM, 512GB SSD",
                    ImageUrl = "/Images/pavvilon.jpg",
                    Processor = "Intel Core 5",
                    RAM = "16GB",
                    Storage = "512GB SSD",
                    CategoryId = 1
                },

                new Product
                {
                    Id = 4,
                    Name = "HP 15, AMD Ryzen 3",
                    Brand = "HP",
                    Price = 50000,
                    Description = "AMD Ryzen 3 7320U, 8GB RAM, 512GB SSD",
                    ImageUrl = "/Images/ryzen3.jpg",
                    Processor = "AMD Ryzen 3 7320U",
                    RAM = "8GB LPDDR5",
                    Storage = "512GB SSD",
                    CategoryId = 1
                },

                new Product
                {
                    Id = 5,
                    Name = "Dell 15 (Previously Inspiron) Laptop",
                    Brand = "Dell",
                    Price = 58000,
                    Description = "Intel Core i5, 16GB RAM, 512GB SSD",
                    ImageUrl = "/Images/inspiron.webp",
                    Processor = "Intel Core i5",
                    RAM = "16GB",
                    Storage = "512GB SSD",
                    CategoryId = 1
                },

                new Product
                {
                    Id = 6,
                    Name = "Dell 15 AI Powered Laptop",
                    Brand = "Dell",
                    Price = 76000,
                    Description = "Intel Core i7, 16GB RAM, 512GB SSD",
                    ImageUrl = "/Images/dell.webp",
                    Processor = "Intel Core i7",
                    RAM = "16GB",
                    Storage = "512GB SSD",
                    CategoryId = 1
                },

                new Product
                {
                    Id = 7,
                    Name = "Dell 15 (Previously Inspiron)",
                    Brand = "Dell",
                    Price = 86000,
                    Description = "R5-7520U Processor, 8GB LPDDR5 RAM, 512GB",
                    ImageUrl = "/Images/dell2.jpg",
                    Processor = "R5-7520U",
                    RAM = "8GB LPDDR5",
                    Storage = "512GB",
                    CategoryId = 1
                },

                new Product
                {
                    Id = 8,
                    Name = "Dell 15 Laptop DC15250-15.6",
                    Brand = "Dell",
                    Price = 85890,
                    Description = "13th Gen Intel Core i7-1355U, 16GB DDR4 RAM, 512GB SSD",
                    ImageUrl = "/Images/dell3.webp",
                    Processor = "13th Gen Intel Core i7-1355U",
                    RAM = "16GB DDR4",
                    Storage = "512GB SSD",
                    CategoryId = 1
                },

                new Product
                {
                    Id = 9,
                    Name = "Lenovo V15 G4 AMD Athlon",
                    Brand = "Lenovo",
                    Price = 52000,
                    Description = "Ryzen 5, 16GB RAM, 512GB SSD",
                    ImageUrl = "/Images/lenovo1.webp",
                    Processor = "Ryzen 5",
                    RAM = "16GB",
                    Storage = "512GB SSD",
                    CategoryId = 1
                },

                new Product
                {
                    Id = 10,
                    Name = "Lenovo IdeaPad Slim 3",
                    Brand = "Lenovo",
                    Price = 53590,
                    Description = "13th Gen Intel Core i3-1315U",
                    ImageUrl = "/Images/lenovo2.webp",
                    Processor = "13th Gen Intel Core i3-1315U",
                    RAM = "",
                    Storage = "",
                    CategoryId = 1
                },

                new Product
                {
                    Id = 11,
                    Name = "ASUS VivoBook 15",
                    Brand = "ASUS",
                    Price = 49000,
                    Description = "Intel Core i5, 8GB RAM, 512GB SSD",
                    ImageUrl = "/Images/asus.webp",
                    Processor = "Intel Core i5",
                    RAM = "8GB",
                    Storage = "512GB SSD",
                    CategoryId = 1
                },

                new Product
                {
                    Id = 12,
                    Name = "ASUS TUF A15",
                    Brand = "ASUS",
                    Price = 73490,
                    Description = "AMD Ryzen 7, RTX 3050 4GB, 16GB RAM",
                    ImageUrl = "/Images/asus2.webp",
                    Processor = "AMD Ryzen 7",
                    RAM = "16GB",
                    Storage = "",
                    CategoryId = 1
                },


                // =========================
                // MOBILES - 20 PRODUCTS
                // =========================

                new Product
                {
                    Id = 13,
                    Name = "iPhone 15",
                    Brand = "Apple",
                    Price = 69999,
                    Description = "Apple iPhone 15 with powerful performance and excellent camera.",
                    ImageUrl = "iphone15.jpg",
                    Processor = "A16 Bionic",
                    RAM = "6GB",
                    Storage = "128GB",
                    CategoryId = 2
                },

                new Product
                {
                    Id = 14,
                    Name = "iPhone 15 Pro",
                    Brand = "Apple",
                    Price = 119999,
                    Description = "Premium Apple smartphone with Pro performance and titanium design.",
                    ImageUrl = "iphone15pro.jpg",
                    Processor = "A17 Pro",
                    RAM = "8GB",
                    Storage = "256GB",
                    CategoryId = 2
                },

                new Product
                {
                    Id = 15,
                    Name = "Samsung Galaxy S24",
                    Brand = "Samsung",
                    Price = 74999,
                    Description = "Flagship Samsung smartphone with premium display and camera.",
                    ImageUrl = "galaxy-s24.jpg",
                    Processor = "Snapdragon 8 Gen 3",
                    RAM = "8GB",
                    Storage = "256GB",
                    CategoryId = 2
                },

                new Product
                {
                    Id = 16,
                    Name = "Samsung Galaxy S24 Ultra",
                    Brand = "Samsung",
                    Price = 129999,
                    Description = "Premium Samsung flagship with S Pen and advanced camera system.",
                    ImageUrl = "s24-ultra.jpg",
                    Processor = "Snapdragon 8 Gen 3",
                    RAM = "12GB",
                    Storage = "256GB",
                    CategoryId = 2
                },

                new Product
                {
                    Id = 17,
                    Name = "Samsung Galaxy A55",
                    Brand = "Samsung",
                    Price = 39999,
                    Description = "Mid-range Samsung smartphone with AMOLED display.",
                    ImageUrl = "galaxy-a55.jpg",
                    Processor = "Exynos 1480",
                    RAM = "8GB",
                    Storage = "128GB",
                    CategoryId = 2
                },

                new Product
                {
                    Id = 18,
                    Name = "OnePlus 12",
                    Brand = "OnePlus",
                    Price = 64999,
                    Description = "High-performance OnePlus smartphone with fast charging.",
                    ImageUrl = "oneplus12.jpg",
                    Processor = "Snapdragon 8 Gen 3",
                    RAM = "12GB",
                    Storage = "256GB",
                    CategoryId = 2
                },

                new Product
                {
                    Id = 19,
                    Name = "OnePlus 12R",
                    Brand = "OnePlus",
                    Price = 42999,
                    Description = "Powerful smartphone designed for gaming and everyday performance.",
                    ImageUrl = "oneplus12r.jpg",
                    Processor = "Snapdragon 8 Gen 2",
                    RAM = "8GB",
                    Storage = "128GB",
                    CategoryId = 2
                },

                new Product
                {
                    Id = 20,
                    Name = "OnePlus Nord CE 4",
                    Brand = "OnePlus",
                    Price = 24999,
                    Description = "Affordable OnePlus smartphone with large battery.",
                    ImageUrl = "nord-ce4.jpg",
                    Processor = "Snapdragon 7 Gen 3",
                    RAM = "8GB",
                    Storage = "128GB",
                    CategoryId = 2
                },

                new Product
                {
                    Id = 21,
                    Name = "Google Pixel 8",
                    Brand = "Google",
                    Price = 75999,
                    Description = "Google smartphone with excellent computational photography.",
                    ImageUrl = "pixel8.jpg",
                    Processor = "Google Tensor G3",
                    RAM = "8GB",
                    Storage = "128GB",
                    CategoryId = 2
                },

                new Product
                {
                    Id = 22,
                    Name = "Google Pixel 8 Pro",
                    Brand = "Google",
                    Price = 99999,
                    Description = "Premium Google Pixel smartphone with advanced AI features.",
                    ImageUrl = "pixel8pro.jpg",
                    Processor = "Google Tensor G3",
                    RAM = "12GB",
                    Storage = "256GB",
                    CategoryId = 2
                },

                new Product
                {
                    Id = 23,
                    Name = "Nothing Phone 2",
                    Brand = "Nothing",
                    Price = 39999,
                    Description = "Unique smartphone featuring the distinctive Glyph interface.",
                    ImageUrl = "nothing-phone2.jpg",
                    Processor = "Snapdragon 8+ Gen 1",
                    RAM = "8GB",
                    Storage = "128GB",
                    CategoryId = 2
                },

                new Product
                {
                    Id = 24,
                    Name = "Nothing Phone 2a",
                    Brand = "Nothing",
                    Price = 23999,
                    Description = "Stylish mid-range smartphone with clean software experience.",
                    ImageUrl = "nothing-phone2a.jpg",
                    Processor = "MediaTek Dimensity 7200 Pro",
                    RAM = "8GB",
                    Storage = "128GB",
                    CategoryId = 2
                },

                new Product
                {
                    Id = 25,
                    Name = "Xiaomi 14",
                    Brand = "Xiaomi",
                    Price = 69999,
                    Description = "Compact flagship smartphone with Leica camera system.",
                    ImageUrl = "xiaomi14.jpg",
                    Processor = "Snapdragon 8 Gen 3",
                    RAM = "12GB",
                    Storage = "512GB",
                    CategoryId = 2
                },

                new Product
                {
                    Id = 26,
                    Name = "Redmi Note 13 Pro",
                    Brand = "Redmi",
                    Price = 25999,
                    Description = "Feature-rich smartphone with high-resolution camera.",
                    ImageUrl = "redmi-note13pro.jpg",
                    Processor = "Snapdragon 7s Gen 2",
                    RAM = "8GB",
                    Storage = "128GB",
                    CategoryId = 2
                },

                new Product
                {
                    Id = 27,
                    Name = "Realme GT 6",
                    Brand = "Realme",
                    Price = 40999,
                    Description = "Performance-focused smartphone with fast charging.",
                    ImageUrl = "realme-gt6.jpg",
                    Processor = "Snapdragon 8s Gen 3",
                    RAM = "8GB",
                    Storage = "256GB",
                    CategoryId = 2
                },

                new Product
                {
                    Id = 28,
                    Name = "Realme 12 Pro+",
                    Brand = "Realme",
                    Price = 29999,
                    Description = "Premium mid-range smartphone with telephoto camera.",
                    ImageUrl = "realme12proplus.jpg",
                    Processor = "Snapdragon 7s Gen 2",
                    RAM = "8GB",
                    Storage = "256GB",
                    CategoryId = 2
                },

                new Product
                {
                    Id = 29,
                    Name = "Vivo V30",
                    Brand = "Vivo",
                    Price = 33999,
                    Description = "Slim Vivo smartphone with an impressive AMOLED display.",
                    ImageUrl = "vivo-v30.jpg",
                    Processor = "Snapdragon 7 Gen 3",
                    RAM = "8GB",
                    Storage = "128GB",
                    CategoryId = 2
                },

                new Product
                {
                    Id = 30,
                    Name = "OPPO Reno 11 Pro",
                    Brand = "OPPO",
                    Price = 39999,
                    Description = "Stylish smartphone with portrait photography capabilities.",
                    ImageUrl = "oppo-reno11pro.jpg",
                    Processor = "MediaTek Dimensity 8200",
                    RAM = "12GB",
                    Storage = "256GB",
                    CategoryId = 2
                },

                new Product
                {
                    Id = 31,
                    Name = "Motorola Edge 50 Pro",
                    Brand = "Motorola",
                    Price = 31999,
                    Description = "Premium Motorola smartphone with curved display.",
                    ImageUrl = "moto-edge50pro.jpg",
                    Processor = "Snapdragon 7 Gen 3",
                    RAM = "8GB",
                    Storage = "256GB",
                    CategoryId = 2
                },

                new Product
                {
                    Id = 32,
                    Name = "Poco X6 Pro",
                    Brand = "Poco",
                    Price = 26999,
                    Description = "Powerful performance smartphone designed for gaming.",
                    ImageUrl = "poco-x6pro.jpg",
                    Processor = "MediaTek Dimensity 8300 Ultra",
                    RAM = "8GB",
                    Storage = "256GB",
                    CategoryId = 2
                }
            );
        }
    }
    }
