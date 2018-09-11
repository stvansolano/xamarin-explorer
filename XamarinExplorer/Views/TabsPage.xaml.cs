using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms.PlatformConfiguration.AndroidSpecific;

namespace XamarinExplorer.Views
{
	public partial class TabsPage
	{
		public TabsPage()
		{
			InitializeComponent();

			On<Android>().SetToolbarPlacement(ToolbarPlacement.Bottom);
		}
	}
}