using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Plugin.Connectivity;
//using Xamarin.Essentials;

namespace XamarinExplorer.Services
{
	public class Repository<T> : IRepository<T>
		where T : class
	{
		IEnumerable<T> _items;

        public IHttpFactory Factory { get; }

        public Repository(IHttpFactory factory)
		{
			_items = new List<T>();
            Factory = factory;
		}

		public virtual async Task<IEnumerable<T>> GetAsync(bool forceRefresh = false)
		{
			if (forceRefresh && CrossConnectivity.Current.IsConnected)
			{
				var json = await GetClient().GetStringAsync(string.Empty);
				_items = await Task.Run(() => JsonConvert.DeserializeObject<IEnumerable<T>>(json));
			}

			return _items;
		}

		public virtual async Task<T> GetAsync(string id)
		{
			if (id != null && CrossConnectivity.Current.IsConnected)
			{
				var json = await GetClient().GetStringAsync($"{id}");
				return await Task.Run(() => JsonConvert.DeserializeObject<T>(json));
			}

			return null;
		}

		public virtual async Task<bool> AddAsync(T item)
		{
			if (item == null || !CrossConnectivity.Current.IsConnected)
				return false;

			var serializedItem = JsonConvert.SerializeObject(item);

			var response = await GetClient().PostAsync("", new StringContent(serializedItem, Encoding.UTF8, "application/json"));

			return response.IsSuccessStatusCode;
		}

		public virtual async Task<bool> UpdateAsync(object id, T item)
		{
			if (item == null || id == null || !CrossConnectivity.Current.IsConnected)
				return false;

			var serializedItem = JsonConvert.SerializeObject(item);
			var buffer = Encoding.UTF8.GetBytes(serializedItem);
			var byteContent = new ByteArrayContent(buffer);

			var response = await GetClient().PutAsync(new Uri($"{id}"), byteContent);

			return response.IsSuccessStatusCode;
		}

		public virtual async Task<bool> DeleteAsync(object id)
		{
			if (id != null && !CrossConnectivity.Current.IsConnected)
				return false;

			var response = await GetClient().DeleteAsync($"{id}");

			return response.IsSuccessStatusCode;
		}

		protected HttpClient GetClient()
		{
            return Factory.GetClient();
		}
	}
}