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
            if (user == null)
            {
                return RedirectToAction("Index", "LogIn");
            }
            else
            {
                WishList wishList = dbContext.WishLists.FirstOrDefault(x => x.UserId == user.Id);
                if (wishList != null)
                {
                    List<WishListItem> wishListItems = dbContext.WishListItems.Where(x => x.WishListId == wishList.Id).OrderByDescending(x => x.TimeStamp).ToList();
                    List<int> stockcount = new List<int>();

                    foreach (WishListItem item in wishListItems)
                    {
                        List<InventoryRecord> record = dbContext.InventoryRecords.Where(x => x.ProductId == item.ProductId).ToList();
                        if (record.Count > 0)
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
            }
            return View();
        }

        public IActionResult AddToWishList([FromBody] Product req)
        {
            User user = dbContext.Users.FirstOrDefault(x => (Request.Cookies["SessionId"] != null) && (x.sessionId == Guid.Parse(Request.Cookies["SessionId"])));
            if (user == null)
            {
                return Json(new
                {
                    status = "needlogin"
                });
            }
            Product product = dbContext.Products.FirstOrDefault(x => x.Id == req.Id);
            if (!IsExistedInWishList(product.Id, user))
            {
                WishList wishlist = dbContext.WishLists.FirstOrDefault(x => x.UserId == user.Id);
                WishListItem item = new WishListItem
                {
                    ProductId = product.Id,
                    WishListId = wishlist.Id,
                    Product = product
                };
                wishlist.WishListItems.Add(item);
                dbContext.WishListItems.Add(item);
                dbContext.SaveChanges();
                return Json(new
                {
                    status = "success",
                    name = product.Name,
                });
            }
            else
            {
                return Json(new
                {
                    status = "existed",
                    name = product.Name,
                });
            }

        }

        public bool IsExistedInWishList(Guid ProductId, User user)
        {
            if (user != null)
            {
                WishList wishlist = dbContext.WishLists.FirstOrDefault(x => x.UserId == user.Id);
                if (wishlist != null)
                {
                    WishListItem wishListItem = dbContext.WishListItems.FirstOrDefault(x => x.WishListId == wishlist.Id && x.ProductId == ProductId);
                    if (wishListItem != null)
                    {
                        return true;
                    }
                }
            }
            return false;

        }

        public IActionResult RemoveFromWishList([FromBody] Product delReq)
        {
            bool flag = false;
            if (!flag)
            {
                User user = dbContext.Users.FirstOrDefault(x => (Request.Cookies["SessionId"] != null) && (x.sessionId == Guid.Parse(Request.Cookies["SessionId"])));

                WishList userWL = dbContext.WishLists.FirstOrDefault(x => x.UserId == user.Id);

                WishListItem toDelete = dbContext.WishListItems.FirstOrDefault(x => x.WishListId == userWL.Id && x.ProductId == delReq.Id);

                dbContext.WishListItems.Remove(toDelete);
                dbContext.SaveChanges();
                flag = true;
            }
            if (flag)
            {
                return Json(new { status = "success" });
            }
            else
            {
                return Json(new { status = "error" });
            }
        }
    }
}
