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
            User usersession = ValidateSession();
            if (usersession == null)
            {
                return RedirectToAction("Index", "Login");
            }
            List<Order> Orders = dbContext.Orders.Where(x =>
                x.UserId == usersession.Id).ToList();
            ViewData["Orders"] = Orders;
            return View();
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
