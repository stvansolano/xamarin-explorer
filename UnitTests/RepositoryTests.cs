using Xamarin.Forms;
using XamarinExplorer.Services;
using XamarinExplorer.ViewModels;
using Xunit;
using Shared;
using XamarinExplorer;

namespace UnitTests
{
	public class ViewModelTests
    {
		private static void Init()
		{
			Device.Info = new MockDeviceInfo();
			Device.PlatformServices = new MockPlatformServices();
			DependencyService.Register<MockResourcesProvider>();
			DependencyService.Register<MockDeserializer>();
		}

		[Fact]
        public void ListTests()
        {
			Init();

			DependencyService.Register<IHttpFactory, MockHttpFactory>();
			DependencyService.Register<ToDoItemsRepository>();

            var viewModel = new ToDoListViewModel();

            viewModel.LoadItemsCommand.Execute(new object());

            Assert.NotNull(viewModel.Items);
            Assert.NotEmpty(viewModel.Items);
        }
    }
}
