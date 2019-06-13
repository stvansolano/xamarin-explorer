﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Shared;

namespace MyWebAPI.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ItemsController : ControllerBase
	{
		// GET api/{Controller}
		[HttpGet]
		public ActionResult<IEnumerable<Item>> Get()
		{
			return new [] { 
				new Item { Text = "values 1" }, 
				new Item { Text = "values 2" },
				new Item { Text = "values 3" }
				};
		}

		// GET api/{Controller}/5
		[HttpGet("{id}")]
		public ActionResult<Item> Get(int id)
		{
			return new Item { Text = "value" };
		}

		// POST api/{Controller}
		[HttpPost]
		public void Post([FromBody] string value)
		{
		}

		// PUT api/{Controller}/5
		[HttpPut("{id}")]
		public void Put(int id, [FromBody] string value)
		{
		}

		// DELETE api/{Controller}/5
		[HttpDelete("{id}")]
		public void Delete(int id)
		{
		}
	}
}
