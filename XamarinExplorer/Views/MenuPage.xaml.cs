using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace XamarinExplorer.Views
{
	public partial class MenuPage : ContentPage
	{
		public MenuPage()
		{
			InitializeComponent();

		}

		public List<MenuItem> Menu { get; } = new List<MenuItem>();
	}
}
