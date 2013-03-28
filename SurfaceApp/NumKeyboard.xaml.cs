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
    public partial class NumKeyboard : AbstractKeyboard
    {

        public NumKeyboard()
        {
            InitializeComponent();
        }

        private void SurfaceButton_Click(object sender, RoutedEventArgs e)
        {
            base.AllButtons_Click(sender, e);
        }

        private void SurfaceButton2_Click(object sender, RoutedEventArgs e)
        {
            base.SurfaceButton2_Click(sender, e);
        }

        private void Enter_Button(object sender, RoutedEventArgs e)
        {
            base.Enter_Button(sender, e);
        }

        private void Backspace_Button(object sender, RoutedEventArgs e)
        {
            base.Backspace_Button(sender, e);
        }

        private void Caret_Forward(object sender, RoutedEventArgs e)
        {
            base.Caret_Forward(sender, e);
        }

        private void Caret_Backward(object sender, RoutedEventArgs e)
        {
            base.Caret_Backward(sender, e);
        }

        private void SurfaceButton_Click_1(object sender, RoutedEventArgs e)
        {
            base.SurfaceButton_Click_1(sender, e);
        }

    }
}
