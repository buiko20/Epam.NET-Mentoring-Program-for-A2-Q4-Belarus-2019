using System;

namespace Task2
{
    internal class SiteData
    {
        public SiteData(Guid guid, string uri)
        {
            Guid = guid;
            Uri = uri;
        }

        public Guid Guid { get; set; }

        public string Uri { get; set; }

        public string Html { get; set; }
    }
}
