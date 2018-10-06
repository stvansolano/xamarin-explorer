using System;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using Xamarin.Forms;

namespace XamarinExplorer.Views
{
	public partial class CameraPage : ContentPage
	{
		public CameraPage()
		{
			InitializeComponent();

			slider.Value = 360;
			captureButton.Command = new Command(() => TakePhoto());
		}

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

			if (cameraStatus != PermissionStatus.Granted || storageStatus == PermissionStatus.Granted)
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
				SaveToAlbum = true, Directory = "Xamarin",
				Name = $"Photo_{DateTime.Now.Millisecond}"
			});

			if (media == null)
			{
				return;
			}
			ShowImage(media);
		}

		private void ShowImage(MediaFile media)
		{
			if (media == null)
			{
				return;
			}
			mediaFile.Text = media.Path;
			image.Source = ImageSource.FromFile(media.Path);
			slider.IsVisible = true;
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
	}
}
