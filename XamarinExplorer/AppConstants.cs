using System;
using System.Collections.Generic;
using XamarinExplorer.Helpers;

namespace XamarinExplorer
{
	public static class AppConstants
	{
		public const string WebServiceUrl = "https://my-signalr-functions.azurewebsites.net/api/";
		public const string SignalRHub = "https://my-signalr-functions.azurewebsites.net/api/hubs/broadcast/";

		public static string AppCenterSecret
		{
			get
			{
				string startup = string.Empty;

				#if APP_CENTER

				if (Guid.TryParse(Secrets.AppCenter_iOS_Secret, out Guid iOSSecret))
				{
					startup += $"ios={iOSSecret};";
				}

				if (Guid.TryParse(Secrets.AppCenter_Android_Secret, out Guid AndroidSecret))
				{
					startup += $"android={AndroidSecret};";
				}
				#endif

				return startup;
			}
		}
	}

	public static class AnalyticEvents
	{
		public const string ItemOpened = "ItemOpened";
		public const string HandledException = "HandledException";

		public static Dictionary<string, string> FromExceptionArgs(Exception ex)
		{
			return new Dictionary<string, string> {
				{ "Exception", ex.GetType().Name },
				{ "InnerException", ex.InnerException?.Message },
				{ "Source", ex.Source }
			};
		}
	}
}
