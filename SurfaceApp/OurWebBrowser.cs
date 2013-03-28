using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows;
using Microsoft.Surface.Presentation.Controls;
using System.Windows.Media;
using Awesomium.Windows.Controls;
using Awesomium.Core;
using System.IO;
using System.Windows.Media.Imaging;

namespace SurfaceApp
{
    public class OurWebBrowser : Grid
    {
        public event EventHandler OnSaved;

        private WebControl _webBrowser;
        private SurfaceTextBox _textBox;
        private string _url;

        private ImageButton _backButton, _forwardButton;

        public OurWebBrowser()
        {
            base.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
            base.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(1, GridUnitType.Star) });
            base.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(280, GridUnitType.Pixel) });
            base.Background = new SolidColorBrush(Color.FromArgb(200, 50, 50, 50));

            _webBrowser = new WebControl()
            {
                VerticalAlignment = System.Windows.VerticalAlignment.Stretch,
                HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch,
                Margin = new Thickness(12, 6, 12, 12)
            };
            _webBrowser.BeginLoading += new BeginLoadingEventHandler(_webBrowser_BeginLoading);
            LoadUrl("msn.com");
            Grid.SetRow(_webBrowser, 1);
            base.Children.Add(_webBrowser);
            
            //grid for the top items
            Grid grid = new Grid();
            grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = GridLength.Auto });
            grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) });
            grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = GridLength.Auto });
            Grid.SetRow(grid, 0);
            base.Children.Add(grid);

            //stack panel for left buttons
            StackPanel sp = new StackPanel()
            {
                Orientation = System.Windows.Controls.Orientation.Horizontal
                //Margin = new Thickness(12, 0, 0, 0)
            };
            Grid.SetColumn(sp, 0);
            grid.Children.Add(sp);

            _backButton = new ImageButton("Back.png") { IsEnabled = false };
            _backButton.Click += new System.Windows.RoutedEventHandler(backButton_Click);
            sp.Children.Add(_backButton);

            _forwardButton = new ImageButton("188-ArrowRounded.png") { IsEnabled = false };
            _forwardButton.Click += new System.Windows.RoutedEventHandler(forwardButton_Click);
            sp.Children.Add(_forwardButton);


            //url bar
            _textBox = new SurfaceTextBox() { VerticalContentAlignment = System.Windows.VerticalAlignment.Center };
            _textBox.Loaded += delegate { _textBox.Focus(); }; //focus on loaded
            _textBox.KeyUp += new System.Windows.Input.KeyEventHandler(_textBox_KeyUp);
            Grid.SetColumn(_textBox, 1);
            grid.Children.Add(_textBox);



            //right buttons
            StackPanel right = new StackPanel() { Orientation = System.Windows.Controls.Orientation.Horizontal };
            Grid.SetColumn(right, 2);
            grid.Children.Add(right);

            //enter button
            ImageButton enterButton = new ImageButton("139-Play.png");
            enterButton.Click += new RoutedEventHandler(enterButton_Click);
            right.Children.Add(enterButton);


            //save button
            ImageButton saveButton = new ImageButton("178-Save.png");
            saveButton.Click += new RoutedEventHandler(saveButton_Click);
            right.Children.Add(saveButton);

            // keyboard
            this.kb = new Keyboard();
            kbwi = new KeyboardWebInput(kb, _webBrowser);
            kbti = new KeyboardTextInput(kb, _textBox);

            _textBox.GotFocus += textGotFocus;
            _textBox.LostFocus += textLostFocus;

            _webBrowser.GotFocus += webGotFocus;
            _webBrowser.LostFocus += webLostFocus;

            kb.KeyboardHidden += keyBoardHidden;
            kb.KeyboardShown += keyBoardShown;

            Grid.SetRow(kb, 2);
            base.Children.Add(kb);
        }

        public AbstractKeyboard kb;

        KeyboardWebInput kbwi;
        KeyboardTextInput kbti;



        public void LoadUrl(string url)
        {
            url = url.Trim();

            //search
            if (url.Contains(' ') || !url.Contains('.'))
                loadUrlHelper("http://google.com/search?q=" + url);

            else if (!url.StartsWith("http://") && !url.StartsWith("https://"))
                loadUrlHelper("http://" + url);
            else
                loadUrlHelper(url);
        }

        private void loadUrlHelper(string url)
        {
            if (_url != null && url != null && _url.Equals(url))
                return;

            _url = url;

            _webBrowser.LoadURL(url);
        }

        void saveButton_Click(object sender, RoutedEventArgs e)
        {
            if (OnSaved != null)
                OnSaved(this, new EventArgs());
        }

        public bool UrlIsFocused
        {
            get
            {
                return _textBox.IsFocused;
            }
        }

        public WebsiteLink GetWebsiteLink()
        {
            return new WebsiteLink(_url, _webBrowser.Title, GetImage());
        }

        public BitmapImage GetImage()
        {
            try
            {
                using (WebView view = WebCore.CreateWebView(300, 300))
                {
                    //string text = Clipboard.GetText();
                    //if (!view.LoadHTML(Clipboard.GetText()))
                    //    return null;
                    if (!view.LoadURL(_url))
                        return null;

                    while (view.IsLoadingPage)
                        WebCore.Update();

                    view.Render().SaveToJPEG("image", 90);
                }

                FileStream stream = File.OpenRead("image");
                return new BitmapImage(new Uri(stream.Name, UriKind.Absolute));
            }

            catch (Exception e) { MessageBox.Show(e.ToString()); return null; }
        }

        void textGotFocus(object sender, EventArgs e)
        {
            kbti.Enabled = true;
        }

        void textLostFocus(object sender, EventArgs e)
        {
            kbti.Enabled = false ;
        }

        void webGotFocus(object sender, EventArgs e)
        {
            kbwi.Enabled = true;
        }

        void webLostFocus(object sender, EventArgs e)
        {
            kbwi.Enabled = false;
        }

        void keyBoardHidden(object sender, EventArgs e)
        {
            base.RowDefinitions[2].Height = new GridLength(60, GridUnitType.Pixel);
        }

        void keyBoardShown(object sender, EventArgs e)
        {
            base.RowDefinitions[2].Height = new GridLength(280, GridUnitType.Pixel);
        }

        void _webBrowser_BeginLoading(object sender, BeginLoadingEventArgs e)
        {
            _textBox.Text = _url;
            _backButton.IsEnabled = _webBrowser.HistoryBackCount > 0;
            _forwardButton.IsEnabled = _webBrowser.HistoryForwardCount > 0;
        }

        void _textBox_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Enter)
                enterButton_Click(sender, new RoutedEventArgs());
        }

        void enterButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                LoadUrl(_textBox.Text);
            }

            catch { MessageBox.Show("You should be slapped."); }
        }

        void forwardButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            try
            {
                _webBrowser.GoForward();
            }

            catch { }
        }

        void backButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            try
            {
                _webBrowser.GoBack();
            }

            catch { }
        }
    }
}
