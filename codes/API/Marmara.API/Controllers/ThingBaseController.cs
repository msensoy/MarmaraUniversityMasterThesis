using Microsoft.AspNetCore.Mvc;
using static Marmara.Common.Helper;
using Marmara.API.Concrete;
using System.Collections.Generic;

namespace Marmara.API.Controllers
{
    public class ThingBaseController : ControllerBase
    {
        //public  IHubContext<Hub> _hubContext;
        public Thing thing { get; set; }

        [HttpGet]
        public IActionResult Index()
        {
            var model = ThingModelHelper.GetThingJsonModel(thing);
            return Ok(model);
        }

        [HttpPost("actions")]
        public IActionResult actions()
        {
            return Ok(thing.Actions ?? new List<Action>());
        }

        [HttpPost("properties")]
        public IActionResult properties()
        {
            return Ok(thing.Properties ?? new List<Property>());
        }
        [HttpPost("events")]
        public IActionResult events()
        {
            return Ok(thing.Events ?? new List<Event>());
        }
    }
}
