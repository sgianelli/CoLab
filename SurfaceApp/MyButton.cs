using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Surface.Presentation.Controls;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Media;

namespace SurfaceApp
{
    public class MyButton : ImageButton
    {
        public event EventHandler SelectedChanged;

        private bool _ignoreClick;

        public MyButton(string imageName) : base(imageName)
        {
            base.Click += new System.Windows.RoutedEventHandler(MyButton_Click);
        }

        void MyButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (_ignoreClick)
                return;

            IsSelected = !IsSelected;
        }

        private bool _isSelected;
        public bool IsSelected
        {
            get
            {
                return _isSelected;
            }

            set
            {
                if (_isSelected == value)
                    return;

                _isSelected = value;

                if (value)
                {
                    base.Background = new SolidColorBrush(Color.FromArgb(255, 200, 0, 0));
                    _ignoreClick = true;
                }

                else
                {
                    base.Background = new SolidColorBrush(Colors.Gray);
                    _ignoreClick = false;
                }

                if (SelectedChanged != null)
                    SelectedChanged(this, new EventArgs());
            }
        }
    }
}
