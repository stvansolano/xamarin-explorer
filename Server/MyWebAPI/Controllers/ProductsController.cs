using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AdventureWorksContext = AdventureWorks.SqlServer.Models.AdventureworksContext;
using Product = AdventureWorks.SqlServer.Models.Product;

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
		public IActionResult Post([FromBody]Product value)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest();
			}
			var model = Context.Product.Find(value.ProductId);
			if (model == null)
			{
				return NotFound();
			}
			model.ListPrice = value.ListPrice;
			model.ModifiedDate = DateTime.Now;
			model.Color = value.Color;
			model.ProductModel = value.ProductModel;
			model.ProductNumber = value.ProductNumber;

			model.Size = value.Size;
			model.StandardCost = value.StandardCost;
			model.Weight = value.Weight;

			Context.Product.Add(model);

			Context.SaveChanges();

			return Ok();
		}

		// PUT api/[controller]/5
		[HttpPut("{id}")]
		public async Task<IActionResult> Put(int id, [FromBody]Product value)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest();
			}
			var model = Context.Product.Find(id);
			if (model == null)
			{
				return NotFound();
			}
			model.ListPrice = value.ListPrice;
			model.ModifiedDate = DateTime.Now;
			model.Color = value.Color;
			model.ProductModel = value.ProductModel;
			model.ProductNumber = value.ProductNumber;

			model.Size = value.Size;
			model.StandardCost = value.StandardCost;
			model.Weight = value.Weight;

			Context.Product.Update(model);

			await Context.SaveChangesAsync();

			return Ok();
		}

		// DELETE api/[controller]/5
		[HttpDelete("{id}")]
		public IActionResult Delete(int id)
		{
			return base.Unauthorized();
		}
	}
}
