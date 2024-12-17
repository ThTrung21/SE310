using BookMK.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace BookMK.Converters
{
	public class BookIdToTitle : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value is int bookId)
			{
				// Use Task.Run or cached title lookup to avoid blocking UI
				return Task.Run(() => Book.GetTitle(bookId)).Result;
			}
			return "Unknown Title";
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}

}
