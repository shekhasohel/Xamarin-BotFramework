using System;
using System.ComponentModel;
using System.Windows.Input;
using Xamarin.Forms;

namespace TodoList
{
	public class ExtendedListView : ListView
	{
		public ExtendedListView()
		{

		}

		public static readonly BindableProperty ItemSelectedCommandProperty = BindableProperty.Create(
			propertyName: "ItemSelectedCommand",
			returnType: typeof(Command),
			declaringType: typeof(ExtendedListView),
			defaultValue: false);

		public Command ItemSelectedCommand
		{
			get { return (Command)GetValue(ItemSelectedCommandProperty); }
			set { SetValue(ItemSelectedCommandProperty, value); }
		}

		private void OnItemSelected(object sender, SelectedItemChangedEventArgs e)
		{
			if (e.SelectedItem == null)
				return;

			if (ItemSelectedCommand != null && ItemSelectedCommand.CanExecute(e.SelectedItem))
				ItemSelectedCommand.Execute(e.SelectedItem);
		}
	}
}