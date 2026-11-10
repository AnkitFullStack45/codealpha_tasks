using Microsoft.AspNetCore.Mvc;

namespace ECommerceStore.Controllers
{
    public class AboutController : Controller
    {
        public IActionResult AboutMe()
        {
            return View();
        }
    }
}
