using Microsoft.AspNetCore.Mvc;

namespace CA1.Controllers
{
    public class WishlistController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
