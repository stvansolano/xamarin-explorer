using Moq;
using Moq.Protected;
using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using XamarinExplorer.Services;

namespace UnitTests
{
    public class MockHttpFactory : IHttpFactory
    {
		public HttpClient GetClient()
		{
			var handlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);
			handlerMock.Protected()

			   // Setup the PROTECTED method to mock
			   .Setup<Task<HttpResponseMessage>>(
				  "SendAsync",
				  ItExpr.IsAny<HttpRequestMessage>(),
				  ItExpr.IsAny<CancellationToken>()
			   )
			   // prepare the expected response of the mocked http call
			   .ReturnsAsync(new HttpResponseMessage()
			   {
				   StatusCode = HttpStatusCode.OK,
				   Content = new StringContent("[{'id':1,'text':'testing'}]"),
			   })
			   .Verifiable();

			// use real http client with mocked handler here
			var httpClient = new HttpClient(handlerMock.Object)
			{
				BaseAddress = new Uri("http://test.com/"),
			};

			return httpClient;
		}
    }
}