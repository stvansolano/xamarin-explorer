using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.WordPress
{
	public class Up
	{
		public bool embeddable { get; set; }
		public string href { get; set; }
	}

	public class WpPostType
	{
		public string href { get; set; }
	}

	public class WP_Category
	{
		public int id { get; set; }
		public int count { get; set; }
		public string description { get; set; }
		public string link { get; set; }
		public string Name { get; set; }
		public string slug { get; set; }
		public string taxonomy { get; set; }
		public int parent { get; set; }
		public List<object> meta { get; set; }
		public Links _links { get; set; }
	}
}