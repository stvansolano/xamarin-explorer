using System;
using System.Threading.Tasks;
using Microsoft.AppCenter.Crashes;
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
			AddCommand = new Command(async () => await AddAsync());
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

		private async Task AddAsync()
		{
			if (IsBusy || string.IsNullOrEmpty(ToDoText))
				return;

			try
			{
				IsBusy = true;

				var model = new Item { Text = ToDoText };
				Items.Add(model);

					await Repository.PostAsync(model);

				ToDoText = string.Empty;
			}
			catch (Exception ex)
			{
				Crashes.TrackError(ex);
			}
			finally
			{
				IsBusy = false;
			}
		}
	}
}