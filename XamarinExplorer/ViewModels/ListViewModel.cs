using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using Xamarin.Forms;
using XamarinExplorer.Services;

namespace XamarinExplorer.ViewModels
{
	public class ListViewModel<T> : BaseViewModel
		where T : class
	{
		public ObservableCollection<T> Items { get; set; }
		public Command LoadItemsCommand { get; set; }
		public IRepository<T> Repository { get; }

		public ListViewModel(IRepository<T> repository)
		{
			Repository = repository;
			Title = "Home";
			Items = new ObservableCollection<T>();
			LoadItemsCommand = new Command(async () => await LoadItemsAsync());
		}

		public async Task LoadItemsAsync()
		{
			if (IsBusy)
				return;

			IsBusy = true;

			try
			{
				Items.Clear();
				var items = await Repository.GetAsync(true);
				foreach (var item in items)
				{
					Items.Add(item);
				}
			}
			catch (Exception ex)
			{
				Debug.WriteLine(ex);
			}
			finally
			{
				IsBusy = false;
			}
		}
	}
}