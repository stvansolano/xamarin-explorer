using System;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms.PlatformConfiguration.AndroidSpecific;

namespace XamarinExplorer.Views
{
	public partial class TabsPage
	{
		public TabsPage()
		{
			InitializeComponent();

			var aboutCommand = GetNavigationCommand(() => new AboutPage { Title = "About" });
			AboutToolbarItem.Command = aboutCommand;

			On<Android>().SetToolbarPlacement(ToolbarPlacement.Bottom);

			MenuScreen.Menu.Add(GetMenuItem("Controls", new ControlsPage()));
			MenuScreen.Menu.Add(GetMenuItem("Lists", new ItemsPage()));
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
				var page = pageFunc();

				Navigation.PushAsync(page);
			});
		}
	}
}