using Xamarin.Forms;

namespace XamarinExplorer.Views
{
	public partial class HomePage
	{
		public HomePage()
		{
			InitializeComponent();

			var menuPage = new MenuPage();
			menuPage.Menu.Add(new MenuItem()
			{
				Icon = ""
			});

			Master = menuPage;
		}
	}
}
