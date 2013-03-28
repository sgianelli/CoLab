
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SurfaceApp
{

    /// <summary>
    /// Interaction logic for Keyboard.xaml
    /// </summary>
    public class AbstractKeyboard : UserControl
    {


        public class KeyEventArgs : EventArgs
        {
            public String Str { get; private set; }
            public KeyEventArgs(String c)
            {
                Str = c;
            }
        }

        protected bool isShift = false;

        public event EventHandler<KeyEventArgs> KeyPressed;
        public event EventHandler CharacterDeleted;
        public event EventHandler EnterPressed;

        public event EventHandler KeyboardHidden;
        public event EventHandler KeyboardShown;

        public event EventHandler CaretMovedForward;
        public event EventHandler CaretMovedBackward;

        public event EventHandler SelectionMovedForward;
        public event EventHandler SelectionMovedBackward;


        public void SurfaceButton2_Click(object sender, RoutedEventArgs e)
        {
            if (KeyPressed == null) return;
            KeyPressed(this, new KeyEventArgs(((Button)sender).Tag.ToString()));
        }

        public void Enter_Button(object sender, RoutedEventArgs e)
        {
            if (EnterPressed == null) return;
            EnterPressed(this, EventArgs.Empty);
        }

        public void Backspace_Button(object sender, RoutedEventArgs e)
        {
            if (CharacterDeleted == null) return;
            CharacterDeleted(this, EventArgs.Empty);
        }

        public void Caret_Forward(object sender, RoutedEventArgs e)
        {
            if (isShift)
            {
                if (SelectionMovedForward == null) return;
                SelectionMovedForward(this, EventArgs.Empty);
            }
            else
            {
                if (CaretMovedForward == null) return;
                CaretMovedForward(this, EventArgs.Empty);
            }
        }

        public void Caret_Backward(object sender, RoutedEventArgs e)
        {
            if (isShift)
            {
                if (SelectionMovedBackward == null) return;
                SelectionMovedBackward(this, EventArgs.Empty);
            }
            else
            {
                if (CaretMovedBackward == null) return;
                CaretMovedBackward(this, EventArgs.Empty);
            }
        }

        public void AllButtons_Click(object sender, RoutedEventArgs e)
        {
            if (KeyPressed == null) return;
            KeyPressed(this, new KeyEventArgs(((Button)sender).Content.ToString()));
        }

        public void SurfaceButton_Click_1(object sender, RoutedEventArgs e)
        {
            if (KeyPressed == null) return;
            KeyPressed(this, new KeyEventArgs(" "));
        }

        public void keyboardV(bool hidden)
        {
            if (hidden)
            {
                if (KeyboardHidden != null)
                    KeyboardHidden(this, EventArgs.Empty);
            }
            else
            {
                if (KeyboardShown != null)
                    KeyboardShown(this, EventArgs.Empty);
            }
        }

    }
}
