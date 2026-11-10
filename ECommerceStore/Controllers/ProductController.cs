using ECommerceStore.Data;
using ECommerceStore.Models;
using ECommerceStore.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
namespace ECommerceStore.Controllers
{
    public class ProductController : Controller
    {
        private readonly ApplicationDbContext context;
        private readonly IWebHostEnvironment env;
        public ProductController(ApplicationDbContext context, IWebHostEnvironment env)
        {
            this.context = context;
            this.env = env;
        }
        public IActionResult Laptop()
        {
            var products = context.Products
                .Where(p => p.CategoryId == 1)
                .ToList();

            return View(products);
        }

        public IActionResult Details(int id)
        {
            var product = context.Products
                .FirstOrDefault(p => p.Id == id);

            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }
        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.Categories = new SelectList(
                context.Categories.ToList(),
                "CategoryId",
                "CategoryName"
            );

            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(productViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Categories = new SelectList(await context.Categories.ToListAsync(), "Id", "Name", model.CategoryId);
                return View(model);
            }
            string imageName = null;

            if (model.ImageFile != null)
            {
                string extension = Path.GetExtension(model.ImageFile.FileName).ToLower();

                string[] allowedExtensions = { ".jpg", ".jpeg", ".png", ".webp" };

                if (!allowedExtensions.Contains(extension))
                {
                    ModelState.AddModelError("ImageFile", "Only JPG, JPEG, PNG and WEBP images are allowed.");
                    ViewBag.Categories = new SelectList(await context.Categories.ToListAsync(), "Id", "Name", model.CategoryId);
                    return View(model);
                }

                imageName = Guid.NewGuid().ToString() + extension;

                string folderPath = Path.Combine(env.WebRootPath, "Images");

                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }

                string filePath = Path.Combine(folderPath, imageName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await model.ImageFile.CopyToAsync(stream);
                }
            }

            var product = new Product
            {
                Name = model.Name,
                Brand = model.Brand,
                CategoryId = model.CategoryId,
                Price = model.Price,
                Description = model.Description,
                Processor = model.Processor,
                RAM = model.RAM,
                Storage = model.Storage,
                ImageUrl = imageName
            };

            context.Products.Add(product);
            await context.SaveChangesAsync();

            TempData["Success"] = "Product added successfully.";

            return RedirectToAction(nameof(Index));
        }
        // GET: Product/Index
        [HttpGet]
            public async Task<IActionResult> Index()
            {
                var products = await context.Products
                    .Include(p => p.Category)
                    .ToListAsync();

                return View(products);
            }
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var product = await context.Products.FindAsync(id);

            if (product == null)
            {
                return NotFound();
            }

            var model = new productViewModel
            {
                Name = product.Name,
                Brand = product.Brand,
                CategoryId = product.CategoryId,
                Price = product.Price,
                Processor = product.Processor,
                RAM = product.RAM,
                Storage = product.Storage,
                Description = product.Description
            };

            ViewBag.Categories = new SelectList(
                await context.Categories.ToListAsync(),
                "Id",
                "Name",
                product.CategoryId
            );

            ViewBag.ImageUrl = product.ImageUrl;

            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, productViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Categories = new SelectList(
                    await context.Categories.ToListAsync(),
                    "Id",
                    "Name",
                    model.CategoryId
                );

                return View(model);
            }

            var product = await context.Products.FindAsync(id);

            if (product == null)
            {
                return NotFound();
            }

            product.Name = model.Name;
            product.Brand = model.Brand;
            product.CategoryId = model.CategoryId;
            product.Price = model.Price;
            product.Processor = model.Processor;
            product.RAM = model.RAM;
            product.Storage = model.Storage;
            product.Description = model.Description;

            if (model.ImageFile != null)
            {
                string extension = Path.GetExtension(model.ImageFile.FileName).ToLower();

                string[] allowedExtensions = { ".jpg", ".jpeg", ".png", ".webp" };

                if (!allowedExtensions.Contains(extension))
                {
                    ModelState.AddModelError(
                        "ImageFile",
                        "Only JPG, JPEG, PNG and WEBP images are allowed."
                    );

                    ViewBag.Categories = new SelectList(
                        await context.Categories.ToListAsync(),
                        "Id",
                        "Name",
                        model.CategoryId
                    );

                    return View(model);
                }

                string imageName = Guid.NewGuid().ToString() + extension;

                string folderPath = Path.Combine(
                    env.WebRootPath,
                    "Images"
                );

                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }

                string filePath = Path.Combine(folderPath, imageName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await model.ImageFile.CopyToAsync(stream);
                }

                product.ImageUrl = imageName;
            }

            await context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Product updated successfully.";

            return RedirectToAction(nameof(Index));
        }
    }
    }

