using Shared;
using Shared.WordPress;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;
using XamarinExplorer.Services;
using XamarinExplorer.ViewModels;

namespace XamarinExplorer.Views
{
	public partial class ItemsPage : ContentPage
	{
		ListViewModel<WP_Post> _viewModel;

		public ItemsPage()
		{
			InitializeComponent();

			On<Xamarin.Forms.PlatformConfiguration.iOS>().SetLargeTitleDisplay(LargeTitleDisplayMode.Always);
				
			var repository = DependencyService.Get<IRepository<WP_Post>>();
			BindingContext = _viewModel = new ListViewModel<WP_Post>(repository);

			_viewModel.FilterPredicate = item => MatchesFilter(item.content.rendered) || MatchesFilter(item.content.rendered);
			
			RefreshToolbar.Command = new Command(() => _viewModel.LoadItemsCommand.Execute(new object()));

			ItemsListView.ItemSelected += OnItemSelected;
		}

		private bool MatchesFilter(string text)
		{
			if (string.IsNullOrEmpty(_viewModel.Filter))
			{
				return true;
			}
			return (text ?? string.Empty).ToLowerInvariant().Contains(_viewModel.Filter.ToLowerInvariant());
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