using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Surface.Presentation.Controls;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Ink;
using ExternalContentManager;

namespace SurfaceApp
{
    public class ISBNField : ScatterViewItem
    {
        public ScatterView parentScatter;

        private SurfaceTextBox _isbnBox = new SurfaceTextBox();
        private Grid _grid;

        public ISBNField() {
            base.CanScale = true;

            _isbnBox.FontSize += 16;

            _grid = new Grid()
            {
                VerticalAlignment = System.Windows.VerticalAlignment.Stretch,
                Background = new SolidColorBrush(Color.FromArgb(200, 50, 50, 50))
            };
            _grid.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
            _grid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(1, GridUnitType.Star) });
            _grid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(230, GridUnitType.Pixel) });

            base.Content = _grid;

            TextBlock _isbnLbl = new TextBlock();
            _isbnLbl.Foreground = Brushes.Wheat;
            _isbnLbl.FontSize = _isbnBox.FontSize - 8;
            _isbnLbl.Text = "Lookup ISBN Number:";

            ImageButton _goButton = new ImageButton("139-Play.png");
            _goButton.HorizontalAlignment = System.Windows.HorizontalAlignment.Right;
            _goButton.Click += new RoutedEventHandler(goButton_Click);

            Grid.SetRow(_isbnLbl, 0);
            Grid.SetRow(_isbnBox, 1);
            Grid.SetRow(_goButton, 1);

            _grid.Children.Add(_isbnLbl);
            _grid.Children.Add(_isbnBox);
            _grid.Children.Add(_goButton);

            AbstractKeyboard kb = new NumKeyboard();
            KeyboardTextInput kbti = new KeyboardTextInput(kb, _isbnBox);
            kbti.Enabled=true;
            //Grid.SetColumnSpan(kb, 2);
            Grid.SetRow(kb, 2);
            _grid.Children.Add(kb);
        }

        void goButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                GoogleBook book = ContentManager.fetchBookForISBN(_isbnBox.Text);

                Uri uri;

                if (book.volumeInfo.imageLinks != null)
                    uri = new Uri(book.volumeInfo.imageLinks.thumbnail, UriKind.Absolute);
                else
                    uri = new Uri("Resources/fillerBook.png", UriKind.Relative);

                WebFactory.LastFactory.AddLink(new WebsiteLink("https://www.google.com/search?q=" + _isbnBox.Text, book.volumeInfo.title, new BitmapImage(uri)));
            }
            catch
            {
            }
        }
        
    }
}
