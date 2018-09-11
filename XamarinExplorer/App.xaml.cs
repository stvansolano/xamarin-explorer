using System;
using Xamarin.Forms;
using XamarinExplorer.Services;
using XamarinExplorer.Views;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace XamarinExplorer
{
	public partial class App : Application
	{
		public static string WebServiceUrl = "http://cloud-services.azurewebsites.net/api/products";
		public static bool UseMockDataStore = true;

		public App()
		{
			InitializeComponent();

			if (UseMockDataStore)
				DependencyService.Register<MockDataStore>();
			else
				DependencyService.Register<Repository<Models.Item>>();

			MainPage = new HomePage();
		}

		protected override void OnStart()
		{
			// Handle when your app starts
		}

		protected override void OnSleep()
		{
			// Handle when your app sleeps
		}

		protected override void OnResume()
		{
			// Handle when your app resumes
		}
	}
}
