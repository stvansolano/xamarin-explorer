using System;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Xamarin.Forms;

namespace XamarinExplorer.Views
{
	public partial class CameraPage : ContentPage
	{
		public CameraPage()
		{
			InitializeComponent();

			captureButton.Command = new Command(() => TakePhoto());
		}

        private async void TakePhoto()
		{
			if (!CrossMedia.Current.IsTakePhotoSupported
				|| !CrossMedia.Current.IsCameraAvailable)
			{
				PickImageInstead();
				return;
			}
			var media = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions
			{
				SaveToAlbum = true, Directory = "Xamarin",
				Name = $"Photo_{DateTime.Now.Millisecond}"
			});

			if (media == null)
			{
				return;
			}
			mediaFile.Text = media.Path;
			image.Source = ImageSource.FromFile(media.Path);
		}

		private async void PickImageInstead()
		{
			if (!CrossMedia.Current.IsPickPhotoSupported)
			{
				PickImageInstead();
				return;
			}

			var media = await CrossMedia.Current.PickPhotoAsync(new PickMediaOptions
			{
				CompressionQuality = 100,
				PhotoSize = PhotoSize.Medium
			});

			if (media == null)
			{
				return;
			}
			mediaFile.Text = media.Path;
			image.Source = ImageSource.FromFile(media.Path);
		}
	}
}
