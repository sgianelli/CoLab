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
using System.IO;

namespace SurfaceApp
{
    public class BookItem : ScatterViewItem
    {
        public BookItem(GoogleBook book)
        {
            Image img;

            if (book.volumeInfo.imageLinks != null) {
                img = new Image()
                {
                    Source = new BitmapImage(new Uri(book.volumeInfo.imageLinks.thumbnail, UriKind.Absolute))
                };
            } else {
                img = new Image()
                {
                    Source = new BitmapImage(new Uri("Resources/bookFiller.png", UriKind.Relative))
                };
            }

            this.AddChild(img);
        }
    }
}
