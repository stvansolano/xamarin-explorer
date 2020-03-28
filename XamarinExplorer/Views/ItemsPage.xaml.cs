using Shared;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;
using XamarinExplorer.ViewModels;

namespace XamarinExplorer.Views
{
	public partial class ItemsPage : ContentPage
	{
		ToDoListViewModel _viewModel;

		public bool IsReady { get; private set; }

		public ItemsPage()
		{
			BindingContext = _viewModel = new ToDoListViewModel();

			InitializeComponent();

			On<Xamarin.Forms.PlatformConfiguration.iOS>().SetLargeTitleDisplay(LargeTitleDisplayMode.Always);
							
			RefreshToolbar.Command = new Command(() => _viewModel.LoadItemsCommand.Execute(new object()));

			ItemsListView.ItemSelected += OnItemSelected;

			AddButton.Clicked += (s, e) => ToDoEntry.Text = string.Empty;
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

		protected override async void OnAppearing()
		{
			IsReady = false;
			base.OnAppearing();

			if (_viewModel.Items.Count == 0)
				_viewModel.LoadItemsCommand.Execute(new object());

			if (!_viewModel.IsHubConnected)
			{
				await _viewModel.ConnectAsync();
			}
			IsReady = true;
		}

		void CheckBox_CheckedChanged(System.Object sender, Xamarin.Forms.CheckedChangedEventArgs e)
		{
			if (sender is CheckBox checkBox && IsReady)
			{
				var context = checkBox.BindingContext as ItemViewModel;
				context?.UpdateCommand?.Execute(context);
			}
		}
	}
}