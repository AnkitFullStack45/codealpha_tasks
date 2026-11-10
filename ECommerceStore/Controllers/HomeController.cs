using ECommerceStore.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace ECommerceStore.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Laptop()
        {
            return View();       
        }
        public IActionResult Privacy()
        {
            return View();
        }
        public IActionResult AboutUs()
        {
            return View();
        }
        public IActionResult ContactUs()
        {
            return View();
        }
        public IActionResult ShippingPolicy()
        {
            return View();
        }
        public IActionResult ReturnPolicy()
        {
            return View();
        }
        public IActionResult Faq()
        {
            return View();
        }
        public IActionResult Terms()
        {
            return View();
        }
        public IActionResult Sitemap()
        {
            return View();
        }
        public IActionResult Deals()
        {
            return View();
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
