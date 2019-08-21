using System.Linq;
using Microsoft.AspNetCore.Mvc;
using AdventureWorksContext = AdventureWorks.SqlServer.Models.AdventureworksContext;

namespace MyWebAPI.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
    public class CustomersController : Controller
    {
		public AdventureWorksContext Context { get; }

		public CustomersController(AdventureWorksContext context)
		{
			Context = context;
		}

        [HttpGet]
		public IActionResult Get()
		{
			return Ok(Context.Customer.Select(c => new {
				c.FirstName,
				c.MiddleName,
				c.LastName,
				c.EmailAddress,
				c.ModifiedDate,
				c.CompanyName,
				c.Title,
				c.Phone
			}).ToArray());
		}
    }
}