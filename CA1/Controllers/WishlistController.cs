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
    public class WishlistController : Controller
    {
        private DBContext dbContext;

        public WishlistController(DBContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public IActionResult Index()
        {
            User user = dbContext.Users.FirstOrDefault(x => (Request.Cookies["SessionId"] != null) && (x.sessionId == Guid.Parse(Request.Cookies["SessionId"])));
            if(user == null)
            {
                return RedirectToAction("Index", "LogIn");
            }
            else
            {
                WishList wishList = dbContext.WishLists.FirstOrDefault(x => x.Id == user.Id);
                List<WishListItem> wishListItems = dbContext.WishListItems.Where(x => x.WishListId == wishList.Id).ToList();
                List<int> stockcount = new List<int>();

                foreach(WishListItem item in wishListItems)
                {
                    List<InventoryRecord> record = dbContext.InventoryRecords.Where(x => x.ProductId == item.ProductId).ToList();
                    if(record.Count > 0)
                    {
                        stockcount.Add(record.Count);
                    }
                    else
                    {
                        stockcount.Add(0);
                    }
                }

                ViewBag.WishList = wishListItems;
                ViewBag.StockCount = stockcount;
            }

            return View();
        }
    }
}
