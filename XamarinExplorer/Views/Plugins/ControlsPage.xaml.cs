using System;
using System.Windows.Input;
using Xamarin.Forms;

namespace XamarinExplorer.Views
{
	public partial class ControlsPage : ContentPage
	{
		public ControlsPage()
		{
			MessageCommand = new Command(() => OnShowMessage());
			InitializeComponent();
		}

		private async void OnShowMessage()
		{
			await DisplayAlert("App says:", "Hello world!", "Close");
		}

		public ICommand MessageCommand { get; set; }
	}
}
