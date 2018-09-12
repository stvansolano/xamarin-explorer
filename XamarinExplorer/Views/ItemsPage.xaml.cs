using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XamarinExplorer.Models;
using XamarinExplorer.ViewModels;

namespace XamarinExplorer.Views
{
	public partial class ItemsPage : ContentPage
	{
		ItemsViewModel viewModel;

		public ItemsPage()
		{
			InitializeComponent();

			RefreshToolbar.Command = new Command(() => viewModel.LoadItemsCommand.Execute(new object()));

			BindingContext = viewModel = new ItemsViewModel();
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

			if (viewModel.Items.Count == 0)
				viewModel.LoadItemsCommand.Execute(new object());
		}
	}
}