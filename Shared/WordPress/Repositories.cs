using Shared;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Shared.WordPress;

namespace Shared.WordPress.Services
{
	public class PostRepository : IRepository<WP_Post>
	{
		public async Task<IEnumerable<WP_Post>> GetAsync(bool forceRefresh = false)
		{
			return await WordPressApi.Instance.GetPosts();
		}
	}

	public class CategoryRepository : IRepository<WP_Category>
	{
		public async Task<IEnumerable<WP_Category>> GetAsync(bool forceRefresh = false)
		{
			return await WordPressApi.Instance.GetCategories();
		}
	}
}