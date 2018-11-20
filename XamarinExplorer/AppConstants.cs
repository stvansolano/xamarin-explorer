﻿using System;
using XamarinExplorer.Helpers;

namespace XamarinExplorer
{
	public static class AppConstants
	{
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


		public static string CognitiveApiKey
		{
			get
			{
				return Secrets.CognitiveApiKey;
			}
		}

		public static string CognitiveUrlEndpoint 
		{
			get 
			{
				return Secrets.CognitiveUrlEndpoint;
			}
		}
	}
}
