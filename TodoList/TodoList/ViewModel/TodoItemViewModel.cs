using System;
using System.ComponentModel;
using System.Windows.Input;
using Xamarin.Forms;

namespace TodoList
{
	public class TodoItemViewModel : INotifyPropertyChanged
	{
		string _title;
		public string Title
		{
			get
			{
				return _title;
			}
			set
			{
				_title = value;
				OnPropertyChanged("Title");
			}
		}

        string _text;
        public string Text
        {
            get
            {
                return _text;
            }
            set
            {
                _text = value;
                OnPropertyChanged("Text");
            }
        }

        bool _isDone;
		public bool IsDone
		{
			get
			{
				return _isDone;
			}
			set
			{
				_isDone = value;
				OnPropertyChanged("IsDone");
			}
		}

		public event PropertyChangedEventHandler PropertyChanged;

		private void OnPropertyChanged(string propertyName)
		{
			if (PropertyChanged != null)
				PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
		}

		public ICommand DoneCommand { get; private set; }

		public TodoItemViewModel()
		{
			DoneCommand = new Command(Done);
		}

		void Done()
		{
			Application.Current.MainPage.Navigation.PopModalAsync();
		}
	}
}
