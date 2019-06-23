using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using AdventureWorksContext = AdventureWorks.SqlServer.Models.AdventureworksContext;
using Product = AdventureWorks.SqlServer.Models.Product;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MyWebAPI.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ProductsController : ControllerBase
	{
		public AdventureWorksContext Context { get; }

		public ProductsController(AdventureWorksContext context)
		{
			Context = context;
		}

		// GET: api/values
		[HttpGet]
		public IEnumerable<Product> Get()
		{
			return Context.Product.ToArray();
		}

		// GET api/[controller]/5
		[HttpGet("{id}")]
		public IActionResult Get(int id)
		{
			var match = Context.Product.FirstOrDefault(model => model.ProductId == id);
			if (match == null)
			{
				return NotFound();
			}
			return Ok( match);
		}

		// POST api/values
		[HttpPost]
		public void Post([FromBody]string value)
		{
		}

		// PUT api/values/5
		[HttpPut("{id}")]
		public void Put(int id, [FromBody]string value)
		{
		}

		// DELETE api/values/5
		[HttpDelete("{id}")]
		public void Delete(int id)
		{
		}
	}
}
