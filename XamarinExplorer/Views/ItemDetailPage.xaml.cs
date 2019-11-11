using System.Collections.Generic;
using Microsoft.AppCenter.Analytics;
using Shared;

namespace XamarinExplorer.Views
{
	public partial class ItemDetailPage
	{
		public Item Model { get; }

		public ItemDetailPage(Item model)
		{
			InitializeComponent();

			BindingContext = new {
				Title = model?.Text,
				Item = model,
			};
			Model = model;
		}

		public ItemDetailPage()
		{
			InitializeComponent();

			Model = new Item
			{
				Text = "This is an item description."
			};

			BindingContext = new
			{
				Title = "Item 1",
				Item = Model
			};
		}

		protected override void OnAppearing()
		{
			base.OnAppearing();

			var item = Model;

			if (item == null)
			{
				return;
			}

			Analytics.TrackEvent(AnalyticEvents.ItemOpened, new Dictionary<string, string> {
				{ "Id", (item?.Id.ToString() ?? string.Empty) },
				{ "Text", item?.Text ?? string.Empty },
				{ "Type", item.GetType().Name }
			});
		}
	}
}