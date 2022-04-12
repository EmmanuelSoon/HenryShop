using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using CA1.Models;
using CA1.Data;
using CA1.Data.Service;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;


namespace CA1.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductService service;
        private readonly DBContext dbContext;

        public ProductController(IProductService service)
        {
            this.service = service;
        }

        public async Task<IActionResult> Index()
        {
            var data = await service.GetAllAsync();
            return View(data);
        }





        //Products/Create
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create([Bind("Name,Img,Price,Desc")] Product product)
        {
            if (!ModelState.IsValid)
            {
                return View(product);
            }
            await service.AddAsync(product);
            return RedirectToAction("Index","Search");
        }

        //Product/Detail
        public async Task<IActionResult> Details(Guid Id)
        {
            var productDetails = await service.GetByIdAsync(Id);

            if (productDetails == null) return View("The product does not exist");
            return View(productDetails);
        }

        //Product/Update
        public async Task<IActionResult> Update(Guid Id)
        {
            var productDetails = await service.GetByIdAsync(Id);
            if (productDetails == null) return View("The product does not exist");
            return View(productDetails);
        }
        [HttpPost]
        public async Task<IActionResult> Update(Guid Id,[Bind("Id,Name,Img,Price,Desc")] Product product)
        {
            if (!ModelState.IsValid)
            {
                return View(product);
            }
            await service.UpdateAsync(Id, product);
            return RedirectToAction("Index","Search");
        }

        //Product/Delete
        public async Task<IActionResult> DeleteConfirmation(Guid Id)
        {
            var productDetails = await service.GetByIdAsync(Id);
            if (productDetails == null) return View("The product does not exist");
            return View(productDetails);
        }
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> Delete(Guid Id, [Bind("Id,Name,Img,Price,Desc")] Product product)
        {
            var productDetails = await service.GetByIdAsync(Id);
            if (productDetails == null) return View("The product does not exist");

            await service.DeleteAsync(Id);
            return RedirectToAction("Index", "Search");
        }
    }
}
