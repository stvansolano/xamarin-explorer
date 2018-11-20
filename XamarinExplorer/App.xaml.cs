using System;
using Xamarin.Forms;
using XamarinExplorer.Services;
using XamarinExplorer.Views;
using Xamarin.Forms.Xaml;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace XamarinExplorer
{
	public partial class App : Application
	{
		public static string WebServiceUrl = "";
		public static bool UseMockDataStore = true;

		public App()
		{
#if APP_CENTER
            AppCenter.Start(AppConstants.AppCenterSecret,
				   typeof(Analytics), typeof(Crashes));
#endif
            InitializeComponent();

			if (UseMockDataStore)
				DependencyService.Register<IRepository<Models.Item>, MockDataStore>();
			else
				DependencyService.Register<IRepository<Models.Item>, Repository<Models.Item>>();

			//MainPage = new CognitiveServices();
			MainPage = new NavigationPage(new TabsPage());
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
