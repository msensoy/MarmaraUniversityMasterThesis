using Marmara.Common;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Marmara.API.Concrete
{
    public abstract class Thing
    {
        public Thing()
        {
            security = new Security();
        }

        [JsonIgnore]
        public string BaseId => "https://localhost.com/things/";

        [JsonProperty(Order = 1)]
        public Security security { get; set; }

        [JsonProperty("@context", Order = 2)]
        public string Context => "https://webthings.io/schemas/";

        //[JsonProperty("@type", Order = 3)]
        [JsonProperty(Order = 3)]
        public abstract string Type { get; }

        [JsonProperty(Order = 4)]
        public abstract string Id { get; }

        [JsonProperty(Order = 5)]
        public abstract string Title { get; }

        [JsonProperty(Order = 6)]
        public abstract string Description { get; }

        [JsonProperty(Order = 7)]
        public abstract string System { get; }

        [JsonProperty(Order = 8)]
        public virtual IEnumerable<Property> Properties { get; set; }

        [JsonProperty(Order = 9)]
        public virtual IEnumerable<Action> Actions { get; set; }

        [JsonProperty(Order = 10)]
        public virtual IEnumerable<Event> Events { get; set; }

        [JsonProperty(Order = 11)]
        public virtual IEnumerable<Link> Links => SetLinks();

        private IEnumerable<Link> SetLinks()
        {
            var list = new List<Link>();
            var link1 = new Link(Const.properties, Id + "/properties");
            var link2 = new Link(Const.actions, Id + "/actions");
            var link3 = new Link(Const.events, Id + "/events");
            list.Add(link1);
            list.Add(link2);
            list.Add(link3);
            return list;
        }
    }

    public class Security
    {
        public string authorizationUrl { get; set; } = "/api/authenticate/login";
        public string scheme { get; set; } = "bearer";
        public string format { get; set; } = "JWT";
    }
}