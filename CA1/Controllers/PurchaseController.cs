using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using CA1.Models;
using Microsoft.AspNetCore.Http;
using System.Linq;
using System;
using System.Collections.Generic;

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
            Session session = ValidateSession();
            if (session == null)
            {
                return RedirectToAction("Index", "Login");
            }
            //Get UserId From session
            Guid UserId = session.UserId;

            List<Order> Orders = dbContext.Orders.Where(x =>
                x.UserId == UserId).ToList();
            ViewData["Orders"] = Orders;
            return View();
        }
        private Session ValidateSession()
        {
            if(Request.Cookies["SessionId"] == null)
            {
                return null;
            }
            Guid SessionId = Guid.Parse(Request.Cookies["SessionId"]);
            Session session = dbContext.Sessions.FirstOrDefault(x =>
                        x.Id == SessionId
            );
            return session;
        }
    }

    
}
