using System.Collections.Generic;
using Microsoft.AppCenter.Analytics;
using Shared;
using Shared.WordPress;

namespace XamarinExplorer.Views
{
	public partial class ItemDetailPage
	{
		public ItemDetailPage(object model)
		{
			BindingContext = new
			{
				Title = (model is WP_Post p ? p?.Title?.Rendered : string.Empty),
				Model = model
			};

			InitializeComponent();
		}

		public ItemDetailPage() :
			this(new object())
		{ }
	}
}