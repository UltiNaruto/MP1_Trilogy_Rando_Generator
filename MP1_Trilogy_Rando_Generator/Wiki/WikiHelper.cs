using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;

namespace MP1_Trilogy_Rando_Generator
{
    class WikiHelper
    {
        static String homePage = "https://github.com/UltiNaruto/MP1_Trilogy_Rando_Generator/wiki";
        static String[] GetAllURLs()
        {
            var URLList = default(List<String>);
            var homeHTML = default(String);
            var reg_result = default(MatchCollection);

            URLList = new List<String>();

            using (var client = new WebClientPlus())
            {
                homeHTML = client.DownloadString(homePage);
                reg_result = Regex.Matches(homeHTML, @"href=\""(.*?)\""", RegexOptions.Singleline);
                foreach (Match match in reg_result)
                    if(match.Success &&
                      !URLList.Contains("https://github.com" + match.Groups[1].Value) &&
                       match.Groups[1].Value.StartsWith("/UltiNaruto/MP1_Trilogy_Rando_Generator/wiki") &&
                      !match.Groups[1].Value.EndsWith("/Home/_history"))
                        URLList.Add("https://github.com"+match.Groups[1].Value);
            }

            URLList.Reverse();

            return URLList.ToArray();
        }

        static String RemoveTag(String HTML, String tag)
        {
            int tag_level = 0;
            int index = HTML.IndexOf(tag);
            int i = 0;
            String previousTag = "";
            String firstTag = "";

            if (index == -1)
                return HTML;
            firstTag = HTML.Substring(index + 1, HTML.IndexOf(' ', index+1) - (index + 1));
            for (i = index + tag.Length; i<HTML.Length - 2; i++)
            {
                if (tag_level >= 0)
                {
                    if (HTML.Substring(i, 2) == "</" || HTML.Substring(i, 2) == "/>")
                    {
                        tag_level--;
                    }
                    else if (HTML.Substring(i, 1) == "<" &&
                        !(HTML.Substring(i, 2) == "</" ||
                        HTML.Substring(i, 4) == "<!--"))
                    {
                        previousTag = HTML.Substring(i + 1, HTML.IndexOf(' ', i + 1) - (i + 1));
                        if (previousTag != "input")
                            tag_level++;
                    }
                }
                else
                {
                    if(HTML.Substring(i-1, 2+ firstTag.Length) == "</"+ firstTag)
                    {
                        i += 3 + firstTag.Length;
                        break;
                    }
                }
            }
            return HTML.Substring(0, index) + HTML.Substring(i - 1, HTML.Length - (i - 1));
        }

        public static void DownloadWikiToCache()
        {
            String[] URLs = GetAllURLs();
            String HTML = "";
            String FileName = "";
            String _FileName = "";

            if (!Directory.Exists(".\\wiki"))
                Directory.CreateDirectory(".\\wiki");

            using (var client = new WebClientPlus())
            {
                foreach (var URL in URLs)
                {
                    FileName = URL.Substring(URL.LastIndexOf("/")+1);
                    if (FileName == "wiki")
                        FileName = "Home";
                    HTML = client.DownloadString(URL);
                    HTML = RemoveTag(HTML, "<div class=\"position-relative js-header-wrapper \">");
                    HTML = RemoveTag(HTML, "<div class=\"bg-gray-light pt-3 hide-full-screen mb-5\">");
                    HTML = RemoveTag(HTML, "<div class=\"footer container-xl width-full p-responsive\" role=\"contentinfo\">");
                    HTML = RemoveTag(HTML, "<div class=\"Box-header js-wiki-toggle-collapse\" style=\"cursor: pointer\">");
                    HTML = RemoveTag(HTML, "<div class=\"filter-bar\">");
                    HTML = RemoveTag(HTML, "<h5 class=\"mt-0 mb-2\">");
                    HTML = RemoveTag(HTML, "<div class=\"width-full input-group\">");
                    HTML = RemoveTag(HTML, "<a href=\"/UltiNaruto/MP1_Trilogy_Rando_Generator/wiki/" + FileName + "/_history\" class=\"muted-link\">");
                    HTML = RemoveTag(HTML, "<div class=\"mt-0 mt-lg-1 flex-shrink-0 gh-header-actions\">");
                    HTML = RemoveTag(HTML, "<template class=\"js-flash-template\">");
                    HTML = RemoveTag(HTML, "<nav aria-label=\"Repository\" data-pjax=\"#js-repo-pjax-container\" class=\"js-repo-nav js-sidenav-container-pjax js-responsive-underlinenav overflow-hidden UnderlineNav px-3 px-md-4 px-lg-5 bg-gray-light\">");
                    foreach (var _URL in URLs)
                    {
                        _FileName = _URL.Substring(_URL.LastIndexOf("/") + 1);
                        if (_FileName == "wiki")
                            _FileName = "Home";
                        HTML = HTML.Replace(_URL.Substring(18), _FileName == "Home" ? "./index.html" : "./" + _FileName + ".html");
                    }
                    HTML = HTML.Replace("<body class=\"logged-out env-production page-responsive\" style=\"word-wrap: break-word;\">", "<body bgcolor=\"#000\" style=\"color: #AAA\" class=\"logged-out env-production page-responsive\" style=\"word-wrap: break-word;\">");
                    File.WriteAllText(FileName == "Home" ? ".\\wiki\\index.html" : ".\\wiki\\" + FileName + ".html", HTML);
                }
            }
        }
    }
}
