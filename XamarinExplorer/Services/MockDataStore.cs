using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using XamarinExplorer.Models;

namespace XamarinExplorer.Services
{
	public class MockDataStore : Repository<Item>
	{
		List<Item> items;

		public MockDataStore()
		{
			items = new List<Item>();
			var mockItems = new List<Item>
			{
				new Item { Id = Guid.NewGuid().ToString(), Text = "First item", Description="This is an item description." },
				new Item { Id = Guid.NewGuid().ToString(), Text = "Second item", Description="This is an item description." },
				new Item { Id = Guid.NewGuid().ToString(), Text = "Third item", Description="This is an item description." },
				new Item { Id = Guid.NewGuid().ToString(), Text = "Fourth item", Description="This is an item description." },
				new Item { Id = Guid.NewGuid().ToString(), Text = "Fifth item", Description="This is an item description." },
				new Item { Id = Guid.NewGuid().ToString(), Text = "Sixth item", Description="This is an item description." },
			};

			foreach (var item in mockItems)
			{
				items.Add(item);
			}
		}

		public override async Task<bool> AddAsync(Item item)
		{
			items.Add(item);

			return await Task.FromResult(true);
		}

		public override async Task<bool> UpdateAsync(object id, Item item)
		{
			var _item = items.FirstOrDefault(arg => arg.Id.Equals(id));
			items.Remove(_item);
			items.Add(item);

			return await Task.FromResult(true);
		}

		public override async Task<bool> DeleteAsync(object id)
		{
			var _item = items.FirstOrDefault(arg => arg.Id.Equals(id));
			items.Remove(_item);

			return await Task.FromResult(true);
		}

		public override async Task<Item> GetAsync(string id)
		{
			return await Task.FromResult(items.FirstOrDefault(s => s.Id.Equals(id)));
		}

		public override async Task<IEnumerable<Item>> GetAsync(bool forceRefresh = false)
		{
			await Task.Delay(2000);
			return await Task.FromResult(items);
		}
	}
}