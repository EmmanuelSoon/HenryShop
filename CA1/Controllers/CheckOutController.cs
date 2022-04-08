using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using CA1.Models;
using Microsoft.AspNetCore.Http;
using System.Linq;
using System.Collections.Generic;
using System;

namespace CA1.Controllers
{
    public class CheckOutController : Controller
    {
        private DBContext dbContext;
        private int UserId;

        public CheckOutController(DBContext dbContext)
        {
            this.dbContext = dbContext;
            this.UserId = 1;
        }
        public IActionResult Index()
        {
            string UserId = HttpContext.Session.GetString("userId");

            ShopCart ShopCart = (ShopCart)dbContext.ShopCarts.FirstOrDefault(x => x.UserId.Equals(UserId));

            ViewData["ShopCart"] = ShopCart;
            List<ShopCartItem> ShopCartItems = (List<ShopCartItem>)ShopCart.ShopCartItems;
            float total = 0;
            for (int i = 0; i < ShopCartItems.Count; i++)
            {
                total += ShopCartItems[i].Product.Price * ShopCartItems[i].Quantity;
            }
            
            ViewBag.Total = total;
            return View();
        }


        public IActionResult PlusToCart(ShopCartItem item)
        {
            item.Quantity++;

            dbContext.ShopCartItems.Update(item);
            dbContext.SaveChanges();

            return RedirectToAction("Index");

        }

        public IActionResult MinusFromCart(ShopCartItem item)
        {
            //Guid UserId = Guid.Parse(Request.Cookies["SessionId"]);
            item.Quantity--;
            if (item.Quantity == 0)
            {
                return RemoveFromCart(item);
            }        
            
            dbContext.ShopCartItems.Update(item);
            dbContext.SaveChanges();

            return RedirectToAction("Index");


        }

        public IActionResult RemoveFromCart(ShopCartItem item)
        {
            dbContext.ShopCartItems.Remove(item);

            dbContext.SaveChanges();

            return RedirectToAction("Index");
        }


        public IActionResult CheckOut(List<ShopCartItem> items)
        {
            // check user has already login or not
            //Session session = ValidateSession();

            //if(session == null)
            //{
            //    return RedirectToAction("Login", "Login");
            //}
            ShopCart cart = dbContext.ShopCarts.FirstOrDefault(x => x.Id == items[0].ShopCartId);
            User user = dbContext.Users.FirstOrDefault(x => x.Id == cart.UserId);
            for (int i = 0; i < items.Count; i++)
            {
                ShopCartItem curr = items[i];
                List<InventoryRecord> invlist = dbContext.InventoryRecords.Where(x => x.ProductId == curr.ProductId && x.IsUsed == false).ToList();
                if (invlist.Count < curr.Quantity)
                {
                    return RedirectToAction("Index"); //need to change to alert customer to change quantity
                }
            }
            for(int i = 0; i < items.Count; i++)
            {
                ShopCartItem curr = items[i];
                List<OrderDetail> orderlist = new List<OrderDetail>();
                List<InventoryRecord> invlist = dbContext.InventoryRecords.Where(x => x.ProductId == curr.ProductId).ToList();

                Order order = new Order
                {

                    ProductId = curr.ProductId,
                    Quantity = curr.Quantity,
                    UserId = user.Id
                };


                for (int j = 0; j < curr.Quantity; j++)
                {
                    InventoryRecord inv = invlist[j];
                    OrderDetail orderDetails = new OrderDetail
                    {
                        OrderId = order.Id,
                        ActivationId = inv.ActivationId,
                    };
                    
                    dbContext.InventoryRecords.Remove(inv);

                }
                order.OrderDetails = orderlist;
                dbContext.Orders.Add(order);
            }

            dbContext.SaveChanges();

            // purchase successful and go to MyPurchase

            return RedirectToAction("Index", "Search");
        }



        public IActionResult Home()
        {
            return RedirectToAction("Index", "Search");
        }



        //private Session ValidateSession()
        //{
        //    if (Request.Cookies["SessionId"] == null)
        //    {
        //        return null;
        //    }

        //    Guid UserId = Guid.Parse(Request.Cookies["SessionId"]);
        //    Session session = dbContext.Sessions.FirstOrDefault(x => x.Id == UserId);

        //    return session;
        //}

    }
}
