using ECommerceStore.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ECommerceStore.Controllers
{
    public class OrderController : Controller
    {
        private readonly ApplicationDbContext _context;

        public OrderController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: /Order
        public IActionResult Index()
        {
            var orders = _context.orders
                .OrderByDescending(x => x.OrderDate)
                .ToList();

            return View(orders);
        }


        // GET: /Order/Details/5
        public IActionResult Details(int id)
        {
            var order = _context.orders
                .Include(x => x.OrderItems)
                .ThenInclude(x => x.Product)
                .FirstOrDefault(x => x.Id == id);

            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }
    }
}
