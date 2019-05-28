using Moq;
using Moq.Protected;
using System;
using XamarinExplorer.Models;
using XamarinExplorer.Services;
using XamarinExplorer.ViewModels;
using Xunit;

namespace UnitTests
{
    public class ViewModelTests
    {
        [Fact]
        public async void ListTests()
        {
            var viewModel = new ListViewModel<Item>(new Repository<Item>(new MockHttpFactory()));

            await viewModel.LoadItemsAsync();

            Assert.NotNull(viewModel.Items);
            Assert.NotEmpty(viewModel.Items);


        }
    }
}
