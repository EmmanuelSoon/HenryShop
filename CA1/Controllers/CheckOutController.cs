using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using CA1.Models;
using Microsoft.AspNetCore.Http;
using System.Linq;
using System.Collections.Generic;
using System;
using System.Diagnostics;
using Newtonsoft.Json;

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
            User user = dbContext.Users.FirstOrDefault(x => (Request.Cookies["SessionId"] != null) && (x.sessionId == Guid.Parse(Request.Cookies["SessionId"])));
            ShopCart Cart = new ShopCart();

            if (user == null)
            {
                if (Request.Cookies["CartId"] == null)
                {
                    //User first time click on cart page after adding to cart without logging in 
                    if (Request.Cookies["Temp"] != null)
                    {
                        Guid cartid = Guid.NewGuid();
                        dbContext.ShopCarts.Add(new ShopCart()
                        {
                            Id = cartid,
                            UserId = null,
                            ShopCartItems = new List<ShopCartItem>()
                        });
                        dbContext.SaveChanges();
                        CookieOptions opts = new CookieOptions()
                        {
                            Expires = DateTime.Now.AddMinutes(360),
                        };
                        Response.Cookies.Append("CartId", cartid.ToString(), opts);

                        string cartstr = Request.Cookies["Temp"];

                        Cart = dbContext.ShopCarts.FirstOrDefault(x => x.Id == cartid);
                        stringtocart(cartstr, Cart.Id);
                        Response.Cookies.Delete("Temp");
                    }
                }
                else //still not log in, moved to search page then back to cart 
                {
                    Guid CartId = Guid.Parse(Request.Cookies["CartId"]);
                    Cart = (ShopCart)dbContext.ShopCarts.FirstOrDefault(x => x.Id == CartId);
                }
            }
            else //logged in already 
            {
                Cart = (ShopCart)dbContext.ShopCarts.FirstOrDefault(x => x.UserId.Equals(user.Id));
            }

            CheckCartCount(Cart);
            List<ShopCartItem> ShopCartItems = (List<ShopCartItem>)Cart.ShopCartItems;
            List<ShopCartItem> insuff_stock = new List<ShopCartItem>();
            List<int> insuff_stock_qty = new List<int>();
            if (TempData.Peek("stocklist") != null)
            {
                insuff_stock = JsonConvert.DeserializeObject<List<ShopCartItem>>((string)TempData.Peek("stocklist"));
                insuff_stock_qty = JsonConvert.DeserializeObject<List<int>>((string)TempData.Peek("stockcount"));

            }

            ViewData["ShopCart"] = ShopCartItems;
            ViewData["stocklist"] = insuff_stock;
            ViewData["stockcount"] = insuff_stock_qty;


            float total = 0;
            for (int i = 0; i < ShopCartItems.Count; i++)
            {
                total += ShopCartItems[i].Product.Price * ShopCartItems[i].Quantity;
            }

            ViewBag.Total = total;
            return View();
        }


        public IActionResult Home()
        {
            return RedirectToAction("Index", "Search");
        }


        public IActionResult RemoveFromCart([FromBody] ShopCartItem req)
        {
            ShopCartItem item = dbContext.ShopCartItems.FirstOrDefault(x => x.Id == req.Id);
            int change = item.Quantity;

            if (TempData.Peek("stocklist") != null)
            {
                List<ShopCartItem> insuff_stock = JsonConvert.DeserializeObject<List<ShopCartItem>>((string)TempData.Peek("stocklist"));
                List<int> insuff_stock_qty = JsonConvert.DeserializeObject<List<int>>((string)TempData.Peek("stockcount"));
                if (item != null)
                {
                    updateLists(item);
                    dbContext.ShopCartItems.Remove(item);
                    dbContext.SaveChanges();
                    if (req.ProductId == Guid.Empty)
                    {
                        return Json(new { status = "success" });
                    }
                    else
                    {
                        return Json(new { status = "removed" });
                    }

                }
            }
            else
            {
                if (item != null)
                {

                    dbContext.ShopCartItems.Remove(item);
                    dbContext.SaveChanges();
                    return Json(new { status = "success" });
                }
            }
            return Json(new { status = "fail" });
        }

        public IActionResult UpdateQuantity([FromBody] ShopCartItem req)
        {

            ShopCartItem item = dbContext.ShopCartItems.FirstOrDefault(x => x.Id.Equals(req.Id));
            if (item != null)
            {

                item.Quantity = req.Quantity;
                dbContext.SaveChanges();
                return Json(new { status = "success" });


            }

            return Json(new { status = "fail" });
        }


        public IActionResult ChangeQ([FromBody] ShopCartItem req)
        {
            ShopCartItem item = dbContext.ShopCartItems.FirstOrDefault(x => x.Id == req.Id);
            if (req.Quantity <= 0)
            {
                return RemoveFromCart(item);
            }
            else
            {
                item.Quantity = req.Quantity;
                dbContext.ShopCartItems.Update(item);
                dbContext.SaveChanges();
                updateLists(item);
                return Json(new { status = "success" });
            }
        }




        //check for user login for the JS
        public IActionResult CheckUser()
        {
            User user = dbContext.Users.FirstOrDefault(x => (Request.Cookies["SessionId"] != null) && (x.sessionId == Guid.Parse(Request.Cookies["SessionId"])));
            if (user == null)
            {
                return Json(new { status = "notlogged" });
            }
            return Json(new { status = "logged" });
        }


        //Checking out from cart function 
        public IActionResult CheckOutCart()
        {
            User user = dbContext.Users.FirstOrDefault(x => (Request.Cookies["SessionId"] != null) && (x.sessionId == Guid.Parse(Request.Cookies["SessionId"])));

            ShopCart ShopCart = (ShopCart)dbContext.ShopCarts.FirstOrDefault(x => x.UserId.Equals(user.Id)); //Get updated Cart 
            List<ShopCartItem> items = (List<ShopCartItem>)ShopCart.ShopCartItems;
            List<ShopCartItem> insuff_stock = new List<ShopCartItem>();
            List<int> insuff_stock_qty = new List<int>();

            for (int i = 0; i < items.Count; i++) //Checking if our inventory has enough for the order 
            {
                ShopCartItem curr = items[i];
                List<InventoryRecord> invlist = dbContext.InventoryRecords.Where(x => x.ProductId == curr.ProductId).ToList();


                if (invlist.Count < curr.Quantity)
                {
                    insuff_stock.Add(curr);
                    insuff_stock_qty.Add(invlist.Count);

                }
            }

            if (insuff_stock.Count != 0) //not enough, let user know, get user to change 
            {
                TempData["stocklist"] = JsonConvert.SerializeObject(insuff_stock, new JsonSerializerSettings()
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                    PreserveReferencesHandling = PreserveReferencesHandling.Objects
                });
                TempData["stockcount"] = JsonConvert.SerializeObject(insuff_stock_qty, new JsonSerializerSettings()
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                    PreserveReferencesHandling = PreserveReferencesHandling.Objects
                });
                return Json(new { status = "fail" });
            }



            else //if have enough, creates new orders and add it into Db
            {
                if (TempData.Peek("stocklist") != null)
                    TempData.Remove("stocklist");

                if (TempData.Peek("stockcount") != null)
                    TempData.Remove("stockcount");

                for (int i = 0; i < items.Count; i++)
                {
                    ShopCartItem curr = items[i];
                    List<OrderDetail> orderlist = new List<OrderDetail>();
                    List<InventoryRecord> invlist = dbContext.InventoryRecords.Where(x => x.ProductId == curr.ProductId).ToList();

                    Order order = new Order
                    {

                        ProductId = curr.ProductId,
                        Quantity = curr.Quantity,
                        UserId = user.Id,

                    };


                    for (int j = 0; j < curr.Quantity; j++)
                    {
                        InventoryRecord inv = invlist[j];
                        OrderDetail orderDetails = new OrderDetail
                        {
                            OrderId = order.Id,
                            ActivationId = inv.ActivationId,
                        };
                        orderlist.Add(orderDetails);
                        dbContext.InventoryRecords.Remove(inv);

                    }
                    order.OrderDetails = orderlist;
                    dbContext.Orders.Add(order);
                    dbContext.ShopCartItems.Remove(curr);
                }
                ResetCartCount();
                dbContext.SaveChanges();

                return Json(new { status = "success" });
            }
        }


/*-----------------------------------HELPER FUNCTIONS HERE-------------------------------------------*/

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

        private void updateLists(ShopCartItem item)
        {
            List<ShopCartItem> insuff_stock = JsonConvert.DeserializeObject<List<ShopCartItem>>((string)TempData["stocklist"]);
            List<int> insuff_stock_qty = JsonConvert.DeserializeObject<List<int>>((string)TempData["stockcount"]);


            for (int i = 0; i < insuff_stock.Count; i++)
            {
                if (item.Id == insuff_stock[i].Id)
                {
                    insuff_stock.RemoveAt(i);
                    insuff_stock_qty.RemoveAt(i);
                }
            }

            if (insuff_stock.Count != 0) //not enough, let user know, get user to change 
            {
                TempData["stocklist"] = JsonConvert.SerializeObject(insuff_stock, new JsonSerializerSettings()
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                    PreserveReferencesHandling = PreserveReferencesHandling.Objects
                });
                TempData["stockcount"] = JsonConvert.SerializeObject(insuff_stock_qty, new JsonSerializerSettings()
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                    PreserveReferencesHandling = PreserveReferencesHandling.Objects
                });
            }
            else
            {
                if (TempData.Peek("stocklist") != null)
                    TempData.Remove("stocklist");

                if (TempData.Peek("stockcount") != null)
                    TempData.Remove("stockcount");

            }
        }

        private void ResetCartCount()
        {
            Response.Cookies.Append("cartcount", "0");
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
