using System;
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

		public string StatusText
		{
			get => IsHubConnected ? "Connected" : "Disconnected";
		}

		public ToDoListViewModel(ToDoItemsRepository repository)
			: base(repository)
		{
			Repository = repository;
			AddCommand = new Command(async () => {
				var todo = await AddAsync();
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

		private bool _isHubConnected;
		public bool IsHubConnected
		{
			get => _isHubConnected;
			set
			{
				SetProperty(ref _isHubConnected, value);
				OnPropertyChanged(nameof(StatusText));
			}
		}

		private async Task<Item> AddAsync()
		{
			if (IsBusy || string.IsNullOrEmpty(ToDoText))
				return null;

			try
			{
				IsBusy = true;

				var todo = new Item { Id = Guid.NewGuid().ToString(), Text = ToDoText };
				Items.Insert(0, todo);

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
				var sorted = Items.OrderByDescending(todo => todo.DateCreated).ToList();

				var sortQuery = Items.Select(todo => new { OldIndex = Items.IndexOf(todo), NewIndex = sorted.IndexOf(todo) })
									 .ToList();

				sortQuery.ForEach(sort => Items.Move(sort.OldIndex, sort.NewIndex));
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

		Microsoft.AspNetCore.SignalR.Client.HubConnection hubConnection;

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

		private void Init()
		{
			var url = $"{AppConstants.SignalRHub}";

			hubConnection =
				new HubConnectionBuilder()
					.WithUrl(url)
					.WithAutomaticReconnect()
					.Build();

			hubConnection.Reconnected += async (error) =>
			{
				IsHubConnected = false;
				await Task.Delay(new Random().Next(0, 5) * 1000);
				IsHubConnected = true;

				Debug.WriteLine("SignalR reconnected...");
			};

			hubConnection.On<object>("notify", async(message) =>
			{
				if (message is JsonElement json)
				{
					try
					{
						var toDo = JsonConvert.DeserializeObject<Item>(json.GetRawText());

						if (!Items.Any(item => item.Id == toDo.Id))
						{
							Items.Insert(0, toDo);
						}

						await CheckRemoteItemsAsync();
					}
					catch (Exception ex)
					{
						Crashes.TrackError(ex);
					}
				}
			});
		}

		#endregion
	}
}