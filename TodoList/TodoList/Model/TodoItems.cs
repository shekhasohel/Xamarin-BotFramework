using System;
using System.Collections.ObjectModel;

namespace TodoList
{
	public static class TodoItems
	{
		public static ObservableCollection<TodoItemViewModel> Items = new ObservableCollection<TodoItemViewModel>();

		static TodoItems()
		{
			Items.Add(new TodoItemViewModel()
			{
				Title = "Reminder",
                Text = "Buy Milk",
				IsDone = false
			});
			Items.Add(new TodoItemViewModel()
			{
				Title = "Pickup Parcel",
				IsDone = true
			});
			Items.Add(new TodoItemViewModel()
			{
                Title = "Home",
				Text = "Prepare Food",
				IsDone = false
			});
			Items.Add(new TodoItemViewModel()
			{
                Title = "Work",
                Text = "Schedule Week Plan",
				IsDone = false
			});
		}
	}
}
