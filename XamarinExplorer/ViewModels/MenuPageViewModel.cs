using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using AdventureWorks.SqlServer.Models;
using Microsoft.AppCenter.Crashes;
using Shared;
using Xamarin.Forms;

namespace XamarinExplorer.ViewModels
{
	public class MenuPageViewModel : BaseViewModel
	{
		private bool _isRefreshingCategories;

		public ICollection<MenuItem> Menu { get; } = new ObservableCollection<MenuItem>();
		public IList<ProductCategory> Categories { get; } = new ObservableCollection<ProductCategory>();
		public IList<Product> Posts { get; } = new ObservableCollection<Product>();
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
				var data = await DependencyService.Get<IRepository<ProductCategory>>().GetAsync();

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
