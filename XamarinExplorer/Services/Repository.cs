using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Plugin.Connectivity;

namespace XamarinExplorer.Services
{
	public class Repository<T> : IRepository<T>
		where T : class
	{
		HttpClient client;
		IEnumerable<T> items;

		public Repository()
		{
			client = new HttpClient();
			client.BaseAddress = new Uri($"{App.WebServiceUrl}/");

			items = new List<T>();
		}

		public virtual async Task<IEnumerable<T>> GetAsync(bool forceRefresh = false)
		{
			if (forceRefresh && CrossConnectivity.Current.IsConnected)
			{
				var json = await client.GetStringAsync($"api/item");
				items = await Task.Run(() => JsonConvert.DeserializeObject<IEnumerable<T>>(json));
			}

			return items;
		}

		public virtual async Task<T> GetAsync(string id)
		{
			if (id != null && CrossConnectivity.Current.IsConnected)
			{
				var json = await client.GetStringAsync($"api/item/{id}");
				return await Task.Run(() => JsonConvert.DeserializeObject<T>(json));
			}

			return null;
		}

		public virtual async Task<bool> AddAsync(T item)
		{
			if (item == null || !CrossConnectivity.Current.IsConnected)
				return false;

			var serializedItem = JsonConvert.SerializeObject(item);

			var response = await client.PostAsync($"api/item", new StringContent(serializedItem, Encoding.UTF8, "application/json"));

			return response.IsSuccessStatusCode;
		}

		public virtual async Task<bool> UpdateAsync(object id, T item)
		{
			if (item == null || id == null || !CrossConnectivity.Current.IsConnected)
				return false;

			var serializedItem = JsonConvert.SerializeObject(item);
			var buffer = Encoding.UTF8.GetBytes(serializedItem);
			var byteContent = new ByteArrayContent(buffer);

			var response = await client.PutAsync(new Uri($"api/item/{id}"), byteContent);

			return response.IsSuccessStatusCode;
		}

		public virtual async Task<bool> DeleteAsync(object id)
		{
			if (id != null && !CrossConnectivity.Current.IsConnected)
				return false;

			var response = await client.DeleteAsync($"api/item/{id}");

			return response.IsSuccessStatusCode;
		}
	}
}