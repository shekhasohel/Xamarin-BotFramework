using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Xamarin.Forms;

namespace TodoList
{
	public partial class TodoItemsList : ContentPage
	{
		public TodoItemsList()
		{
			InitializeComponent();

			BindingContext = new TodoItemListViewModel();
		}
	}
}