using System;
using System.ComponentModel;
using Xamarin.Forms;

namespace TodoList
{
	public class CustomImage : Image, INotifyPropertyChanged
	{
		public CustomImage()
		{

		}

		public static readonly BindableProperty IsCheckedProperty = BindableProperty.Create(
			propertyName: "IsChecked",
			returnType: typeof(bool),
			declaringType: typeof(CustomImage),
			defaultValue: false,
			propertyChanged: OnPropertyChanged,
			validateValue: OnValidateValue,
			coerceValue: OnCoerceValue, defaultValueCreator: OnDefaultValueCreator);

		public bool IsChecked
		{
			get { return (bool)GetValue(IsCheckedProperty); }
			set { SetValue(IsCheckedProperty, value); }
		}

		static void OnPropertyChanged(BindableObject bindable, object oldValue, object newValue)
		{
			// not implemented;
		}

		static bool OnValidateValue(BindableObject bindable, object value)
		{
			return value is bool;
		}

		static object OnCoerceValue(BindableObject bindable, object value)
		{
			if (!(value is bool))
				return false;
			return value;
		}

		static object OnDefaultValueCreator(BindableObject bindable)
		{
			return true;
		}
	}

	public class SourceConvertor : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			return (bool)value ? "Tick.png" : "Untick.png";
		}

		public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			return value.ToString() == "Tick.png" ? true : false;
		}
	}
}