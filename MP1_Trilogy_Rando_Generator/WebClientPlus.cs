namespace System.Net
{
    class WebClientPlus : WebClient
    {
        public int Timeout = 100 * 60 * 1000;

        protected override WebRequest GetWebRequest(Uri uri)
        {
            WebRequest w = base.GetWebRequest(uri);
            w.Timeout = this.Timeout;
            return w;
        }
    }
}
