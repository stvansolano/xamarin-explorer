using Shared;

namespace XamarinExplorer.Views
{
	public partial class ItemDetailPage
	{
		public ItemDetailPage(object model)
		{
			BindingContext = new
			{
				Title = (model is Item p ? p?.Title : string.Empty),
				Model = model
			};

			InitializeComponent();
		}

		public ItemDetailPage() :
			this(new object())
		{ }
	}
}