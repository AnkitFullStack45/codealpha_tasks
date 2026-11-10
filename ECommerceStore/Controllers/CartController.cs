using ECommerceStore.Data;
using ECommerceStore.Extensions;
using ECommerceStore.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceStore.Controllers
{
    public class CartController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CartController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Cart Page
        [Authorize]
        public IActionResult Index()
        {
            var cart = HttpContext.Session.GetObject<List<CartItem>>("Cart")
                       ?? new List<CartItem>();

            return View(cart);
        }


        // Add Product To Cart
        public IActionResult AddToCart(int id)
        {
            var product = _context.Products
                .FirstOrDefault(p => p.Id == id);

            if (product == null)
            {
                return NotFound();
            }

            var cart = HttpContext.Session.GetObject<List<CartItem>>("Cart")
                       ?? new List<CartItem>();

            var existingItem = cart.FirstOrDefault(x => x.ProductId == id);

            if (existingItem != null)
            {
                existingItem.Quantity++;
            }
            else
            {
                cart.Add(new CartItem
                {
                    ProductId = product.Id,
                    ProductName = product.Name,
                    ImageUrl = product.ImageUrl,
                    Price = product.Price,
                    Quantity = 1
                });
            }

            HttpContext.Session.SetObject("Cart", cart);

            return RedirectToAction("Index");
        }


        // Increase Quantity
        public IActionResult Increase(int id)
        {
            var cart = HttpContext.Session.GetObject<List<CartItem>>("Cart")
                       ?? new List<CartItem>();

            var item = cart.FirstOrDefault(x => x.ProductId == id);

            if (item != null)
            {
                item.Quantity++;
            }

            HttpContext.Session.SetObject("Cart", cart);

            return RedirectToAction("Index");
        }


        // Decrease Quantity
        public IActionResult Decrease(int id)
        {
            var cart = HttpContext.Session.GetObject<List<CartItem>>("Cart")
                       ?? new List<CartItem>();

            var item = cart.FirstOrDefault(x => x.ProductId == id);

            if (item != null)
            {
                item.Quantity--;

                // Remove item if quantity becomes 0
                if (item.Quantity <= 0)
                {
                    cart.Remove(item);
                }
            }

            HttpContext.Session.SetObject("Cart", cart);

            return RedirectToAction("Index");
        }


        // Remove Product
        public IActionResult Remove(int id)
        {
            var cart = HttpContext.Session.GetObject<List<CartItem>>("Cart")
                       ?? new List<CartItem>();

            var item = cart.FirstOrDefault(x => x.ProductId == id);

            if (item != null)
            {
                cart.Remove(item);
            }

            HttpContext.Session.SetObject("Cart", cart);

            return RedirectToAction("Index");
        }
    }
}