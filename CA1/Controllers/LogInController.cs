using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using CA1.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;


namespace CA1.Controllers
{
    public class LogInController : Controller
    {
        private DBContext dbContext;

        public LogInController(DBContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public IActionResult Index()
        {
            if (Request.Cookies["SessionId"] != null)
            {
                Guid sessionId = Guid.Parse(Request.Cookies["SessionId"]);
                User usersession = dbContext.Users.FirstOrDefault(x =>
                    x.sessionId == sessionId
                );

                if (usersession == null)
                {
                    // someone has used an invalid Session ID (to fool us?); 
                    // route to Logout controller
                    return RedirectToAction("Login", "LogIn");
                }

                // valid Session ID; route to Home page
                return RedirectToAction("Index", "Search");
            }
            return View("LogIn");
        }

        public IActionResult Login(IFormCollection form)
        {
            string username = form["Username"];
            string password = form["Password"];
            string message = string.Empty;

            HashAlgorithm sha = SHA256.Create();

            string combo = username + password;
            byte[] hash = sha.ComputeHash(Encoding.UTF8.GetBytes(combo));



            User user = dbContext.Users.FirstOrDefault(x =>
                     x.UserName == username &&
                     x.PassHash == hash
            );


            if (user == null && (!string.IsNullOrEmpty(username) || !string.IsNullOrEmpty(password)))
            {
                message = "Incorrect Username/Password";
            }

            if (user != null)
            {
                user.sessionId = Guid.NewGuid();
                dbContext.SaveChanges();

                //Combine temp cart if needed.
                if (Request.Cookies["CartId"] != null || Request.Cookies["Temp"] != null)
                {
                    ShopCart UserCart = dbContext.ShopCarts.FirstOrDefault(x => x.UserId == user.Id);

                    if (Request.Cookies["Temp"] != null)
                    {
                        Guid Tempid = Guid.NewGuid();
                        dbContext.ShopCarts.Add(new ShopCart()
                        {
                            Id = Tempid,
                            UserId = null,
                            ShopCartItems = new List<ShopCartItem>()
                        });
                        dbContext.SaveChanges();
                        stringtocart(Request.Cookies["Temp"], Tempid);
                        CombineCarts(Tempid, UserCart);
                        Response.Cookies.Delete("Temp");
                    }
                    else
                    {
                        Guid cartid = Guid.Parse(Request.Cookies["CartId"]);
                        CombineCarts(cartid, UserCart);
                        Response.Cookies.Delete("CartId");
                    }
                }

                // ask browser to save and send back these cookies next time
                Response.Cookies.Append("SessionId", user.sessionId.ToString());
                Response.Cookies.Append("Username", user.UserName);
                Response.Cookies.Append("Name", user.Firstname + " " + user.Lastname);

                string controllerUrl = form["returnUrl"];
                return RedirectToAction("Index", controllerUrl);
            }
            else
            {
                ViewData["ErrorMessage"] = message;
                return View();
            }
        }

        public IActionResult Logout()
        {
            if (Request.Cookies["SessionId"] != null)
            {
                Guid sessionId = Guid.Parse(Request.Cookies["sessionId"]);

                User usersession = dbContext.Users.FirstOrDefault(x => x.sessionId == sessionId);
                if (usersession != null)
                {
                    usersession.sessionId = null;
                    dbContext.SaveChanges();
                }
            }

            Response.Cookies.Delete("SessionId");
            Response.Cookies.Delete("Username");
            Response.Cookies.Delete("cartcount");
            Response.Cookies.Delete("Name");

            if (TempData.Peek("stocklist") != null)
                TempData.Remove("stocklist");

            if (TempData.Peek("stockcount") != null)
                TempData.Remove("stockcount");


            return RedirectToAction("Index", "LogIn");
        }



        /*---------------------------HELPER FUNCTIONS HERE-----------------------------------*/

        private void stringtocart(string cartstr, Guid CartId)
        {
            string[] strarr = cartstr.Split(',');
            Dictionary<Guid, int> freqdict = new Dictionary<Guid, int>();
            ShopCart Cart = dbContext.ShopCarts.FirstOrDefault(x => x.Id == CartId);
            foreach (string str in strarr)
            {
                Guid productid = Guid.Parse(str);
                if (freqdict.ContainsKey(productid))
                {
                    freqdict[productid]++;
                }
                else
                {
                    freqdict.Add(productid, 1);
                }
            }

            foreach (KeyValuePair<Guid, int> item in freqdict)
            {
                Product product = dbContext.Products.FirstOrDefault(x => x.Id == item.Key);
                ShopCartItem cartitem = new ShopCartItem(product)
                {
                    Quantity = item.Value,
                    ShopCartId = Cart.Id
                };
                dbContext.ShopCartItems.Add(cartitem);
            }
            dbContext.SaveChanges();
        }

        private void CombineCarts(Guid tempcartid, ShopCart Cart)
        {
            ShopCart temp = dbContext.ShopCarts.FirstOrDefault(x => x.Id == tempcartid);
            List<ShopCartItem> items = dbContext.ShopCartItems.Where(x => x.ShopCartId == tempcartid).ToList();

            foreach (ShopCartItem item in items)
            {
                ShopCartItem cartitem = dbContext.ShopCartItems.FirstOrDefault(x => x.ShopCartId == Cart.Id && x.ProductId == item.ProductId);
                if (cartitem == null)
                {
                    item.ShopCartId = Cart.Id;
                    dbContext.ShopCartItems.Update(item);
                }
                else
                {
                    cartitem.Quantity = cartitem.Quantity + item.Quantity;
                    dbContext.ShopCartItems.Update(cartitem);
                    dbContext.ShopCartItems.Remove(item);
                }

            }

            dbContext.ShopCarts.Remove(temp);
            dbContext.SaveChanges();
        }


    }


}