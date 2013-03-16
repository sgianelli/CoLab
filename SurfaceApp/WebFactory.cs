using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Surface.Presentation.Controls;
using System.Windows.Input;
using System.Windows;

namespace SurfaceApp
{
    public class WebFactory
    {
        public static WebFactory LastFactory;

        private ScatterView _scatterView;

        public WebFactory(ScatterView scatterView)
        {
            _scatterView = scatterView;

            LastFactory = this;
        }

        private List<OurWebBrowser> _webBrowsers = new List<OurWebBrowser>();
        private List<WebsiteLink> _links = new List<WebsiteLink>();

        public void AddLink(WebsiteLink link)
        {
            _links.Add(link);
            _scatterView.Items.Add(link);

            //watch the movements
            link.LayoutUpdated += delegate
            {
                touchReleased(link, link.TranslatePoint(new Point(0, 0), null), link.TranslatePoint(new Point(0, 0), null));
            };
        }

        /// <summary>
        /// Adds a new web browser to the scatter control
        /// </summary>
        public void NewWebBrowser()
        {
            OurWebBrowser wb = new OurWebBrowser();
            ScatterViewItem item = new ScatterViewItem()
            {
                Width = 800,
                Height = 700,
                Content = wb
            };
            _webBrowsers.Add(wb);
            _scatterView.Items.Add(item);

            //watch for the save button
            wb.OnSaved += new EventHandler(wb_OnSaved);
        }

        void wb_OnSaved(object sender, EventArgs e)
        {
            AddLink((sender as OurWebBrowser).GetWebsiteLink());
        }

        

        private void touchReleased(object sender, Point curr, Point last)
        {
            if (Math.Abs(curr.X - last.X) > 4 || Math.Abs(curr.Y - last.Y) > 4)
                return;

            //look through all web browsers to see which it was placed on
            foreach (OurWebBrowser wb in _webBrowsers)
            {
                Point point = wb.TranslatePoint(new Point(0, 0), null);

                if (curr.X >= point.X && curr.X <= point.X + wb.ActualWidth &&
                    curr.Y >= point.Y && curr.Y <= point.Y + wb.ActualHeight)
                {
                    wb.LoadUrl((sender as WebsiteLink).Url);

                    return;
                }
            }
        }
    }
}
