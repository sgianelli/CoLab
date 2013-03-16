using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media.Imaging;

namespace SurfaceApp
{
    public class WebScreenshots
    {
        private static Dictionary<string, BitmapImage> _storedImages = new Dictionary<string, BitmapImage>();

        public static void Add(string url, BitmapImage img)
        {
            _storedImages[url] = img;
        }

        public static BitmapImage Get(string url)
        {
            if (_storedImages.ContainsKey(url))
                return _storedImages[url];

            return new BitmapImage();
        }
    }
}
