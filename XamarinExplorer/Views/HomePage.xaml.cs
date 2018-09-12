using System;
using System.Windows.Input;
using Xamarin.Forms;

namespace XamarinExplorer.Views
{
	public partial class HomePage
	{
		public HomePage()
		{
			InitializeComponent();

			var menuPage = new MenuPage();
			menuPage.Menu.Add(new MenuItem { Text = "Home", Command = GetNavigationCommand(() => new HomePage { Title = "Home" }) });
			menuPage.Menu.Add(new MenuItem { Text = "About", Command = GetNavigationCommand(() => new AboutPage { Title = "About" }) });

			Master = menuPage;

			RefreshToolbar.Command = new Command(() => { });
		}

		private ICommand GetNavigationCommand(Func<Page> pageFunc)
		{
			return new Command(() =>
			{
				var page = new TabsPage();

				page.Children.Add(pageFunc());
			    Detail = page;
				IsPresented = false;
			});
		}
	}
}
