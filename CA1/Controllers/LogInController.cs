using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using CA1.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

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

            User user = dbContext.Users.FirstOrDefault(x =>
                     x.UserName == username &&
                     x.PassHash == password
            );



            if (user == null && (!string.IsNullOrEmpty(username) || !string.IsNullOrEmpty(password)))
            {
                message = "Incorrect Username/Password";
            }

            if (user != null && username == user.UserName && password == user.PassHash)
            {
                user.sessionId = Guid.NewGuid();
                dbContext.SaveChanges();


                // ask browser to save and send back these cookies next time
                Response.Cookies.Append("SessionId", user.sessionId.ToString());
                Response.Cookies.Append("Username", user.UserName);

                return RedirectToAction("Index", "Search");
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
                    // commit to save changes
                    dbContext.SaveChanges();
                }
            }

            // ask client to remove these cookies so that
            // they won't be sent over next time
            Response.Cookies.Delete("SessionId");
            Response.Cookies.Delete("Username");

            return RedirectToAction("Index", "LogIn");
        }
    }
}

