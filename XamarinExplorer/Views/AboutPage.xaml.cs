using System;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace XamarinExplorer.Views
{
	public partial class AboutPage : ContentPage
	{
		public AboutPage()
		{
			InitializeComponent();

			Title = "About";
			OpenWebCommand = new Command(() => Device.OpenUri(new Uri("https://xamarin.com/platform")));
			OpenGitHubCommand = new Command(() => Device.OpenUri(new Uri("https://github.com/stvansolano/xamarin-explorer")));
		}

		public ICommand OpenWebCommand { get; }
		public ICommand OpenGitHubCommand { get; }
	}
}