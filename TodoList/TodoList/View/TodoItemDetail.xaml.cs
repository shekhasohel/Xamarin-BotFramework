using System;
using Xamarin.Forms;

namespace TodoList
{
	public partial class TodoItemDetail : ContentPage
	{
		public TodoItemDetail(TodoItemViewModel Item)
		{
			InitializeComponent();
			BindingContext = Item;
		}
	}
}