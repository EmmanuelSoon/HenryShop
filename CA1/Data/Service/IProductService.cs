using System;
using CA1.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CA1.Data.Service
{
	public interface IProductService
	{
		//https://docs.microsoft.com/en-us/dotnet/api/system.collections.ienumerable?view=netcore-3.1
		Task<IEnumerable<Product>> GetAllAsync();

		Task<Product> GetByIdAsync(Guid Id);

		Task AddAsync(Product product);

		Task<Product> UpdateAsync(Guid Id, Product newProduct);

		Task DeleteAsync (Guid Id);
	}
}
