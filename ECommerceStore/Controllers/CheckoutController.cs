using ECommerceStore.Data;
using ECommerceStore.Extensions;
using ECommerceStore.Models;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceStore.Controllers
{
    public class CheckoutController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CheckoutController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: /Checkout/Index
        public IActionResult Index()
        {
            var cart = HttpContext.Session.GetObject<List<CartItem>>("Cart")
                       ?? new List<CartItem>();
            if (cart.Count == 0)
            {
                return RedirectToAction("Index", "Cart");
            }
            ViewBag.Cart = cart;
            return View();
        }


        // POST: /Checkout/PlaceOrder
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult PlaceOrder(
            string CustomerName,
            string CustomerEmail,
            string ShippingAddress)
        {
            // 1. Get cart
            var cart = HttpContext.Session.GetObject<List<CartItem>>("Cart")
                       ?? new List<CartItem>();

            if (cart.Count == 0)
            {
                TempData["Error"] = "Your cart is empty.";
                return RedirectToAction("Index", "Cart");
            }


            // 2. Validate customer information
            if (string.IsNullOrWhiteSpace(CustomerName))
            {
                TempData["Error"] = "Please enter your name.";
                return RedirectToAction("Index");
            }

            if (string.IsNullOrWhiteSpace(CustomerEmail))
            {
                TempData["Error"] = "Please enter your email.";
                return RedirectToAction("Index");
            }

            if (string.IsNullOrWhiteSpace(ShippingAddress))
            {
                TempData["Error"] = "Please enter your shipping address.";
                return RedirectToAction("Index");
            }


            // 3. Calculate total
            decimal totalAmount = cart.Sum(x => x.TotalPrice);


            // 4. Create Order
            var order = new Order
            {
                CustomerName = CustomerName.Trim(),
                CustomerEmail = CustomerEmail.Trim(),
                ShippingAddress = ShippingAddress.Trim(),
                OrderDate = DateTime.Now,
                TotalAmount = totalAmount,
                Status = "Pending"
            };


            try
            {
                // 5. Save Order
                _context.orders.Add(order);
                _context.SaveChanges();


                // 6. Create OrderItems
                foreach (var item in cart)
                {
                    var orderItem = new OrderItem
                    {
                        OrderId = order.Id,
                        ProductId = item.ProductId,
                        Quantity = item.Quantity,
                        UnitPrice = item.Price,
                        TotalPrice = item.TotalPrice
                    };

                    _context.orderItems.Add(orderItem);
                }


                // 7. Save OrderItems
                _context.SaveChanges();


                // 8. Clear cart
                HttpContext.Session.Remove("Cart");


                // 9. Confirmation page
                return RedirectToAction(
                    "Confirmation",
                    new { id = order.Id }
                );
            }
            catch (Exception ex)
            {
                // Show the actual database error during development
                TempData["Error"] = ex.InnerException?.Message
                                    ?? ex.Message;

                return RedirectToAction("Index");
            }
        }


        // GET: /Checkout/Confirmation/5
        public IActionResult Confirmation(int id)
        {
            var order = _context.orders
                .FirstOrDefault(x => x.Id == id);

            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }
    }
}