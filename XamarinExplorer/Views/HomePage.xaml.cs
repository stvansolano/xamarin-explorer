using System;
using System.Windows.Input;
using Xamarin.Forms;

namespace XamarinExplorer.Views
{
	public partial class HomePage
	{
		private MenuPage MenuScreen { get; } = new MenuPage();

		public HomePage()
		{
			InitializeComponent();

			MenuScreen.Menu.Add(GetMenuItem("Home", new HomePage()));
			MenuScreen.Menu.Add(GetMenuItem("About", new AboutPage()));

			Master = MenuScreen;

			RefreshToolbar.Command = new Command(() => { });
		}

		private MenuItem GetMenuItem(string title, Page page)
		{
			page.Title = title;

			return new MenuItem { Text = title, Command = GetNavigationCommand(() => page) };
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
