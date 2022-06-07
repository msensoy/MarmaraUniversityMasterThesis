using Marmara.API.Things;
using Marmara.API.Concrete;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using static Marmara.Common.Helper;
using Microsoft.AspNetCore.Authorization;

namespace Marmara.API.Controllers
{
    [Authorize]
    [Route("/")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        [HttpGet]
        public IActionResult Index()
        {
            List<Thing> things = ThingModelHelper.ReflectiveEnumerator.GetEnumerableOfType<Thing>();
            var model = ThingModelHelper.GetThingsJsonModel(things);
            return Ok(model);
        }
    }
}

