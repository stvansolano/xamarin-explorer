using Shared;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Shared;
using Shared.AdventureWorks;
using AdventureWorks.SqlServer.Models;

namespace XamarinExplorer.Services
{
	public class ProductRepository : IRepository<Product>
	{
		public async Task<IEnumerable<Product>> GetAsync(bool forceRefresh = false)
		{
			return await AdventureWorksApi.Instance.Get();
		}
	}

	public class CategoryRepository : IRepository<ProductCategory>
	{
		public async Task<IEnumerable<ProductCategory>> GetAsync(bool forceRefresh = false)
		{
			return await AdventureWorksApi.Instance.GetCategories();
		}
	}
}