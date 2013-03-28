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
using Microsoft.Surface;
using Microsoft.Surface.Presentation;
using Microsoft.Surface.Presentation.Controls;
using Microsoft.Surface.Presentation.Input;

namespace SurfaceApp
{
    public class KeyboardTextInput
    {
        private AbstractKeyboard k;
        private SurfaceTextBox t;

        public bool Enabled = false;

        public KeyboardTextInput(AbstractKeyboard k, SurfaceTextBox t)
        {
            this.k = k;
            this.t = t;

            k.CaretMovedBackward += this.keyboard1_CaretMovedBackward;
            k.CaretMovedForward += this.keyboard1_CaretMovedForward;
            k.CharacterDeleted += this.keyboard1_CharacterDeleted;
            k.SelectionMovedBackward += this.keyboard1_SelectionMovedBackward;
            k.SelectionMovedForward += this.keyboard1_SelectionMovedForward;
            k.KeyPressed += this.keyboard1_KeyEvent;
        }

        private void keyboard1_KeyEvent(object sender, Keyboard.KeyEventArgs e)
        {
            if (!Enabled) return;
            int selStar = t.SelectionStart;
            t.Text = t.Text.Substring(0, selStar) + e.Str + t.Text.Substring(selStar + t.SelectionLength);
            t.SelectionStart = selStar + e.Str.Length;
            t.SelectionLength = 0;
        }

        private void keyboard1_CharacterDeleted(object sender, EventArgs e)
        {
            if (!Enabled) return;
            if (t.Text.Length == 0) return;
            int selStar = t.SelectionStart;
            if (t.SelectionLength == 0)
            {
                t.Text = t.Text.Substring(0, selStar - 1) + t.Text.Substring(selStar + t.SelectionLength);
                t.SelectionStart = selStar - 1;
                t.SelectionLength = 0;
            }
            else
            {
                t.Text = t.Text.Substring(0, selStar) + t.Text.Substring(selStar + t.SelectionLength);
                t.SelectionStart = selStar;
                t.SelectionLength = 0;
            }
        }

        private void keyboard1_CaretMovedBackward(object sender, EventArgs e)
        {
            if (!Enabled) return;
            if (t.SelectionStart <= 0) return;
            t.SelectionStart--;
            t.SelectionLength = 0;
        }

        private void keyboard1_CaretMovedForward(object sender, EventArgs e)
        {
            if (!Enabled) return;
            t.SelectionStart++;
            t.SelectionLength = 0;
        }

        private int selDir = 0;

        private void keyboard1_SelectionMovedBackward(object sender, EventArgs e)
        {
            if (!Enabled) return;
            if (t.SelectionLength == 0)
                selDir = -1;

            if (selDir == -1)
            {
                if (t.SelectionStart == 0) return;
                t.SelectionStart--;
                t.SelectionLength++;
            }
            else
            {
                t.SelectionLength--;
            }
        }

        private void keyboard1_SelectionMovedForward(object sender, EventArgs e)
        {
            if (!Enabled) return;
            if (t.SelectionLength == 0)
                selDir = 0;

            if (selDir == -1)
            {
                t.SelectionLength--;
                t.SelectionStart++;
            }
            else
            {
                t.SelectionLength++;
            }
        }

    }
}
