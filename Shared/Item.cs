using System;
using Newtonsoft.Json;

namespace Shared
{
	public class Item
	{
		public int Id { get; set; }

		[JsonProperty("Name")]
		public string Text { get; set; }

		public string Description { get; set; } = "This is the description";

		[JsonProperty("ListedPrice")]
		public string Price { get; set; }
	}
}