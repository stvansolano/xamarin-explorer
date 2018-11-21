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
			if (DateTime.Now.Minute % 2 == 0)
				throw new Exception("Oops...");

			await DisplayAlert("App says:", "Hello world!", "Close");
		}

		public ICommand MessageCommand { get; set; }
	}
}
