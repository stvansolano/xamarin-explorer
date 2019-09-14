using System.ComponentModel.DataAnnotations;

namespace MyWebAPI.Controllers
{
	public class LoginInputModel
	{
		[Required]
		public string UserName { get; set; }
	}
}