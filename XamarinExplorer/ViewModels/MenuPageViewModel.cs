using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.AppCenter.Crashes;
using Shared.WordPress;
using Xamarin.Forms;
using XamarinExplorer.Services;

namespace XamarinExplorer.ViewModels
{
	public class MenuPageViewModel : BaseViewModel
	{
		private bool _isRefreshingCategories;

		public ICollection<MenuItem> Menu { get; } = new ObservableCollection<MenuItem>();
		public IList<WP_Category> Categories { get; } = new ObservableCollection<WP_Category>();
		public IList<WP_Post> Posts { get; } = new ObservableCollection<WP_Post>();
		public ICommand LoadCategoriesCommand { get; set; }

		public MenuPageViewModel()
		{
			LoadCategoriesCommand = new Command(async (obj) => await LoadCategoriesAsync());
		}

		public bool IsRefreshingCategories
		{
			get => _isRefreshingCategories;
			set
			{
				SetProperty(ref _isRefreshingCategories, value);
			}
		}

		public async Task LoadCategoriesAsync()
		{
			IsRefreshingCategories = true;

			try
			{
				var data = await DependencyService.Get<IRepository<WP_Category>>().GetAsync();

				Categories.Clear();

				foreach (var item in data)
				{
					Categories.Add(item);
				}
			}
			catch (Exception ex)
			{
				Crashes.TrackError(ex);
			}
			finally
			{
				IsRefreshingCategories = false;
			}
		}
	}
}
