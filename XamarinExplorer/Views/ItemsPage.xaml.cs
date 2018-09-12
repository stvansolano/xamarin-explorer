using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XamarinExplorer.Models;
using XamarinExplorer.Services;
using XamarinExplorer.ViewModels;

namespace XamarinExplorer.Views
{
	public partial class ItemsPage : ContentPage
	{
		ListViewModel<Item> _viewModel;

		public ItemsPage()
		{
			InitializeComponent();

			RefreshToolbar.Command = new Command(() => _viewModel.LoadItemsCommand.Execute(new object()));

			var repository = DependencyService.Get<IRepository<Item>>() ?? new MockDataStore();
			BindingContext = _viewModel = new ListViewModel<Item>(repository);

			ItemsListView.ItemSelected += OnItemSelected;
		}

		async void OnItemSelected(object sender, SelectedItemChangedEventArgs args)
		{
			var item = args.SelectedItem as Item;
			if (item == null)
				return;
			
			await Navigation.PushAsync(new ItemDetailPage(item));

			// Manually deselect item.
			ItemsListView.SelectedItem = null;
		}

		protected override void OnAppearing()
		{
			base.OnAppearing();

			if (_viewModel.Items.Count == 0)
				_viewModel.LoadItemsCommand.Execute(new object());
		}
	}
}