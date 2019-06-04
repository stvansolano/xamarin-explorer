using Xamarin.Forms;
using XamarinExplorer.Services;
using XamarinExplorer.ViewModels;
using Xunit;
using Shared;

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
        public async void ListTests()
        {
			Init();

			DependencyService.Register<IHttpFactory, MockHttpFactory>();

            var viewModel = new ListViewModel<Item>(new Repository<Item>());

            await viewModel.LoadItemsAsync();

            Assert.NotNull(viewModel.Items);
            Assert.NotEmpty(viewModel.Items);
        }
    }
}
