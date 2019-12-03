using Xamarin.Forms;
using XamarinExplorer.Services;
using XamarinExplorer.Views;
using Xamarin.Forms.Xaml;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using Shared;
using WordPress = Shared.WordPress;
using XamarinExplorer.WordPress;
using Shared.WordPress;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace XamarinExplorer
{
	public partial class App : Application
	{
		// HTTPS
		public const string WebServiceUrl = "https://checkmywordpress.azurewebsites.net";

		public static bool UseMockDataStore = string.IsNullOrEmpty(WebServiceUrl);

		public App()
		{
#if APP_CENTER
            AppCenter.Start(AppConstants.AppCenterSecret,
				   typeof(Analytics), typeof(Crashes));
#endif
            InitializeComponent();

			DependencyService.Register<IHttpFactory, HttpFactory>();
			DependencyService.Register<IRepository<WP_Post>, PostRepository>();
			DependencyService.Register<IRepository<WP_Category>, CategoryRepository>();

			if (UseMockDataStore)
				DependencyService.Register<IRepository<Item>, MockDataStore>();
			else
				DependencyService.Register<IRepository<Item>, Repository<Item>>();

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
