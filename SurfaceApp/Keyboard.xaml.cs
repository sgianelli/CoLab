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
    public partial class Keyboard : UserControl
    {

        public static readonly DependencyProperty IsURLProp = DependencyProperty.Register("IsURL", typeof(Boolean), typeof(Keyboard));

        public bool IsURL
        {
            get { return (bool)GetValue(IsURLProp); }
            set { SetValue(IsURLProp, value); }
        }

        private bool isShift = false;

        public Keyboard()
        {
            InitializeComponent();

            ShiftedKeys.Add('1', '!');
            ShiftedKeys.Add('2', '@');
            ShiftedKeys.Add('3', '#');
            ShiftedKeys.Add('4', '$');
            ShiftedKeys.Add('5', '%');
            ShiftedKeys.Add('6', '^');
            ShiftedKeys.Add('7', '&');
            ShiftedKeys.Add('8', '*');
            ShiftedKeys.Add('9', '(');
            ShiftedKeys.Add('0', ')');
            ShiftedKeys.Add('-', '_');
            ShiftedKeys.Add('=', '+');

            ShiftedKeys.Add('[', '{');
            ShiftedKeys.Add(']', '}');
            ShiftedKeys.Add('\\', '|');

            ShiftedKeys.Add(';', ':');
            ShiftedKeys.Add('\'', '"');

            ShiftedKeys.Add('.', '<');
            ShiftedKeys.Add(',', '>');

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            isShift = !isShift;
            Render_Keys();
        }

        private Dictionary<char, char> ShiftedKeys = new Dictionary<char, char>();

        private void Render_Keys()
        {
            foreach (Button b in FindVisualChildren<Button>(MainGrid))
            {
                if (b.Tag.ToString().Length > 1) continue;
                Char x = b.Tag.ToString().ToCharArray()[0];
                if (Char.IsLetter(x))
                    b.Content = (isShift ? x.ToString().ToUpper() : x.ToString().ToLower());
                if (ShiftedKeys.ContainsKey(x))
                    b.Content = (isShift ? ShiftedKeys[x] : x).ToString();
            }

            MainGrid.RowDefinitions[0].Height = (IsURL ? gridLength1 : gridLength0);
        }

        public static IEnumerable<T> FindVisualChildren<T>(DependencyObject depObj) where T : DependencyObject
        {
            if (depObj != null)
            {
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
                {
                    DependencyObject child = VisualTreeHelper.GetChild(depObj, i);
                    if (child != null && child is T)
                    {
                        yield return (T)child;
                    }

                    foreach (T childOfChild in FindVisualChildren<T>(child))
                    {
                        yield return childOfChild;
                    }
                }
            }
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            Render_Keys();
        }


        public class KeyEventArgs : EventArgs
        {
            public String Str { get; private set; }
            public KeyEventArgs(String c)
            {
                Str = c;
            }
        }

        public event EventHandler<KeyEventArgs> KeyPressed;
        public event EventHandler CharacterDeleted;
        public event EventHandler EnterPressed;

        public event EventHandler KeyboardHidden;
        public event EventHandler KeyboardShown;

        public event EventHandler CaretMovedForward;
        public event EventHandler CaretMovedBackward;

        public event EventHandler SelectionMovedForward;
        public event EventHandler SelectionMovedBackward;

        private void SurfaceButton_Click(object sender, RoutedEventArgs e)
        {
            if (KeyPressed == null) return;
            KeyPressed(this, new KeyEventArgs(((Button)sender).Content.ToString()));
            if (isShift)
            {
                isShift = false;
                Render_Keys();
            }
        }

        private void SurfaceButton2_Click(object sender, RoutedEventArgs e)
        {
            if (KeyPressed == null) return;
            KeyPressed(this, new KeyEventArgs(((Button)sender).Tag.ToString()));
        }

        private void Enter_Button(object sender, RoutedEventArgs e)
        {
            if (EnterPressed == null) return;
            EnterPressed(this, EventArgs.Empty);
        }

        private void Backspace_Button(object sender, RoutedEventArgs e)
        {
            if (CharacterDeleted == null) return;
            CharacterDeleted(this, EventArgs.Empty);
        }

        private void Caret_Forward(object sender, RoutedEventArgs e)
        {
            if (isShift)
            {
                if (CaretMovedForward == null) return;
                SelectionMovedForward(this, EventArgs.Empty);
            }
            else
            {
                if (CaretMovedForward == null) return;
                CaretMovedForward(this, EventArgs.Empty);
            }
        }

        private void Caret_Backward(object sender, RoutedEventArgs e)
        {
            if (isShift)
            {
                if (CaretMovedBackward == null) return;
                SelectionMovedBackward(this, EventArgs.Empty);
            }
            else
            {
                if (CaretMovedBackward == null) return;
                CaretMovedBackward(this, EventArgs.Empty);
            }
        }

        private void SurfaceButton_Click_1(object sender, RoutedEventArgs e)
        {
            if (KeyPressed == null) return;
            KeyPressed(this, new KeyEventArgs(" "));
        }

        private bool isHidden = false;

        private static GridLength gridLength0 = new GridLength(0, GridUnitType.Star);
        private static GridLength gridLength1 = new GridLength(1, GridUnitType.Star);

        private void SurfaceButton_Hide_Click(object sender, RoutedEventArgs e)
        {
            isHidden = !isHidden;

            var lastRow = MainGrid.Children[MainGrid.Children.Count - 1];

            if (isHidden)
            {
                for (int i = 0; i < MainGrid.RowDefinitions.Count - 1; ++i)
                    MainGrid.RowDefinitions[i].Height = gridLength0;

                Rar.Visibility = Visibility.Hidden;
                Lar.Visibility = Visibility.Hidden;
                Spc.Visibility = Visibility.Hidden;

                Hide1.Content = "Keyboard";
                Hide2.Content = "Keyboard";

                if (KeyboardHidden != null)
                    KeyboardHidden(this, EventArgs.Empty);
            }
            else
            {
                for (int i = 1; i < MainGrid.RowDefinitions.Count - 1; ++i)
                    MainGrid.RowDefinitions[i].Height = gridLength1;

                Render_Keys();

                Rar.Visibility = Visibility.Visible;
                Lar.Visibility = Visibility.Visible;
                Spc.Visibility = Visibility.Visible;

                Hide1.Content = "Hide";
                Hide2.Content = "Hide";

                if (KeyboardShown != null)
                    KeyboardShown(this, EventArgs.Empty);
            }
        }
    }
}
