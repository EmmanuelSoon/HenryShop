using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using CA1.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace CA1.Data.Service
{
    public class ProductService : IProductService
	{
        private readonly DBContext dbContext;

        public ProductService(DBContext dbContext)
		{
            this.dbContext = dbContext;
		}

        public async Task AddAsync(Product product)
        {
            await dbContext.Products.AddAsync(product);
            await dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid Id)
        {
            var results = await dbContext.Products.FirstOrDefaultAsync(n => n.Id == Id);
            dbContext.Products.Remove(results);
            await dbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            var result = await dbContext.Products.ToListAsync();
            return result;
        }

        public async Task<Product> GetByIdAsync(Guid Id)
        {
            var results = await dbContext.Products.FirstOrDefaultAsync(n => n.Id == Id);
            return results;
        }

        public async Task<Product> UpdateAsync(Guid Id, Product newProduct)
        {
            dbContext.Update(newProduct);
            await dbContext.SaveChangesAsync();
            return newProduct;
        }
    }
}

