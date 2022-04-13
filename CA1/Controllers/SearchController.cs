using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using CA1.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Web;
using System.Net;
using Microsoft.AspNetCore.Http;

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
            if(searchstr == null)
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

            CleanUp();

            ViewBag.Searchstr = searchstr;
            return View();
        }


        public IActionResult AddtoCart([FromBody] Product req)
        {

            User user = dbContext.Users.FirstOrDefault(x => (Request.Cookies["SessionId"] != null) && (x.sessionId == Guid.Parse(Request.Cookies["SessionId"])));
            Product product = dbContext.Products.FirstOrDefault(x => x.Id == req.Id);

            CookieOptions opts = new CookieOptions()
            {
                Expires = DateTime.Now.AddMinutes(360),
            };

            if (user == null) //user has not log in yet
            {
                if(Request.Cookies["CartId"] == null) //user has not gone to the cart page before in the current session
                {
                    if (Request.Cookies["Temp"] == null) //user has not added to cart yet 
                    {
                        string str = product.Id.ToString();
                        Response.Cookies.Append("Temp", str, opts);
                    }
                    else
                    {
                        string temp = Request.Cookies["Temp"];
                        temp = temp +","+product.Id.ToString();
                        Response.Cookies.Delete("Temp");
                        Response.Cookies.Append("Temp", temp, opts);
                    }

                    return Json(new
                    {
                        status = "success",
                        name = product.Name,
                    });

                }
                else //user gone to cart page before in session 
                {
                    Guid CartId = Guid.Parse(Request.Cookies["CartId"]);
                    ShopCart cart = dbContext.ShopCarts.FirstOrDefault(x => x.Id == CartId);
                    return AddHelper(cart, product);
                }

            }
            else //user log in already 
            {
                ShopCart cart = dbContext.ShopCarts.FirstOrDefault(x => x.UserId == user.Id);
                return AddHelper(cart, product);
            }

        }



 /*---------------------------HELPER FUNCTIONS HERE-----------------------------------*/
        private IActionResult AddHelper(ShopCart cart, Product product)
        {
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

        private bool CleanUp()
        {
            List<ShopCart> carts = dbContext.ShopCarts.Where(x => x.UserId == null).ToList();

            foreach (ShopCart cart in carts)
            {
                long curr = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds();
                long duration = curr - cart.Created;
                if (duration > 21600) //6 hours
                {
                   foreach (ShopCartItem shopcartitem in cart.ShopCartItems)
                    {
                        dbContext.ShopCartItems.Remove(shopcartitem);
                    }
                   dbContext.ShopCarts.Remove(cart);
                }
            }
            dbContext.SaveChanges();

            return true;
        }
    }
}
