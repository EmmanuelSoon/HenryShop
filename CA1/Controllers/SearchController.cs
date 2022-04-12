using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using CA1.Models;
using CA1.Data;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;



namespace CA1.Controllers
{
    public class SearchController : Controller
    {
        private DBContext dbContext;

        public SearchController(DBContext dbContext)
        {
            this.dbContext = dbContext;
        }


        public IActionResult Index(string searchstr)
        {
            if (searchstr == null)
            {
                List<Product> products = dbContext.Products.ToList();
                ViewBag.Products = products;
                searchstr = "";
            }
            else
            {
                List<Product> products = dbContext.Products.Where(x =>
                    x.Name.Contains(searchstr) ||
                    x.Desc.Contains(searchstr)
                ).ToList();
                ViewBag.Products = products;
            }


            ViewBag.Searchstr = searchstr;
            return View();
        }


        //adding to cart, if user not logged in, we send the user to the log in page.
        public IActionResult AddtoCart([FromBody] Product req)
        {

            User user = dbContext.Users.FirstOrDefault(x => (Request.Cookies["SessionId"] != null) && (x.sessionId == Guid.Parse(Request.Cookies["SessionId"])));
            Product product = dbContext.Products.FirstOrDefault(x => x.Id == req.Id);

            if (user == null)
            {
                return RedirectToAction("Index", "LogIn");
            }
            else
            {
                ShopCart cart = dbContext.ShopCarts.FirstOrDefault(x => x.UserId == user.Id);
                ShopCartItem cartitem = dbContext.ShopCartItems.FirstOrDefault(x => x.ShopCartId == cart.Id && x.Product.Id == product.Id);

                if (cartitem != null)
                {
                    cartitem.Quantity++;
                    dbContext.Update(cartitem);
                }
                else
                {
                    cartitem = new ShopCartItem(product)
                    {
                        Quantity = 1,
                        ShopCartId = cart.Id
                    };
                    dbContext.ShopCartItems.Add(cartitem);
                }

                dbContext.SaveChanges();

                return Json(new
                {
                    status = "success",
                    name = product.Name,
                });

            }

        }
    }

}
