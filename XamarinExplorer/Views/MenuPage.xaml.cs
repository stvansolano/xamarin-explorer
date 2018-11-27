using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Xamarin.Forms;

namespace XamarinExplorer.Views
{
	public partial class MenuPage : ContentPage
	{
		public MenuPage()
		{
			InitializeComponent();

			BindingContext = this;
		}

		public ICollection<MenuItem> Menu { get; } = new ObservableCollection<MenuItem>();
	}
}
