using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using CA1.Models;
using Microsoft.AspNetCore.Http;
using System.Linq;

namespace CA1.Controllers
{
    public class CheckOutController : Controller
    {
        private DBContext dbContext;
        public CheckOutController(DBContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public IActionResult Index()
        {
            string UserId = HttpContext.Session.GetString("userId");

            ShopCart ShopCart = (ShopCart)dbContext.ShopCarts.FirstOrDefault(x => x.UserId.Equals(UserId));

            ViewData["ShopCart"] = ShopCart;
            return View();
        }
    }
}
