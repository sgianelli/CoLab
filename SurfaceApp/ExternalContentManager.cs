using System;
using System.Net;
using System.IO;
using System.Drawing;
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
using mshtml;
using Microsoft.Surface;
using Microsoft.Surface.Presentation;
using Microsoft.Surface.Presentation.Controls;
using Microsoft.Surface.Presentation.Input;
using Newtonsoft.Json;
using System.Text.RegularExpressions;

namespace ExternalContentManager
{
    public class GoogleVolumeLink : Object
    {
        public String
            thumbnail,
            smallThumbnail;
    }
    public class GoogleVolume : Object
    {
        public String
            title,
            subtitle,
            publisher,
            publishedDate,
            description,
            language,
            previewLink,
            printType;
        public Int32
            pageCount;
        public String[]
            authors;
        public GoogleVolumeLink
            imageLinks;
    }
    public class GoogleBook : Object
    {
        public String
            kind,
            selfLink;
        public GoogleVolume
            volumeInfo;
    }
    public class GoogleBookResults : Object
    {
        public String kind;
        public Int32 totalItems;
        public GoogleBook[] items;
    }
    public class UABook : Object
    {
        public String
            location,
            isbn;

        public UABook(String location, String isbn)
        {
            this.location = location;
            this.isbn = isbn;
        }
    }

    public class ContentManager
    {
        private static IHTMLElement elementForID(IHTMLDocument2 doc, String id)
        {
            if (null != doc)
            {
                foreach (IHTMLElement element in doc.all)
                {
                    if (element.id != null && element.id.Length > 0)
                    {
                        if (element.id.Equals(id))
                        {
                            return element;
                        }
                    }
                }
            }

            return null;
        }

        private static IHTMLElement elementForClass(IHTMLDocument2 doc, String className)
        {
            if (null != doc)
            {
                foreach (IHTMLElement element in doc.all)
                {
                    if (element.className != null && element.className.Length > 0)
                    {
                        if (element.className.Contains(className))
                        {
                            return element;
                        }
                    }
                }
            }

            return null;
        }

        private static IHTMLElement secondElementForClass(IHTMLDocument2 doc, String id)
        {
            int i = 0;

            if (null != doc)
            {
                foreach (IHTMLElement element in doc.all)
                {
                    if (element.className != null && element.className.Length > 0)
                    {
                        if (element.className.Contains(id))
                        {
                            if (i >= 1)
                                return element;
                            else
                                i++;
                        }
                    }
                }
            }

            return null;
        }

        public static GoogleBook fetchBookForISBN(String isbn)
        {
            WebClient wc = new WebClient();
            String json = wc.DownloadString("https://www.googleapis.com/books/v1/volumes?q=" + isbn + "+isbn");

            GoogleBookResults parsed = JsonConvert.DeserializeObject<GoogleBookResults>(json);

            return parsed.items[0];
        }

        public static System.Drawing.Image fetchImage(string uri)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            // Check that the remote file was found. The ContentType
            // check is performed since a request for a non-existent
            // image file might be redirected to a 404-page, which would
            // yield the StatusCode "OK", even though the image was not
            // found.
            if ((response.StatusCode == HttpStatusCode.OK ||
                response.StatusCode == HttpStatusCode.Moved ||
                response.StatusCode == HttpStatusCode.Redirect) &&
                response.ContentType.StartsWith("image", StringComparison.OrdinalIgnoreCase))
            {

                // if the remote file was found, download oit
                using (Stream inputStream = response.GetResponseStream())
                //        using (Stream outputStream = File.OpenWrite(fileName))
                {
                    Bitmap bmp = new Bitmap(inputStream);
                    inputStream.Flush();
                    inputStream.Close();

                    return (System.Drawing.Image)bmp;
                }
            }

            return null;
        }

        public static UABook fetchUABook(String isbn)
        {
            IHTMLDocument2 doc = (IHTMLDocument2)new HTMLDocument();

            WebClient wc = new WebClient();
            doc.write(wc.DownloadString("http://sabio.library.arizona.edu/search/i?SEARCH=" + isbn + "&searchscope=9&SORT=D&extended=1&SUBMIT=Search"));

            IHTMLElement browse = elementForClass(doc, "browseList");

            if (browse != null)
                return null;

            IHTMLElement call = secondElementForClass(doc, "bibInfoData");

            return new UABook(call.innerHTML, isbn);
        }

        public static List<String> wikiBookISBNs(String wikiURL)
        {
            String html = (new WebClient()).DownloadString(wikiURL);
            Regex rx = new Regex(@"/wiki/Special:BookSources/(\d)*");

            Match m = rx.Match(html);

            List<String> ISBNs = new List<String>();

            while (m.Success)
            {
                ISBNs.Add(m.Value.Substring(26));
                m = m.NextMatch();
            }

            return ISBNs;
        }

        public static String fetchHTMLCitation(String isbn)
        {
            IHTMLDocument2 doc = (IHTMLDocument2)(new HTMLDocument());

            doc.write((new WebClient()).DownloadString("http://www.ottobib.com/search/searchisbn?search=" + isbn));

            IHTMLElement nahcontent = elementForID(doc, "nahcontent");
            String html = nahcontent.innerHTML;

            Regex rx = new Regex(@"</[Hh]3>.*");
            String result = rx.Match(html).Value;

            return (result != null && result.Length > 0 ? result.Substring(5) : null);
        }

        public ContentManager()
        {
        }
    }
}