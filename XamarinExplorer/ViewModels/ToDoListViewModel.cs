using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AppCenter.Crashes;
using Microsoft.AspNetCore.SignalR.Client;
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
					.WithAutomaticReconnect()
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

			hubConnection.On<string, string>("notify", async(user, message) =>
			{
				await LoadItemsAsync();
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