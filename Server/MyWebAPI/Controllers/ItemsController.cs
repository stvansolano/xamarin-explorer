using Microsoft.AspNetCore.Mvc;
using Shared;
using System.Collections.Generic;
using System.Linq;

namespace MyWebAPI.Controllers
{
    [Route("api/[controller]")]
	[ApiController]
	public class ItemsController : ControllerBase
	{
        private List<Item> _items = new List<Item> {
            new Item { Text = "values 1" },
            new Item { Text = "values 2" },
            new Item { Text = "values 3" }
        };

        private const string GET_BY_ID = "Get";

        // GET api/{Controller}
        [HttpGet]
		public ActionResult<IEnumerable<Item>> Get()
		{
			return _items;
		}

		// GET api/{Controller}/5
		[HttpGet("{id}", Name = GET_BY_ID)]
		public ActionResult<Item> Get(int id)
		{
			return _items.ElementAtOrDefault(id);
		}

		// POST api/{Controller}
		[HttpPost]
		public IActionResult Post([FromBody] Item value)
		{
            if (!ModelState.IsValid)
            {   
                return BadRequest();
            }

            _items.Add(value);

            return CreatedAtAction(GET_BY_ID, new { id = _items.IndexOf(value) });
        }

		// PUT api/{Controller}/5
		[HttpPut("{id}")]
		public IActionResult Put(int id, [FromBody] Item value)
		{
            if (_items.ElementAtOrDefault(id) == null)
            {
                return NotFound();
            }
            _items[id] = value;

            return Ok();
		}

		// DELETE api/{Controller}/5
		[HttpDelete("{id}")]
		public IActionResult Delete(int id)
		{
            if (_items.ElementAtOrDefault(id) == null)
            {
                return NotFound();
            }
            _items.RemoveAt(id);

            return Ok();
        }
	}
}
