using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyWebAPI.Security;
using AdventureWorksContext = AdventureWorks.SqlServer.Models.AdventureworksContext;

namespace MyWebAPI.Controllers
{
	[Route("api/[controller]")]
	[Authorize(AuthenticationSchemes = AuthorizedResources.AUTHENTICATION_SCHEMES, Policy = AuthorizedResources.AUTHENTICATION_POLICY)]
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