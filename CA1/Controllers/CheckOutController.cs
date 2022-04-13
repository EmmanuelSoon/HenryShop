using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using CA1.Models;
using Microsoft.AspNetCore.Http;
using System.Linq;
using System.Collections.Generic;
using System;
using System.Diagnostics;

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
                if(Request.Cookies["CartId"] == null)
                {
                    if(Request.Cookies["Temp"] != null)
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
                else
                {
                    Guid CartId = Guid.Parse(Request.Cookies["CartId"]);
                    Cart = (ShopCart)dbContext.ShopCarts.FirstOrDefault(x => x.Id == CartId);
                }
            }
            else
            {
                Cart = (ShopCart)dbContext.ShopCarts.FirstOrDefault(x => x.UserId.Equals(user.Id));
            }


            ViewData["ShopCart"] = Cart;
            List<ShopCartItem> ShopCartItems = (List<ShopCartItem>)Cart.ShopCartItems;
            List<InsufficientStock> insufficientStocks = (List<InsufficientStock>)dbContext.InsufficientStocks.ToList();
            ViewBag.stock = insufficientStocks;
            List<ShopCartItem> stocklist = new List<ShopCartItem>();
            float total = 0;
            foreach (var item in insufficientStocks)
            {
                stocklist.Add(item.ShopCartItem);
            }
            ViewBag.stocklist = stocklist;
            for (int i = 0; i < ShopCartItems.Count; i++)
            {
                total += ShopCartItems[i].Product.Price * ShopCartItems[i].Quantity;
            }
            
            ViewBag.Total = total;
            return View();
        }

        
        private void stringtocart(string cartstr, Guid CartId)
        {
            string[] strarr = cartstr.Split(',');
            Dictionary<Guid,int> freqdict = new Dictionary<Guid,int>();
            ShopCart Cart = dbContext.ShopCarts.FirstOrDefault(x => x.Id == CartId);
            foreach(string str in strarr)
            {
                Guid productid = Guid.Parse(str);
                Debug.WriteLine("str:" + str);
                Debug.WriteLine("productid:"+productid);
                if (freqdict.ContainsKey(productid))
                {
                    freqdict[productid]++;
                }
                else
                {
                    freqdict.Add(productid, 1);
                }
            }

            foreach(KeyValuePair<Guid,int> item in freqdict)
            {
                Debug.WriteLine("Key:"+item.Key);
                Product product = dbContext.Products.FirstOrDefault(x => x.Id == item.Key);
                ShopCartItem cartitem = new ShopCartItem(product)
                {
                    Quantity = item.Value,
                    ShopCartId= Cart.Id
                };
                dbContext.ShopCartItems.Add(cartitem);
            }
            dbContext.SaveChanges();
        }

        //public IActionResult PlusToCart(ShopCartItem item)
        //{
        //    item.Quantity++;

        //    dbContext.ShopCartItems.Update(item);
        //    dbContext.SaveChanges();

        //    return RedirectToAction("Index");

        //}

        public IActionResult PlusToCart([FromBody] ShopCartItem req)
        {
            ShopCartItem item = dbContext.ShopCartItems.FirstOrDefault(x => x.Id.Equals(req.Id));
            if(item != null)
            {
                item.Quantity++;
                dbContext.ShopCartItems.Update(item);
                dbContext.SaveChanges();
                return Json(new { status = "success" });
            }

            return Json(new { status = "fail" });
        }

        public IActionResult MinusFromCart([FromBody] ShopCartItem req)
        {
            ShopCartItem item = dbContext.ShopCartItems.FirstOrDefault(x => x.Id.Equals(req.Id));
            if (item != null)
            {
                item.Quantity--;
                if(item.Quantity <= 0)
                {
                    return RemoveFromCart1(req);
                }
            
                dbContext.ShopCartItems.Update(item);
                dbContext.SaveChanges();
                return Json(new { status = "success" });
            }

            return Json(new { status = "fail" });
        }

        //public IActionResult MinusFromCart(ShopCartItem item)
        //{
        //    item.Quantity--;
        //    if (item.Quantity == 0)
        //    {
        //        return RemoveFromCart1(item);
        //    }        

        //    dbContext.ShopCartItems.Update(item);
        //    dbContext.SaveChanges();

        //    return RedirectToAction("Index");


        //}

        //public IActionResult RemoveFromCart1(ShopCartItem item)
        //{
        //    dbContext.ShopCartItems.Remove(item);

        //    dbContext.SaveChanges();

        //    return RedirectToAction("Index");
        //}

        public IActionResult RemoveFromCart1([FromBody] ShopCartItem req)
        {
            ShopCartItem item = dbContext.ShopCartItems.FirstOrDefault(x => x.Id.Equals(req.Id));
            if (item != null)
            {
                dbContext.ShopCartItems.Remove(item);
                dbContext.SaveChanges();
                return Json(new { status = "success" });
            }

            return Json(new { status = "fail" });
        }


        public IActionResult RemoveFromCart2(InsufficientStock stock)
        {
            ShopCartItem shopCartItem = dbContext.ShopCartItems.FirstOrDefault(x=> x.Id == stock.ShopCartItemId);
            dbContext.ShopCartItems.Remove(shopCartItem);
            dbContext.InsufficientStocks.Remove(stock);
            dbContext.SaveChanges();

            return RedirectToAction("Index");
        }

        public IActionResult ChangeQ(InsufficientStock stock)
        {
            if (stock.Quantity == 0)
            {
                return RemoveFromCart2(stock);
            }
            else
            {
                ShopCartItem shopCartItem = dbContext.ShopCartItems.FirstOrDefault(x => x.Id == stock.ShopCartItemId);
                shopCartItem.Quantity = stock.Quantity;
                dbContext.ShopCartItems.Update(shopCartItem);
                dbContext.InsufficientStocks.Remove(stock);
                dbContext.SaveChanges();

                return RedirectToAction("Index");
            }

        }

        public IActionResult CheckOutCart()
        {
            User user = dbContext.Users.FirstOrDefault(x => (Request.Cookies["SessionId"] != null) && (x.sessionId == Guid.Parse(Request.Cookies["SessionId"])));
            List<InsufficientStock> insufficientStocks = dbContext.InsufficientStocks.ToList();
            ShopCart ShopCart = (ShopCart)dbContext.ShopCarts.FirstOrDefault(x => (Request.Cookies["SessionId"] != null) && (x.UserId.Equals(user.Id)));
            

            if (user == null)
            {

                return RedirectToAction("Index", "LogIn");
            }

            else
            {
                ShopCart = (ShopCart)dbContext.ShopCarts.FirstOrDefault(x => x.UserId.Equals(user.Id)); //Get updated Cart 
                List<ShopCartItem> items = (List<ShopCartItem>)ShopCart.ShopCartItems;

                for (int i = 0; i < items.Count; i++)
                {
                    ShopCartItem curr = items[i];
                    List<InventoryRecord> invlist = dbContext.InventoryRecords.Where(x => x.ProductId == curr.ProductId).ToList();
                    if (invlist.Count < curr.Quantity)
                    {
                        InsufficientStock insufficientStock = new InsufficientStock
                        {
                            ShopCartItemId = curr.Id,
                            Quantity = invlist.Count
                        };
                        if (!insufficientStocks.Contains(insufficientStock))
                        {
                            dbContext.InsufficientStocks.Add(insufficientStock);
                            dbContext.SaveChanges();
                        }
                    }
                }
                insufficientStocks = dbContext.InsufficientStocks.ToList();

                if (insufficientStocks.Count != 0)
                {
                    return RedirectToAction("Index");
                }
                else
                {
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

                    dbContext.SaveChanges();

                    return RedirectToAction("Index", "Purchase");
                }

            }
        }



        public IActionResult Home()
        {
            return RedirectToAction("Index", "Search");
        }


    }
}
