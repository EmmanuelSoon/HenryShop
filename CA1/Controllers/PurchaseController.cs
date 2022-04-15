using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using CA1.Models;
using Microsoft.AspNetCore.Http;
using System.Linq;
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Hosting;
using System.IO;

namespace CA1.Controllers
{
    public class PurchaseController : Controller
    {
        private DBContext dbContext;
        public PurchaseController(DBContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public IActionResult Index()
        {
            User usersession = ValidateSession();
            if (usersession == null)
            {
                return RedirectToAction("Index", "Login");
            }
            List<Order> Orders = dbContext.Orders.Where(x =>
                x.UserId == usersession.Id).OrderByDescending(x => x.TimeStamp).ToList();
            ViewData["Orders"] = Orders;
            return View();
        }

        public IActionResult AddReview(Order order)
        {
            Order currOrder = dbContext.Orders.FirstOrDefault(x => x.Id == order.Id);
            Product currproduct = dbContext.Products.FirstOrDefault(x => x.Id == order.ProductId);
            ViewBag.order = currOrder;
            ViewBag.path =  "../../" + currOrder.Product.Img;
            return View();
        }

        public IActionResult SaveReview(string rate, string content, string orderId)
        {

            User usersession = ValidateSession();
            if (usersession == null)
            {
                return RedirectToAction("Index", "Login");
            }
            //can not find current order 
            Order currOrder = dbContext.Orders.FirstOrDefault(x => x.Id == Guid.Parse(orderId));
            if(currOrder == null)
            {
                return RedirectToAction("Index", "Purchase");
            }

            int rating = 0;
            if(rate != null)
            {
                rating = Convert.ToInt32(rate);
            }
            if (content == null)
            {
                content = "";
            }

            ProductReview review = new ProductReview()
            {
                Rating = rating,
                Content = content,
                OrderId = currOrder.Id,
            };
            dbContext.ProductReviews.Add(review);
            currOrder.ProductReview = review;
            dbContext.Orders.Update(currOrder);
            dbContext.SaveChanges();
            return RedirectToAction("Index", "Purchase");
        }
        public IActionResult ReviewDetail([FromBody] Order data)
        {
            User usersession = ValidateSession();
            if (usersession == null)
            {
                return RedirectToAction("Index", "Login");
            }

            if( data != null && data.Id != null)
            {
                ProductReview review = (ProductReview)dbContext.ProductReviews.FirstOrDefault(x => x.OrderId == data.Id);
                return Json(new
                {
                    status = "success",
                    date = review.CreatedDate,
                    rating = review.Rating,
                    Content = review.Content,
                });
            }
            return Json(new { status = "fail" });
        }
        private User ValidateSession()
        {
            if(Request.Cookies["SessionId"] == null)
            {
                return null;
            }
            Guid SessionId = Guid.Parse(Request.Cookies["SessionId"]);
            User usersession = dbContext.Users.FirstOrDefault(x => x.sessionId == SessionId);
            return usersession;
        }
    }
}
