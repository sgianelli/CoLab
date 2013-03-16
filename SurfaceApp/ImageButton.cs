using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using Microsoft.Surface.Presentation.Controls;
using System.Windows.Data;

namespace SurfaceApp
{
    public class ImageButton : SurfaceButton
    {
        public ImageButton(string imageName)
        {
            base.Content = new Image()
            {
                Source = new BitmapImage(new Uri("Resources/" + imageName, UriKind.Relative))
            };
            base.HorizontalContentAlignment = System.Windows.HorizontalAlignment.Center;
            base.VerticalContentAlignment = System.Windows.VerticalAlignment.Center;
            base.Padding = new System.Windows.Thickness(0);

            base.SizeChanged += new System.Windows.SizeChangedEventHandler(ImageButton_SizeChanged);
        }

        void ImageButton_SizeChanged(object sender, System.Windows.SizeChangedEventArgs e)
        {
            if (base.ActualHeight > base.ActualWidth)
                base.Width = base.ActualHeight;

            else if (base.ActualWidth > base.ActualHeight)
                base.Height = base.ActualWidth;
        }
    }
}
