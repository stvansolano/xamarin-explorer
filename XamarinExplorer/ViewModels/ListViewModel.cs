using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AppCenter.Analytics;
using Shared;
using Xamarin.Forms;

namespace XamarinExplorer.ViewModels
{
	public class ListViewModel<T> : BaseViewModel
		where T : class
	{
		public virtual ObservableCollection<T> Items { get; private set; }
		public Command LoadItemsCommand { get; set; }
		public Command AddMoreCommand { get; set; }
		public IRepository<T> Repository { get; }

		public ListViewModel()
		{ }

		public ListViewModel(IRepository<T> repository)
		{
			Repository = repository;
			Title = "Home";
            
			Items = new ObservableCollection<T>();
			LoadItemsCommand = new Command(async () => await LoadItemsAsync());
			AddMoreCommand = new Command(async () => await AddMoreAsync());
		}

		public async Task AddMoreAsync()
		{
			await TryAddMore();
		}

		public async Task LoadItemsAsync()
		{
			if (IsBusy)
				return;

			IsBusy = true;
			Items.Clear();
			await TryAddMore();
		}

		public async Task TryAddMore()
		{
			try
			{
				var items = await Repository.GetAsync(true);
				foreach (var item in items)
				{
					Items.Add(item);
				}
			}
			catch (Exception ex)
			{
				Debug.WriteLine(ex);
				Analytics.TrackEvent(AnalyticEvents.HandledException, AnalyticEvents.FromExceptionArgs(ex));
			}
			finally
			{
				IsBusy = false;
			}
		}

		public Predicate<T> FilterPredicate { get; set; }

		private string _filter;
		public virtual string Filter
		{
			get
			{
				return _filter;
			}
			set 
			{
				SetProperty(ref _filter, value);
				OnPropertyChanged(nameof(Items));
			}
		}
	}
}