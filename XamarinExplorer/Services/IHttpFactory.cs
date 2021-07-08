using System;
using System.Net.Http;

namespace XamarinExplorer.Services
{
    public interface IHttpFactory
    {
        HttpClient GetClient();
    }

    public class HttpFactory : IHttpFactory
    {
        public virtual HttpClient GetClient()
        {
            var client = new HttpClient();
            if (!string.IsNullOrEmpty(AppConstants.WebServiceUrl))
            {
                client.BaseAddress = new Uri($"{AppConstants.WebServiceUrl}");
            }

            return client;
        }
    }
}