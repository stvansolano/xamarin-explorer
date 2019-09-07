using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Refit;
using Shared.WordPress;
using Xamarin.Forms;

namespace XamarinExplorer.Services
{
    public static class WordPressApi
    {
        const string URL_BASE = "http://checkmywordpress.azurewebsites.net";

        private static Lazy<IWordPressApi> _instance = new Lazy<IWordPressApi>(
            () => RestService.For<IWordPressApi>(URL_BASE)
        );

        public static IWordPressApi Instance => _instance.Value;
    }

    public interface IWordPressApi
    {
        [Get("/wp-json/wp/v2/posts")]
        Task<WP_Post[]> GetPosts();
    }
}
