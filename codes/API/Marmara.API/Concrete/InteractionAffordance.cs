using System;
using System.Collections.Generic;
using System.Text;

namespace Marmara.API.Concrete
{
    public abstract class InteractionAffordance
    {
        public string Type { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Unit { get; set; }
        public object Value { get; set; }
        public List<Link> Links { get; set; }


        //"links": [{"href": "/things/lamp/actions/fade"}]

        //     "brightness" : {
        //  "@type": "BrightnessProperty",
        //  "type": "integer",
        //  "title": "Brightness",
        //  "description": "The level of light from 0-100",
        //  "minimum" : 0,
        //  "maximum" : 100,
        //  "links": [{"href": "/things/lamp/properties/brightness"}]
        //}
    }
}
