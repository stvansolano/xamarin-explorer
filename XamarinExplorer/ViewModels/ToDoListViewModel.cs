using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AppCenter.Crashes;
using Microsoft.AspNetCore.SignalR.Client;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Shared;
using Xamarin.Forms;

namespace XamarinExplorer.ViewModels
{
	public class ToDoListViewModel : BaseViewModel
	{
		public ObservableCollection<ItemViewModel> Items { get; private set; } = new ObservableCollection<ItemViewModel>();
		private ToDoItemsRepository Repository { get; } = DependencyService.Get<IRepository<Item>>() as ToDoItemsRepository ?? new ToDoItemsRepository();

		public Command AddCommand { get; private set; }

		public string StatusText
		{
			get => IsHubConnected ? "Connected" : "Disconnected";
		}

		public ToDoListViewModel()
		{
			AddCommand = new Command(async () =>
			{
				var todo = await AddAsync();

				ToDoText = string.Empty;
			});

			LoadItemsCommand = new Command(async () => {
				Items.Clear();
				await CheckRemoteItemsAsync();
				IsBusy = false;
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
				SetProperty(ref _toDoText, value, nameof(ToDoText));
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

		public Command LoadItemsCommand { get; set; }

		private async Task<Item> AddAsync()
		{
			if (IsBusy || string.IsNullOrEmpty(ToDoText))
				return null;

			try
			{
				IsBusy = true;

				var todo = new Item { Id = Guid.NewGuid().ToString(), Title = ToDoText };
				Items.Insert(0, new ItemViewModel(todo) { UpdateCommand = new Command<ItemViewModel>(async param => await UpdateItemAsync(param)) });

				await Repository.PostAsync(todo);

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

		private async Task UpdateItemAsync(ItemViewModel item)
		{
			await Repository.UpdateAsync(item.Id, item.Model);
		}

		private async Task CheckRemoteItemsAsync()
		{
			IsBusy = true;

			try
			{
				var todoItems = await Repository.GetAsync(true);

				foreach (var toDo in todoItems)
				{
					if (Items.Any(item => item.Id == toDo.Id))
					{
						continue;
					}
					Items.Add(new ItemViewModel(toDo) { UpdateCommand = new Command<ItemViewModel>(async param => await UpdateItemAsync(param)) });
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

			hubConnection.On<object>("notify", async (message) =>
			{
				if (message is JsonElement json)
				{
					try
					{
						var toDo = JsonConvert.DeserializeObject<Item>(json.GetRawText());

						if (!Items.Any(item => item.Id == toDo.Id))
						{
							Items.Insert(0, new ItemViewModel(toDo) { UpdateCommand = new Command<ItemViewModel>(async param => await UpdateItemAsync(param)) });
						}

						await CheckRemoteItemsAsync();
					}
					catch (Exception ex)
					{
						Crashes.TrackError(ex);
					}
				}
			});

			hubConnection.On<object>("notify_update", (message) =>
			{
				if (message is JsonElement json)
				{
					JsonElement element;
					var canParse = json.TryGetProperty("Data", out element);

					if (!canParse)
					{
						return;
					}
					var encoded = element.GetString();
					var todo = JsonConvert.DeserializeObject<Item>(encoded);
					var match = Items.FirstOrDefault(item => item.Id == todo.Id);

					if (match != null)
					{
						match.IsCompleted = todo.IsCompleted;
						match.Title = todo.Title;
					}
					Console.WriteLine("received:" + message);

				}
			});
		}

		#endregion
	}

}