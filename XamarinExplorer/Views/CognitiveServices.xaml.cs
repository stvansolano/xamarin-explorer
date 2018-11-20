using System;
using Plugin.Permissions;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Xamarin.Forms;
using Plugin.Permissions.Abstractions;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using System.Collections.ObjectModel;

namespace XamarinExplorer.Views
{
	public partial class CognitiveServices
	{
		public CognitiveServices()
		{
			InitializeComponent();

			//slider.Value = 360;
			captureButton.Command = new Command(() => AnalyzePhoto());
		}

		private async void AnalyzePhoto()
		{
			if (string.IsNullOrEmpty(EntryUrl.Text))
			{
				return;
			}
			image.Source = FileImageSource.FromUri(new Uri(EntryUrl.Text));
			//MediaStream = await GetBytes(media.GetStream());

			//slider.IsVisible = true;

			await FormsUtils.RunAsBusyAsync(this, () => {

				AnalyzeMedia();

			}).ConfigureAwait(false);
		
		}

		#region Private Methods

		private async void TakePhoto()
		{
			var cameraStatus = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Camera);
			var storageStatus = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Storage);

			if (cameraStatus != PermissionStatus.Granted || storageStatus != PermissionStatus.Granted)
			{
				var results = await CrossPermissions.Current.RequestPermissionsAsync(new[] { Permission.Camera, Permission.Storage });
				cameraStatus = results[Permission.Camera];
				storageStatus = results[Permission.Storage];
			}

			if (cameraStatus != PermissionStatus.Granted || storageStatus != PermissionStatus.Granted)
			{
				await DisplayAlert("Permissions Denied", "Unable to take photos.", "OK");
				//On iOS you may want to send your user to the settings screen.
				//CrossPermissions.Current.OpenAppSettings();
				return;
			}


			if (!CrossMedia.Current.IsTakePhotoSupported
				|| !CrossMedia.Current.IsCameraAvailable)
			{
				PickImageInstead();
				return;
			}
			var media = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions
			{
				SaveToAlbum = true,
				Directory = "Xamarin",
				Name = $"Photo_{DateTime.Now.Millisecond}"
			});

			if (media == null)
			{
				return;
			}
			ShowImage(media);
		}

		private async void ShowImage(MediaFile media)
		{
			if (media == null)
			{
				return;
			}
			image.Source = FileImageSource.FromFile(media.Path);
			MediaStream = await GetBytes(media.GetStream());

			//slider.IsVisible = true;

			await FormsUtils.RunAsBusyAsync(this, () => {

				AnalyzeMedia();
				AnalyzeText();

			}).ConfigureAwait(false);
		}

		private async void PickImageInstead()
		{
			if (!CrossMedia.Current.IsPickPhotoSupported)
			{
				return;
			}

			var media = await CrossMedia.Current.PickPhotoAsync(new PickMediaOptions
			{
				CompressionQuality = 100,
				PhotoSize = PhotoSize.Medium
			});
			ShowImage(media);
		}

		private async Task<byte[]> GetBytes(Stream stream)
		{
			var bytes = new byte[stream.Length];
			await stream.ReadAsync(bytes, 0, (int)stream.Length);

			return bytes;
		}

		private async void AnalyzeText()
		{
			if (MediaStream == null)
			{
				return;
			}
			var result = await CognitiveService.RecognizeTextAsync(MediaStream);

			if (result == null)
			{
				return;
			}
			StringBuilder builder = new StringBuilder();
			var allLines = result.Regions.SelectMany(region => region.Lines).ToList();

			foreach (var line in allLines)
			{
				var lineWords = line.Words.ToList();

				foreach (var word in lineWords)
				{
					builder.Append($"{word.Text}");

					if (lineWords.IndexOf(word) + 1 < lineWords.Count)
					{
						builder.Append(" ");
					}
				}

				if (allLines.IndexOf(line) + 1 < allLines.Count)
				{
					builder.Append(" ");
				}
			}
			RecognizedText = builder.ToString();
		}

		private async void AnalyzeMedia()
		{
			//if (MediaStream == null)
			//{//
			//	return;
			//}
			Description = "Analyzing...";
			TagsPanel.IsVisible = true;

			var result = await CognitiveService.AnalyzeImageUrl(EntryUrl.Text).ConfigureAwait(false);

			if (result == null)
			{
				return;
			}

			var caption = (from match in result.Description.Captions
						   where match.Confidence.Equals(result.Description.Captions.Max(item => item.Confidence))
						   select match).FirstOrDefault();

			Confidence = caption.Confidence * 100;
			Description = caption.Text;
			VerbosedDescription = $" {caption.Text}({(caption.Confidence * 100).ToString("F2")}%)";

			Device.BeginInvokeOnMainThread(() => { 
				if (result.Tags != null && result.Tags.Any())
				{
					Tags.Clear();

					foreach (var tag in result.Tags)
					{
						Tags.Add(tag);
					}
				}
			});

			//ImageSource = MediaStream;

			PrimaryColor = FormsUtils.FromColor(result.Color.DominantColorBackground);
			AccentColor = FormsUtils.FromColor(result.Color.AccentColor);
			SecondaryColor = FormsUtils.FromColor(result.Color.DominantColorForeground);
		}

		#endregion

		#region Properties

		protected byte[] MediaStream { get; set; }
		protected CognitiveService CognitiveService { get; set; } = new CognitiveService();

		public ObservableCollection<Microsoft.ProjectOxford.Vision.Contract.Tag> Tags { get; } = new ObservableCollection<Microsoft.ProjectOxford.Vision.Contract.Tag>();

		private string _description;
		public string Description
		{
			get { return _description; }
			set
			{
				_description = value;
				OnPropertyChanged(nameof(Description));
			}
		}

		private string _verbosedDescription;
		public string VerbosedDescription
		{
			get { return _verbosedDescription; }
			set
			{
				_verbosedDescription = value;
				OnPropertyChanged(nameof(VerbosedDescription));
			}
		}

		private Color _accentColor;
		public Color AccentColor
		{
			get { return _accentColor; }
			set
			{
				_accentColor = value;
				OnPropertyChanged(nameof(AccentColor));
			}
		}

		private Color _primaryColor;
		public Color PrimaryColor
		{
			get { return _primaryColor; }
			set
			{
				_primaryColor = value;
				OnPropertyChanged(nameof(PrimaryColor));
			}
		}

		private Color _secondaryColor;
		public Color SecondaryColor
		{
			get { return _secondaryColor; }
			set
			{
				_secondaryColor = value;
				OnPropertyChanged(nameof(SecondaryColor));
			}
		}

		private byte[] _imageSource;
		public byte[] ImageSource
		{
			get { return _imageSource; }
			set
			{
				_imageSource = value;
				OnPropertyChanged(nameof(ImageSource));
			}
		}

		private double _confidence;
		public double Confidence
		{
			get { return _confidence; }
			set
			{
				_confidence = value;
				OnPropertyChanged(nameof(Confidence));
			}
		}

		private string _recognizedText;
		public string RecognizedText
		{
			get { return _recognizedText; }
			set
			{
				_recognizedText = value;
				OnPropertyChanged(nameof(RecognizedText));
			}
		}
	
		#endregion
	}
}
