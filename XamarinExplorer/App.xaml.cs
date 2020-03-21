using Xamarin.Forms;
using XamarinExplorer.Services;
using XamarinExplorer.Views;
using Xamarin.Forms.Xaml;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using Shared;
using Shared.AdventureWorks;
using AdventureWorks.SqlServer.Models;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace XamarinExplorer
{
	public partial class App : Application
	{
		public static bool UseMockDataStore = string.IsNullOrEmpty(AppConstants.WebServiceUrl);

		public App()
		{
#if APP_CENTER
            AppCenter.Start(AppConstants.AppCenterSecret,
				   typeof(Analytics), typeof(Crashes));
#endif
            InitializeComponent();

			DependencyService.Register<IHttpFactory, HttpFactory>();
			DependencyService.Register<IRepository<Product>, ProductRepository>();
			DependencyService.Register<IRepository<ProductCategory>, CategoryRepository>();

			if (UseMockDataStore)
				DependencyService.Register<IRepository<Item>, MockDataStore>();
			else
				DependencyService.Register<IRepository<Item>, ToDoItemsRepository>();

			MainPage = new NavigationPage(new ItemsPage());
			//MainPage = new NavigationPage(new TabsPage());
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
