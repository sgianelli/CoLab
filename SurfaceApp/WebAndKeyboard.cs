using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Surface.Presentation.Controls;
using System.Windows.Controls;
using System.Windows;

namespace SurfaceApp
{
    public class WebAndKeyboard : ScatterViewItem
    {
        private OurWebBrowser _ourWebBrowser;

        public WebAndKeyboard()
        {
            Grid grid = new Grid()
            {
                VerticalAlignment = System.Windows.VerticalAlignment.Stretch
            };
            grid.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto }); //web browser
            grid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(300) }); //keyboard
            base.Content = grid;

            _ourWebBrowser = new OurWebBrowser();
            Grid.SetRow(_ourWebBrowser, 0);
            grid.Children.Add(_ourWebBrowser);

            //TODO - add keyboard
        }
    }
}
