using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Xamarin.Forms;
using XamarinExplorer.Services;
using XamarinExplorer.ViewModels;

namespace XamarinExplorer.Views
{
	public partial class MenuPage : ContentPage
	{
		public MenuPageViewModel ViewModel { get; set; } = new MenuPageViewModel();

		public MenuPage()
		{
			InitializeComponent();

			BindingContext = ViewModel;
		}

		protected override async void OnAppearing()
		{
			base.OnAppearing();

			await ViewModel.LoadCategoriesAsync();
		}
	}
}
