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

			MenuScreen.Menu.Add(new MenuItem { Text = "Controls", Command = GetNavigationCommand(() => new ControlsPage { Title = "Controls" }) });
			MenuScreen.Menu.Add(new MenuItem { Text = "Lists", Command = GetNavigationCommand(() => new ItemsPage { Title = "Lists" }) });
			MenuScreen.Menu.Add(new MenuItem { Text = "About", Command = aboutCommand });
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