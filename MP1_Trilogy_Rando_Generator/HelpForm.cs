using DarkUI.Forms;
using System;
using System.IO;
using System.Net;
using System.Windows.Forms;

namespace MP1_Trilogy_Rando_Generator
{
    public partial class HelpForm : DarkForm
    {
        bool IsConnectedToInternet()
        {
            try
            {
                using (var client = new WebClientPlus())
                {
                    client.Timeout = (int)TimeSpan.FromSeconds(1.0).TotalMilliseconds;
                    using (client.OpenRead("http://google.com/generate_204"))
                        return true;
                }
            }
            catch
            {
                return false;
            }
        }

        public HelpForm()
        {
            InitializeComponent();
            if(IsConnectedToInternet())
                WikiHelper.DownloadWikiToCache();
            if (!File.Exists(".\\wiki\\index.html"))
                throw new Exception();
            webBrowser1.Url = new Uri(Directory.GetCurrentDirectory()+"\\wiki\\index.html");
        }

        private void NewURL(object sender, WebBrowserNavigatedEventArgs e)
        {
        }

        private void close_btn_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
