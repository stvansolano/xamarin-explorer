using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Shared;
using Xamarin.Essentials;
using XamarinExplorer.Services;

namespace XamarinExplorer
{
	public class ToDoItemsRepository : Repository<Item>
	{
		private List<Item> _items;

		public override async Task<IEnumerable<Item>> GetAsync(bool forceRefresh = false)
		{
			if (forceRefresh && Connectivity.NetworkAccess == NetworkAccess.Internet)
			{
				var json = await GetClient().GetStringAsync("HttpGetTrigger");
				_items = JsonConvert.DeserializeObject<List<Item>>(json);
			}

			return _items.OrderByDescending(item => item.DateCreated);
		}

		public async Task PostAsync(Item model)
		{
			_items.Add(model);

			var content = new StringContent(JsonConvert.SerializeObject(model), System.Text.Encoding.UTF8, "application/json");

			await GetClient().PostAsync("HttpPostTrigger", content);
		}
	}
}