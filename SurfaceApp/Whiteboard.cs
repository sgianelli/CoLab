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

namespace SurfaceApp
{
    public class Whiteboard : ScatterViewItem
    {
        private static StrokeCollection _groupStrokeCollection = new StrokeCollection();

        private StrokeCollection _privateStrokeCollection;

        private Grid _grid;
        private SurfaceInkCanvas _canvas;

        private MyButton _eraseButton, _drawButton;

        private SwitchButton _typeButton;

        public Whiteboard()
        {
            base.CanScale = true;
            
            _grid = new Grid()
            {
                VerticalAlignment = System.Windows.VerticalAlignment.Stretch,
                Background = new SolidColorBrush(Color.FromArgb(200, 50, 50, 50))
            };
            _grid.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
            _grid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(1, GridUnitType.Star) });
            base.Content = _grid;

            _canvas = new SurfaceInkCanvas()
            {
                Background = new SolidColorBrush(Colors.Black),
                VerticalAlignment = System.Windows.VerticalAlignment.Stretch,
                Margin = new Thickness(12, 0, 12, 12)
            };

            _privateStrokeCollection = _canvas.Strokes;

            Grid.SetRow(_canvas, 1);
            _grid.Children.Add(_canvas);

            StackPanel buttons = new StackPanel()
            {
                Orientation = System.Windows.Controls.Orientation.Horizontal,
                HorizontalAlignment = System.Windows.HorizontalAlignment.Right
            };
            Grid.SetRow(buttons, 0);
            _grid.Children.Add(buttons);


            _eraseButton = new MyButton("Erase.png");
            _eraseButton.SelectedChanged += new EventHandler(_eraseButton_SelectedChanged);
            buttons.Children.Add(_eraseButton);

            _drawButton = new MyButton("167-Painting.png") { IsSelected = true };
            _drawButton.SelectedChanged += new EventHandler(_drawButton_SelectedChanged);
            buttons.Children.Add(_drawButton);


            _typeButton = new SwitchButton("109-AdminUser.png", "108-Group.png")
            {
                Margin = new Thickness(12,0,0,0)
            };
            _typeButton.DisplayedSwitched += new EventHandler(_typeButton_DisplayedSwitched);
            buttons.Children.Add(_typeButton);
        }

        void _typeButton_DisplayedSwitched(object sender, EventArgs e)
        {
            if (_typeButton.Displayed == SwitchButton.ImageDisplayed.One)
            {
                _canvas.Strokes = _privateStrokeCollection;
            }

            else
            {
                _canvas.Strokes = _groupStrokeCollection;
            }
        }

        void _drawButton_SelectedChanged(object sender, EventArgs e)
        {
            if (_drawButton.IsSelected)
            {
                _canvas.EditingMode = SurfaceInkEditingMode.Ink;

                _eraseButton.IsSelected = false;
            }
        }

        void _eraseButton_SelectedChanged(object sender, EventArgs e)
        {
            if (_eraseButton.IsSelected)
            {
                _canvas.EditingMode = SurfaceInkEditingMode.EraseByStroke;

                _drawButton.IsSelected = false;
            }
        }
    }
}
