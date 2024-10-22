using BookMK.Service;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace BookMK.Converters
{
    public class PathToBookCover:IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var imageUrl = value as string;
            if (!string.IsNullOrEmpty(imageUrl))
            {
                // Return the image from URL
                return new BitmapImage(new Uri(imageUrl));
            }
            return LoadImage("BookCoverExample.png");

            //string filename = value as string;

            //if (string.IsNullOrEmpty(filename))
            //{
            //    return LoadImage("BookCoverExample.png");
            //}

            //string imagePath = Path.Combine(ImageStorage.BookImageLocation, filename);

            //if (!File.Exists(imagePath))
            //{
            //    // Handle the case when the file doesn't exist
            //    return LoadImage("BookCoverExample.png");
            //}

            //return LoadImage(imagePath);
        }


        private BitmapImage LoadImage(string filename)
        {
            BitmapImage bmp = new BitmapImage();
            bmp.BeginInit();
            bmp.CacheOption = BitmapCacheOption.OnLoad;
            bmp.UriSource = new Uri(Path.Combine(ImageStorage.CurrentSolutionLocation, "Images", filename), UriKind.Absolute);
            bmp.EndInit();

            return bmp;
        }
        
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
