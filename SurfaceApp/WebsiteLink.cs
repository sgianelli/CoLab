using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Surface.Presentation.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace SurfaceApp
{
    public class WebsiteLink : ScatterViewItem
    {
        public string Url { get; private set; }

        private Grid _help;

        public WebsiteLink(string url, string title, BitmapImage img)
        {
            Url = url;

            Grid grid = new Grid()
            {
                Background = new SolidColorBrush(Color.FromArgb(200, 50, 50, 50))
            };
            grid.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
            grid.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
            base.Content = grid;

            if (title != null)
            {
                TextBlock text = new TextBlock()
                {
                    Text = title,
                    Foreground = new SolidColorBrush(Colors.White),
                    FontSize = 16,
                    Margin = new Thickness(12, 0, 12, 12)
                };
                Grid.SetRow(text, 1);
                grid.Children.Add(text);
            }

            Grid imgContainer = new Grid();
            Grid.SetRow(imgContainer, 0);
            grid.Children.Add(imgContainer);


            SurfaceButton button = new SurfaceButton()
            {
                Padding = new Thickness(0),
                Content = new Image()
                {
                    Source = img,
                    Margin = new Thickness(12)
                }
            };
            button.Click += new RoutedEventHandler(button_Click);
            imgContainer.Children.Add(button);


            _help = new Grid()
            {
                Background = new SolidColorBrush(Colors.Black),
                Opacity = 0,
                Visibility = System.Windows.Visibility.Collapsed
            };
            imgContainer.Children.Add(_help);

            _help.Children.Add(new TextBlock()
            {
                Text = "Drop the page on your web browser to open the link.",
                TextWrapping = TextWrapping.Wrap,
                Foreground = new SolidColorBrush(Colors.White),
                Margin = new Thickness(24)
            });
        }

        void button_Click(object sender, RoutedEventArgs e)
        {
            if (_help.Visibility == System.Windows.Visibility.Visible)
            {
                return;
            }
            else { }

            _help.Visibility = System.Windows.Visibility.Visible;

            DoubleAnimation a = new DoubleAnimation()
            {
                From = 0,
                To = 0.7,
                Duration = TimeSpan.FromSeconds(1)
            };

            Storyboard.SetTarget(a, _help);
            Storyboard.SetTargetProperty(a, new PropertyPath("Opacity"));

            Storyboard sb = new Storyboard();
            sb.Children.Add(a);
            sb.Begin();
        }

        private DateTime _down = DateTime.MinValue;
        protected override void OnTouchDown(System.Windows.Input.TouchEventArgs e)
        {
            base.OnTouchDown(e);

            _down = DateTime.Now;
        }

        protected override void OnTouchUp(System.Windows.Input.TouchEventArgs e)
        {
            base.OnTouchUp(e);

            //Normal tap
            if ((DateTime.Now - _down).TotalSeconds < 0.7)
            {
                
            }
        }
    }
}
