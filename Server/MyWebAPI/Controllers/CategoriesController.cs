using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Category = AdventureWorks.SqlServer.Models.ProductCategory;
using AdventureWorksContext = AdventureWorks.SqlServer.Models.AdventureworksContext;

namespace MyWebAPI.Controllers
{

	[Route("api/[controller]")]
	[ApiController]
    public class CategoriesController
    {
		public AdventureWorksContext Context { get; }

		public CategoriesController(AdventureWorksContext context)
		{
			Context = context;
		}

        [HttpGet]
		public IEnumerable<Category> Get()
		{
			return Context.ProductCategory.ToArray();
		}
    }
}