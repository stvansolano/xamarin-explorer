using Shared;

namespace XamarinExplorer.Views
{
	public partial class ItemDetailPage
	{
		public ItemDetailPage(object model)
		{
			BindingContext = new
			{
				Title = (model is Item p ? p?.Text : string.Empty),
				Model = model
			};

			InitializeComponent();
		}

		public ItemDetailPage() :
			this(new object())
		{ }
	}
}