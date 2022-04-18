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
            User user = dbContext.Users.FirstOrDefault(x => (Request.Cookies["SessionId"] != null) && (x.sessionId == Guid.Parse(Request.Cookies["SessionId"])));
            List<Product> products = new List<Product>();
            List<Product> oos = new List<Product>();

            if (searchstr == null)
            {
                products = dbContext.Products.ToList();
                ViewBag.Products = products;
                searchstr = "";
            }
            else
            {
                products = dbContext.Products.Where(x => 
                    x.Name.Contains(searchstr) ||
                    x.Desc.Contains(searchstr) 
                ).ToList();
                ViewBag.Products = products;
            }

            foreach (Product item in products)
            {
                List<InventoryRecord> invList = dbContext.InventoryRecords.Where(x => x.ProductId == item.Id).ToList();
                if (invList.Count <= 0)
                {
                    oos.Add(item);
                }
            }
            
            //cleans up function to clean database 
            CleanUp();
            if(Request.Cookies["cartcount"] == null)
            {
                Response.Cookies.Append("cartcount", "0");
            }

            if(user != null)
            {
                ShopCart Cart = dbContext.ShopCarts.FirstOrDefault(x => x.UserId == user.Id);
                CheckCartCount(Cart);
            }

            ViewBag.OutOfStockItems = oos;
            ViewBag.Searchstr = searchstr;
            return View();
        }

        public IActionResult ProductDetails(Product product)
        {
            List<Order> orders = dbContext.Orders.Where(x => x.ProductId == product.Id).OrderByDescending(x => x.TimeStamp).ToList();
            List<ProductReview> reviews = new List<ProductReview>();
            List<User> users = new List<User>();
            List<Product> oos = new List<Product>();

            foreach (Order order in orders)
            {
                ProductReview review = dbContext.ProductReviews.FirstOrDefault(x => x.OrderId == order.Id);
                User user = dbContext.Users.FirstOrDefault(x => x.Id == order.UserId);
                if (review != null && user != null)
                {
                    reviews.Add(review);
                    users.Add(user);
                }

            }
            List<InventoryRecord> invList = dbContext.InventoryRecords.Where(x => x.ProductId == product.Id).ToList();
            if (invList.Count <= 0)
            {
                oos.Add(product);
            }
            double AvgRating = AverageRating(reviews);
            ViewBag.Reviews = reviews;
            ViewBag.Product = product;
            ViewBag.Rating = AvgRating;
            ViewBag.Users = users;
            ViewBag.OutOfStockItems = oos;
            return View();
        }



        public IActionResult AddtoCart([FromBody] Product req)
        {
            AddCartCount();

            User user = dbContext.Users.FirstOrDefault(x => (Request.Cookies["SessionId"] != null) && (x.sessionId == Guid.Parse(Request.Cookies["SessionId"])));
            Product product = dbContext.Products.FirstOrDefault(x => x.Id == req.Id);

            CookieOptions opts = new CookieOptions()
            {
                Expires = DateTime.Now.AddMinutes(360),
            };

            if (user == null) //user has not log in yet
            {
                if (Request.Cookies["CartId"] == null) //user has not gone to the cart page before in the current session
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



 /*----------------------------------HELPER FUNCTIONS HERE-----------------------------------*/
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

        private double AverageRating(List<ProductReview> reviews)
        {
            double rating = 0;
            int sum = 0;
            foreach (ProductReview review in reviews)
            {
                sum = sum + review.Rating;
            }
            if(reviews.Count > 0)
            {
                rating = Math.Round(sum*1.0 / reviews.Count, 1);
            }
            else
            {
                return 0;
            }
            return rating;
        }

        private void AddCartCount()
        {
            int cartcount = int.Parse(Request.Cookies["cartcount"]);
            cartcount++;
            Response.Cookies.Append("cartcount", cartcount.ToString());
        }

        private void CheckCartCount(ShopCart Cart)
        {
            List<ShopCartItem> items = dbContext.ShopCartItems.Where(x => x.ShopCartId == Cart.Id).ToList();
            int cartcount = 0;
            foreach (ShopCartItem item in items)
            {
                cartcount = cartcount + item.Quantity;
            }
            Response.Cookies.Append("cartcount", cartcount.ToString());
        }
    }
}
