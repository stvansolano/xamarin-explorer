using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
//using Xamarin.Essentials;
using Xamarin.Forms;

namespace XamarinExplorer.Views
{
	public partial class CompassPage : ContentPage
	{
		public CompassPage()
		{
			StartCommand = new Command(Start);
			StopCommand = new Command(Stop);

			InitializeComponent();
		}

		string headingDisplay;
		public string HeadingDisplay
		{
			get => headingDisplay;
			set => SetProperty(ref headingDisplay, value);
		}

		double heading = 0;
		public double Heading
		{
			get => heading;
			set => SetProperty(ref heading, value);
		}

		public Command StopCommand { get; }
		public Command StartCommand { get; }

		/*void Gyroscope_ReadingChanged(object sender, GyroscopeChangedEventArgs e)
		{
		}*/

		void Stop()
		{
			/*if (!Compass.IsMonitoring)
				return;

			Compass.ReadingChanged -= Compass_ReadingChanged;
			Compass.Stop();*/
		}


		void Start()
		{
			/*try
			{
				if (Compass.IsMonitoring)
					return;

				Compass.ReadingChanged += Compass_ReadingChanged;

				Compass.Start(SensorSpeed.Normal);
			}
			catch (FeatureNotSupportedException ex)
			{
				Console.WriteLine(ex);
				DisplayAlert("Compass not available", "Compass", "Close");
			}*/
		}

		//void Compass_ReadingChanged(object sender, CompassChangedEventArgs e)
		//{
		//	Heading = e.Reading.HeadingMagneticNorth;
		//	HeadingDisplay = $"Heading: {Heading.ToString()}";
		//}

		protected override void OnAppearing()
		{
			base.OnAppearing();

			if (!DesignMode.IsDesignModeEnabled)
				Start();
		}

		protected override void OnDisappearing()
		{
			base.OnDisappearing();

			if (!DesignMode.IsDesignModeEnabled)
				Stop();
		}

		private void SetProperty<T>(ref T refValue, T value, 
		                            [CallerMemberName]
									string property = "")
		{
			refValue = value;
			OnPropertyChanged(property);
		}
	}
}
