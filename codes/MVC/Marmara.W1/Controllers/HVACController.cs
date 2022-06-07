using Marmara.Common;
using Marmara.Common.ThingClass;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using static Marmara.Common.Helper;


namespace Marmara.W1.Controllers
{
    public class HVACController : Controller
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
            var thingsDescriptionHVAC= thingsDescription.Where(x => x.System == Const.HVAC).ToList();
            return View(thingsDescriptionHVAC);
        }
    }
}
