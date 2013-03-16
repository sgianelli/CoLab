using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using Microsoft.Surface.Presentation.Controls;

namespace SurfaceApp
{
    public class SwitchButton : SurfaceButton
    {
        public event EventHandler DisplayedSwitched;

        public enum ImageDisplayed
        {
            One, Two
        };

        private Image _imageOne, _imageTwo;

        public SwitchButton(string imageNameOne, string imageNameTwo)
        {
            _imageOne = new Image()
            {
                Source = new BitmapImage(new Uri("Resources/" + imageNameOne, UriKind.Relative))
            };

            _imageTwo = new Image()
            {
                Source = new BitmapImage(new Uri("Resources/" + imageNameTwo, UriKind.Relative))
            };

            base.Content = _imageOne;
            base.Background = new SolidColorBrush(Colors.Gray);
            base.HorizontalContentAlignment = System.Windows.HorizontalAlignment.Center;
            base.VerticalContentAlignment = System.Windows.VerticalAlignment.Center;
            base.Padding = new System.Windows.Thickness(0);

            base.Click += new System.Windows.RoutedEventHandler(SwitchButton_Click);

            base.SizeChanged += new System.Windows.SizeChangedEventHandler(SwitchButton_SizeChanged);
        }

        void SwitchButton_SizeChanged(object sender, System.Windows.SizeChangedEventArgs e)
        {
            if (base.ActualHeight > base.ActualWidth)
                base.Width = base.ActualHeight;

            else if (base.ActualWidth > base.ActualHeight)
                base.Height = base.ActualWidth;
        }

        void SwitchButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (Displayed == ImageDisplayed.One)
                Displayed = ImageDisplayed.Two;

            else
                Displayed = ImageDisplayed.One;

            if (DisplayedSwitched != null)
                DisplayedSwitched(this, new EventArgs());
        }

        public ImageDisplayed Displayed
        {
            get
            {
                if (base.Content == _imageOne)
                    return ImageDisplayed.One;

                return ImageDisplayed.Two;
            }

            set
            {
                if (value == ImageDisplayed.One)
                    base.Content = _imageOne;

                else
                    base.Content = _imageTwo;
            }
        }
    }
}
