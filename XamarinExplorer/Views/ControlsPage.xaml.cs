using System;
using System.Windows.Input;
using Xamarin.Forms;

namespace XamarinExplorer.Views
{
	public partial class ControlsPage : ContentPage
	{
		public ControlsPage()
		{
			MessageCommand = new Command(async() => await DisplayAlert("App says:", "Hello world!", "Close"));
			InitializeComponent();
		}

		public ICommand MessageCommand { get; set; }
	}
}
