using Xamarin.Forms;
using XamarinExplorer.Services;
using XamarinExplorer.Views;
using Xamarin.Forms.Xaml;
using Shared;

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

			if (UseMockDataStore)
				DependencyService.Register<IRepository<Item>, MockDataStore>();
			else
				DependencyService.Register<IRepository<Item>, ToDoItemsRepository>();

			MainPage = new NavigationPage(new ItemsPage() { Title = "My To-Dos" });
		}
	}
}
