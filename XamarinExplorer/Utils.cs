using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace XamarinExplorer
{
	public static class FormsUtils
	{
		public static Color FromColor(string codeOrName)
		{
			Xamarin.Forms.Color? color = null;

			try
			{
				switch (codeOrName.ToLowerInvariant())
				{
					case "black": color = Color.Black; break;
					case "green": color = Color.Green; break;
					case "purple": color = Color.Purple; break;
					case "blue": color = Color.Blue; break;
					case "red": color = Color.Red; break;
					default:
						color = Color.FromHex("#" + codeOrName);
						break;
				}
			}
			catch (Exception ex)
			{
				color = Xamarin.Forms.Color.Transparent;
				Debug.WriteLine(ex);
			}
			return color.Value;
		}

		public static async void RunAsBusy(Page page, Action action)
		{
			page.IsBusy = true;

			try
			{
				Device.BeginInvokeOnMainThread(action);
			}
			catch (Exception ex)
			{
				await page.DisplayAlert("Error", ex.Message, "Close");
			}
			finally
			{
				page.IsBusy = false;
			}
		}

		public static Task RunAsBusyAsync(Page page, Action action)
		{
			return Task.Factory.StartNew(() => RunAsBusy(page, action));
		}
	}
}