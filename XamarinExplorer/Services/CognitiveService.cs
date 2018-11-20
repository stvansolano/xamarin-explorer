using Newtonsoft.Json;
using Plugin.Connectivity;
using System;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.IO;
using System.Linq;
using Microsoft.ProjectOxford.Vision;
using Microsoft.ProjectOxford.Vision.Contract;

namespace XamarinExplorer
{
	// stvansolano@outlook.com
	// http://stvansolano.github.io/blog/

	public class CognitiveService
	{
		public string VISION_URL = AppConstants.CognitiveUrlEndpoint;

		public string API_KEY = AppConstants.CognitiveApiKey;

		public async Task<AnalysisResult> AnalyzeImageUrl(string imageUrl)
		{
			AnalysisResult result = null;

			try
			{
				if (CrossConnectivity.Current.IsConnected == false)
				{
					return null;
				}
				string[] features = { "Tags", "Categories", "Description", "Color" };

				var url = "/vision/v1.0/analyze?visualFeatures=" + string.Join("&", features) + "&language=en";

				using (var newClient = GetClient())
				{
					var json = JsonConvert.SerializeObject(new { url = imageUrl });
					var response = await newClient.PostAsync(url, new StringContent(json, System.Text.Encoding.UTF8, "application/json"));
				
					if (response.IsSuccessStatusCode)
					{
						var responseJson = await response.Content.ReadAsStringAsync();

						Debug.WriteLine(responseJson);
						return JsonConvert.DeserializeObject<AnalysisResult>(responseJson);
					}
				}
			}
			catch (Exception ex)
			{
				Debug.WriteLine(ex.Message);
			}
			return result;
		}

		/*
		public async Task<AnalysisResult> AnalyzeImage(byte[] mediaStream)
		{
			AnalysisResult result = null;

			try
			{
				if (CrossConnectivity.Current.IsConnected == false)
				{
					return null;
				}
				string[] features = { "Tags", "Categories", "Description" "Color" };
				var visionClient = new VisionServiceClient(API_KEY, VISION_URL);

				var url = "/vision/v1.0/analyze?visualFeatures=" + features.Join("&") + "&language=en";

				using (var newClient = GetClient())
				{
					newClient.DefaultRequestHeaders.TransferEncodingChunked = true;
					var RequestContent = new MultipartFormDataContent();
					for (int i = 0; i < Request.FormDataAsByteArray.Count; ++i)
					{
						StreamContent scontent = new StreamContent(new MemoryStream(Request.FormDataAsByteArray.Values.ElementAt(i)));
						scontent.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
						{
							FileName = Request.FormDataAsByteArray.Keys.ElementAt(i) + ".jpg",
							Name = Request.FormDataAsByteArray.Keys.ElementAt(i)
						};
						scontent.Headers.ContentType = new MediaTypeHeaderValue("image/jpeg");
						RequestContent.Add(scontent);
					}
					postRequest.Content = RequestContent;
					var response = await newClient.SendAsync(postRequest);
					HandleResponse(response, AuthBehavior.OnlySend, out ResponseString);
					return JsonConvert.DeserializeObject<CSResponse>(ResponseString);
				}
			}
				var result = await GetClient(VISION_URL)

				Debug.WriteLine(result.ToString());
			}
			catch (Exception ex)
			{
				Debug.WriteLine(ex.Message);
			}
			return result;
		}*/

		private HttpClient GetClient()
		{
			var client = new HttpClient();
			client.BaseAddress = new Uri(VISION_URL);
			client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", API_KEY);

			return client;
		}

		public async Task<Microsoft.ProjectOxford.Vision.Contract.OcrResults> RecognizeTextAsync(byte[] mediaStream)
		{
			Microsoft.ProjectOxford.Vision.Contract.OcrResults result = null;

			try
			{
				if (CrossConnectivity.Current.IsConnected == false)
				{
					return null;
				}
				VisualFeature[] features = { VisualFeature.Tags, VisualFeature.Categories, VisualFeature.Description, VisualFeature.Color };

				var visionClient = new VisionServiceClient(API_KEY, VISION_URL);

				result = await visionClient.RecognizeTextAsync(new MemoryStream(mediaStream));

				Debug.WriteLine(result.ToString());
			}
			catch (Exception ex)
			{
				Debug.WriteLine(ex.Message);
			}
			return result;
		}
	}
}