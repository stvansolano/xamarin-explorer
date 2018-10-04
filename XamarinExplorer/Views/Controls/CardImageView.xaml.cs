using System;
using System.Collections.Generic;
using System.Windows.Input;
using Xamarin.Forms;

namespace XamarinExplorer.Views
{
	public partial class CardImageView
	{
		public CardImageView()
		{
			InitializeComponent();
		}

		#region Properties

		public string Title
		{
			get { return (string)GetValue(TitleProperty); }
			set { SetValue(TitleProperty, value); }
		}

		public ImageSource Image
		{
			get { return (ImageSource)GetValue(ImageProperty); }
			set { SetValue(ImageProperty, value); }
		}

		public ICommand Command
		{
			get { return (ICommand)GetValue(CommandProperty); }
			set { SetValue(CommandProperty, value); }
		}

		public object CommandParameter
		{
			get { return (object)GetValue(CommandParameterProperty); }
			set { SetValue(CommandParameterProperty, value); }
		}

		public TextAlignment HorizontalTextAlignment
		{
			get { return (TextAlignment)GetValue(HorizontalTextAlignmentProperty); }
			set { SetValue(HorizontalTextAlignmentProperty, value); }
		}

		public static readonly BindableProperty TitleProperty =
			BindableProperty.Create(nameof(Title), typeof(string), typeof(CardImageView), null,
			                        BindingMode.OneWay, null, (s, oldValue, newValue) => OnPropertyChanged(nameof(Title), s, oldValue, newValue));
		
		public static readonly BindableProperty ImageProperty =
			BindableProperty.Create(nameof(Image), typeof(ImageSource), typeof(CardImageView), null,
			                        BindingMode.OneWay, null, (s, oldValue, newValue) => OnPropertyChanged(nameof(Image), s, oldValue, newValue));

		public static readonly BindableProperty CommandProperty =
			BindableProperty.Create(nameof(Command), typeof(ICommand), typeof(CardImageView), null,
			                        BindingMode.OneWay, null, (s, oldValue, newValue) => OnPropertyChanged(nameof(Command), s, oldValue, newValue));

		public static readonly BindableProperty CommandParameterProperty =
			BindableProperty.Create(nameof(CommandParameter), typeof(object), typeof(CardImageView), null,
			                        BindingMode.OneWay, null, (s, oldValue, newValue) => OnPropertyChanged(nameof(CommandParameter), s, oldValue, newValue));

		public static readonly BindableProperty HorizontalTextAlignmentProperty =
			BindableProperty.Create(nameof(HorizontalTextAlignment), typeof(TextAlignment), typeof(CardImageView), TextAlignment.Start,
			                        BindingMode.OneWay, null, (s, oldValue, newValue) => OnPropertyChanged(nameof(HorizontalTextAlignment), s, oldValue, newValue));
		
		#endregion

		private static void OnPropertyChanged(string property, BindableObject sender, object oldValue, object newValue)
		{
			((CardImageView)sender)?.OnPropertyChanged(property);
		}
	}
}
