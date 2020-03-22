using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AppCenter.Crashes;
using Microsoft.AspNetCore.SignalR.Client;
using Newtonsoft.Json;
using Shared;
using Xamarin.Forms;

namespace XamarinExplorer.ViewModels
{
	public class ToDoListViewModel : ListViewModel<Item>
	{
		public Command AddCommand { get; private set; }

		public new ToDoItemsRepository Repository { get; }
		
		public ToDoListViewModel(ToDoItemsRepository repository)
			: base(repository)
		{
			Repository = repository;
			AddCommand = new Command(async () => {
				var todo = await AddAsync();

				if (todo != null)
				{
					await NotifyAll(todo);
				}
			});

			Init();
		}

		private string _toDoText;
		public string ToDoText
		{
			get => _toDoText;
			set
			{
				_toDoText = value;
				SetProperty(ref _toDoText, value);
			}
		}

		private async Task<Item> AddAsync()
		{
			if (IsBusy || string.IsNullOrEmpty(ToDoText))
				return null;

			try
			{
				IsBusy = true;

				var todo = new Item { Text = ToDoText };
				Items.Add(todo);

				await Repository.PostAsync(todo);

				ToDoText = string.Empty;

				return todo;
			}
			catch (Exception ex)
			{
				Crashes.TrackError(ex);
				return null;
			}
			finally
			{
				IsBusy = false;
			}
		}

		private async Task CheckRemoteItemsAsync()
		{
			IsBusy = true;

			var todoItems = await Repository.GetAsync(true);

			try
			{
				foreach (var toDo in todoItems)
				{
					if (Items.Any(item => item.Id == toDo.Id))
					{
						continue;
					}
					Items.Add(toDo);
				}
			}
			catch (Exception up)
			{
				throw up;
			}
			finally
			{
				IsBusy = false;
			}
		}

		#region SignalR

		//  <PackageReference Include="Microsoft.AspNetCore.SignalR.Client" Version="3.0.0" />

		public bool IsHubConnected { get; private set; }

		Microsoft.AspNetCore.SignalR.Client.HubConnection hubConnection;

		private void Init()
		{
			var url = $"{AppConstants.SignalRUrl}";

			hubConnection =
				new HubConnectionBuilder()
					.WithUrl(url)
					//.WithAutomaticReconnect()
					.Build();

			hubConnection.Closed += async (error) =>
			{
				IsHubConnected = false;
				await Task.Delay(new Random().Next(0, 5) * 1000);
				try
				{
					await ConnectAsync();
				}
				catch (Exception ex)
				{
					Debug.WriteLine(ex);
				}
			};

			hubConnection.On<object>("notify", async(message) =>
			{
				if (message is JsonElement json)
				{
					try
					{
						var item = JsonConvert.DeserializeObject<Item>(json.GetRawText());

						Items.Add(item);

						await CheckRemoteItemsAsync();
					}
					catch (Exception ex)
					{
						Crashes.TrackError(ex);
					}
				}
			});
		}

		public async Task ConnectAsync()
		{
			if (hubConnection == null)
			{
				Init();
			}
			if (IsHubConnected)
				return;

			await hubConnection.StartAsync();
			IsHubConnected = true;
		}

		public async Task DisconnectAsync()
		{
			if (!IsHubConnected)
				return;

			try
			{
				await hubConnection.DisposeAsync();
			}
			catch (Exception ex)
			{
				Debug.WriteLine(ex);
			}

			IsHubConnected = false;
		}

		private async Task NotifyAll(Item item)
		{
			await hubConnection.InvokeAsync("notify", item);
		}

		#endregion
	}
}