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

        // GET: /<controller>/
        public IActionResult Index(IFormCollection form)
        {
            string username = form["username"];
            string password = form["password"];
            string message = "";


            //if (password != "")
            //{
            //    HashAlgorithm sha = SHA256.Create();
            //    byte[] hash = sha.ComputeHash(
            //        Encoding.UTF8.GetBytes(password));

                
                
            //}

            User user = dbContext.Users.FirstOrDefault(x =>
                     x.UserName == username &&
                     x.PassHash == password
            );



            if (user == null && (!string.IsNullOrEmpty(username) || !string.IsNullOrEmpty(password)))
            {
                message = "Username/Password doesnot exist";
            }

            if (user != null && username == user.UserName && password == user.PassHash)
            {
                HttpContext.Session.SetString("username", user.UserName);
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ViewData["ErrorMessage"] = message;
                return View();
            }
        }

        //public IActionResult LogIn(IFormCollection form)
        //{
        //    string username = form["username"];
        //    string password = form["password"];

        //    HashAlgorithm sha = SHA256.Create();
        //    byte[] hash = sha.ComputeHash(
        //        Encoding.UTF8.GetBytes(password));

        //    string message = "";
        //    User user = dbContext.Users.FirstOrDefault(x =>
        //        x.UserName == username &&
        //        x.PassHash == hash
        //    );

        //    if (user == null && (!string.IsNullOrEmpty(username) || !string.IsNullOrEmpty(password)))
        //    {
        //        message = "Username/Password doesnot exist";
        //    }

        //    if (user != null && username == user.UserName && hash == user.PassHash)
        //    {
        //        HttpContext.Session.SetString("username", user.UserName);
        //        return RedirectToAction("Index", "Home");
        //    }
        //    else
        //    {
        //        ViewData["ErrorMessage"] = message;
        //        return View();
        //    }
        //}

        //public IActionResult LogOut()
        //{
        //    HttpContext.Session.Remove("username");
        //    return RedirectToAction("Index");
        //}
    }
}

