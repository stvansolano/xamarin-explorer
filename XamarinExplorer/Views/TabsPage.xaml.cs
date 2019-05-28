using System;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms.PlatformConfiguration.AndroidSpecific;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;

namespace XamarinExplorer.Views
{
	public partial class TabsPage
	{
		public TabsPage()
		{
			InitializeComponent();

			var aboutCommand = GetNavigationCommand(() => new AboutPage { Title = "About" });
			AboutToolbarItem.Command = aboutCommand;

			//On<Android>().SetToolbarPlacement(ToolbarPlacement.Bottom);
			//On<Xamarin.Forms.PlatformConfiguration.iOS>().set(true);

            MenuScreen.Menu.Add(GetMenuItem("Controls", new ControlsPage()));
            MenuScreen.Menu.Add(GetMenuItem("Lists", new ItemsPage()));
        }

        private MenuItem GetMenuItem(string title, Xamarin.Forms.Page page)
		{
			page.Title = title;

			return new MenuItem { Text = title, Command = GetNavigationCommand(() => page) };
		}

		private ICommand GetNavigationCommand(Func<Xamarin.Forms.Page> pageFunc)
		{
			return new Command(() =>
			{
				var page = pageFunc();

				Navigation.PushAsync(page);
			});
		}
	}
}