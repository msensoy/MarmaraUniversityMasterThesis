using Marmara.Common;
using Marmara.Common.ThingClass;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using static Marmara.Common.Helper;

namespace Marmara.W1.Controllers
{
    public class LightingController : Controller
    {
        public IActionResult Index()
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString(Const.Token_Login)))
            {
                return RedirectToAction("Login", "Account");
            }
            ViewData["tokenLogin"] = HttpContext.Session.GetString(Const.Token_Login);
            ViewData["userRole"] = HttpContext.Session.GetString(Const.Token_UserRole);
            List<Thing> thingsDescription = RequestHelper.GetRequestAPIMethod(Const.API_URL);
            var thingsDescriptionLighting = thingsDescription.Where(x => x.System == Const.Lighting).ToList();
            return View(thingsDescriptionLighting);
        }
    }
}
