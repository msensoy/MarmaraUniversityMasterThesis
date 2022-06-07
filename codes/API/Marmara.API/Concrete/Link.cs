using System;
using System.Collections.Generic;
using System.Text;

namespace Marmara.API.Concrete
{
    public class Link
    {
        public Link(string rel, string href)
        {
            Rel = rel;
            Href = href;
        }
        public Link(string href)
        {
            Href = href;
        }
        public string Rel { get; set; }
        public string Href { get; set; }
    }
}
