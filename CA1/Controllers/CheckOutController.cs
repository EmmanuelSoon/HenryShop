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

        public CheckOutController(DBContext dbContext)
        {
            this.dbContext = dbContext;


        }
        public IActionResult Index()
        {
            User user = dbContext.Users.FirstOrDefault(x => x.sessionId == Guid.Parse(Request.Cookies["SessionId"]));
            if (user == null)
            {
                return RedirectToAction("Index", "logIn");
            }

            ShopCart ShopCart = (ShopCart)dbContext.ShopCarts.FirstOrDefault(x => x.UserId.Equals(user.Id));

            ViewData["ShopCart"] = ShopCart;
            List<ShopCartItem> ShopCartItems = (List<ShopCartItem>)ShopCart.ShopCartItems;
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


        public IActionResult PlusToCart(ShopCartItem item)
        {
            item.Quantity++;

            dbContext.ShopCartItems.Update(item);
            dbContext.SaveChanges();

            return RedirectToAction("Index");

        }

        public IActionResult MinusFromCart(ShopCartItem item)
        {
            item.Quantity--;
            if (item.Quantity == 0)
            {
                return RemoveFromCart1(item);
            }        
            
            dbContext.ShopCartItems.Update(item);
            dbContext.SaveChanges();

            return RedirectToAction("Index");


        }

        public IActionResult RemoveFromCart1(ShopCartItem item)
        {
            dbContext.ShopCartItems.Remove(item);

            dbContext.SaveChanges();

            return RedirectToAction("Index");
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
            User user = dbContext.Users.FirstOrDefault(x => x.sessionId == Guid.Parse(Request.Cookies["SessionId"]));
            List<InsufficientStock> insufficientStocks = dbContext.InsufficientStocks.ToList();
            ShopCart ShopCart = (ShopCart)dbContext.ShopCarts.FirstOrDefault(x => x.UserId.Equals(user.Id));
            List<ShopCartItem> items = (List<ShopCartItem>) ShopCart.ShopCartItems;

            if (user == null)
            {
                return RedirectToAction("Index", "logIn");
            }

            else
            {
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
