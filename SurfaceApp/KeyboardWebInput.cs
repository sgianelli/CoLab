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
using Awesomium.Windows.Controls;

namespace SurfaceApp
{
    public class KeyboardWebInput
    {
        private Keyboard k;
        private WebControl w;

        public bool Enabled = false;

        public KeyboardWebInput(Keyboard k, WebControl w)
        {
            this.k = k;
            this.w = w;

            k.CaretMovedBackward += this.keyboard1_CaretMovedBackward;
            k.CaretMovedForward += this.keyboard1_CaretMovedForward;
            k.CharacterDeleted += this.keyboard1_CharacterDeleted;
            k.SelectionMovedBackward += this.keyboard1_SelectionMovedBackward;
            k.SelectionMovedForward += this.keyboard1_SelectionMovedForward;
            k.EnterPressed += this.keyboard1_Enter;
            k.KeyPressed += this.keyboard1_KeyEvent;
        }

        private void keyboard1_Enter(object sender, EventArgs e)
        {
            if (!Enabled) return;
            String JSTR = "";

            JSTR += "var evt1 = document.createEvent('KeyboardEvent');";
            JSTR += "evt1.initKeyboardEvent('keydown', true, true, null, 13, 13, '', false, null);";
            JSTR += "document.activeElement.dispatchEvent(evt1, null);";

            JSTR += "var evt2 = document.createEvent('KeyboardEvent');";
            JSTR += "evt2.initKeyboardEvent('keypress', true, true, null, 13, 13, '', false, null);";
            JSTR += "document.activeElement.dispatchEvent(evt2, null);";

            JSTR += "var evt3 = document.createEvent('KeyboardEvent');";
            JSTR += "evt3.initKeyboardEvent('keyup', true, true, null, 13, 13, '', false, null);";
            JSTR += "document.activeElement.dispatchEvent(evt3, null);";

            w.ExecuteJavascript(JSTR);
        }

        private void keyboard1_KeyEvent(object sender, Keyboard.KeyEventArgs e)
        {
            if (!Enabled) return;
            String JSTR = "";

            JSTR += "var t = document.activeElement;";
            JSTR += "var selStar = t.selectionStart;";
            JSTR += "t.value = t.value.substring(0, selStar) + '" + e.Str.Replace("'", "\\'") + "' + t.value.substring(t.selectionEnd);";
            JSTR += "t.selectionStart = selStar + " + e.Str.Length + ";";
            JSTR += "t.selectionEnd = t.selectionStart;";

            w.ExecuteJavascript(JSTR);
        }

        private void keyboard1_CharacterDeleted(object sender, EventArgs e)
        {
            if (!Enabled) return;

            String JSTR = "";

            JSTR += "var t = document.activeElement;";
            JSTR += "if (t.value.Length == 0) return;";
            JSTR += "var selStar = t.selectionStart;";
            JSTR += "if (t.selectionEnd == t.selectionStart)";
            JSTR += "{";
            JSTR += "t.value = t.value.substring(0, selStar - 1) + t.value.substring(selStar + t.selectionEnd - t.selectionStart);";
            JSTR += "t.selectionStart = selStar - 1;";
            JSTR += "t.selectionEnd = t.selectionStart;";
            JSTR += "}";
            JSTR += "else";
            JSTR += "{";
            JSTR += "t.value = t.value.substring(0, selStar) + t.value.substring(selStar + t.selectionEnd - t.selectionStart);";
            JSTR += "t.selectionStart = selStar;";
            JSTR += "t.selectionEnd = t.selectionStart;";
            JSTR += "}";

            w.ExecuteJavascript(JSTR);
        }

        private void keyboard1_CaretMovedBackward(object sender, EventArgs e)
        {
            if (!Enabled) return;

            String JSTR = "";

            JSTR += "var t = document.activeElement;";
            JSTR += "if (t.selectionStart <= 0) return;";
            JSTR += "t.selectionStart--;";
            JSTR += "t.selectionEnd = t.selectionStart;";

            w.ExecuteJavascript(JSTR);
        }

        private void keyboard1_CaretMovedForward(object sender, EventArgs e)
        {
            if (!Enabled) return;

            String JSTR = "";

            JSTR += "var t = document.activeElement;";
            JSTR += "t.selectionStart++;";
            JSTR += "t.selectionEnd = t.selectionStart;";

            w.ExecuteJavascript(JSTR);
        }

        private int selDir = 0;

        private void keyboard1_SelectionMovedBackward(object sender, EventArgs e)
        {
            if (!Enabled) return;

            String JSTR = "";
            JSTR += "var t = document.activeElement;";

            if (w.ExecuteJavascriptWithResult("(document.activeElement.selectionEnd - document.activeElement.selectionStart);").ToString().Equals("0"))
                selDir = -1;

            if (selDir == -1)
            {
                JSTR += "if (t.selectionStart == 0) return;";
                JSTR += "t.selectionStart--;";
            }
            else
            {
                JSTR += "t.SelectionEnd--;";
            }

            w.ExecuteJavascript(JSTR);
        }

        private void keyboard1_SelectionMovedForward(object sender, EventArgs e)
        {
            if (!Enabled) return;

            String JSTR = "";
            JSTR += "var t = document.activeElement;";

            if (w.ExecuteJavascriptWithResult("(document.activeElement.selectionEnd - document.activeElement.selectionStart);").ToString().Equals("0"))
                selDir = 0;

            if (selDir == -1)
            {
                JSTR += "t.selectionStart++;";
            }
            else
            {
                JSTR += "t.selectionEnd++;";
            }

            w.ExecuteJavascript(JSTR);
        }

    }
}
