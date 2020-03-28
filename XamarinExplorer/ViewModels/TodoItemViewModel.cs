using System;
using Shared;
using Xamarin.Forms;

namespace XamarinExplorer.ViewModels
{
	public class ItemViewModel : BaseViewModel
	{
		public ItemViewModel(Item item)
		{
			Model = item;
		}

		public string Id
		{
			get => Model.Id;
		}

		public DateTime DateCreated
		{
			get => Model.DateCreated;
		}

		public new string Title
		{
			get => Model.Title;
			set
			{
				Model.Title = value;
				OnPropertyChanged();
			}
		}

		public bool IsCompleted
		{
			get => Model.IsCompleted;
			set
			{
				Model.IsCompleted = value;
				OnPropertyChanged();
			}
		}

		public Command UpdateCommand { get; set; }
		public Item Model { get; private set; }
	}
}
