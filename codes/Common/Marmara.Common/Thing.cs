using System;
using System.Collections.Generic;
using System.Text;

namespace Marmara.Common.ThingClass
{
    public class Thing
    {
        public string Name { get; set; }
        public string BaseName { get; set; }
        public string BaseId { get; set; }


        public string Context { get; set; }

        public string Type { get; set; }

        public string Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }


        public IEnumerable<Property> Properties { get; set; }


        public IEnumerable<Action> Actions { get; set; }


        public IEnumerable<Event> Events { get; set; }

        public IEnumerable<Link> Links { get; set; }
        public string System { get; set; }

    }

    public class Link
    {
        public Link(string rel, string href)
        {
            Rel = rel;
            Href = href;
        }
        public string Rel { get; set; }
        public string Href { get; set; }
    }
    public class Event : InteractionAffordance
    {
    }
    public class Action : InteractionAffordance
    {
    }
    public class Property : InteractionAffordance
    {

    }
    public abstract class InteractionAffordance
    {
        public string Type { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Unit { get; set; }
        public object Value { get; set; }
        public List<Link> Links { get; set; }

    }

}
