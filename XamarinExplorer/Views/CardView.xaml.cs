using Xamarin.Forms;

namespace XamarinExplorer.Views
{
	public partial class CardView
	{
		public CardView()
		{
			InitializeComponent();

			if (Device.RuntimePlatform == Device.Android)
			{
				HasShadow = true;
				BorderColor = Color.DarkGray;
				Margin = new Thickness(5);
			}
		}
	}
}
