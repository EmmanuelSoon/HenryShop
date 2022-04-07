using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using CA1.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;


namespace CA1.Controllers
{
    public class SearchController : Controller
    {
        private DBContext dbContext;
        public SearchController(DBContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public IActionResult Index(string searchstr)
        {   
            if(searchstr == null)
            {
                List<Product> products = dbContext.Products.ToList();
                ViewBag.Products = products;
                searchstr = "";
            }
            else
            {
                List<Product> products = dbContext.Products.Where(x => 
                    x.Name.Contains(searchstr) ||
                    x.Desc.Contains(searchstr) 
                ).ToList();
                ViewBag.Products = products;
            }


            ViewBag.Searchstr = searchstr;
            return View();
        }



        public IActionResult AddtoCart(Product product)
        {   
            string statement;
            if (true)
            {   string name = product.Name;
                statement = name + " added to Cart!";
            }
            TempData["statement"] = statement;

            return RedirectToAction("Index","Search");
        }
    }
}
