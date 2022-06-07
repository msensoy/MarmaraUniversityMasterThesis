using Marmara.Common;
using Marmara.Data.Entity;
using Marmara.W1.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Marmara.W1.Controllers
{
    public class ScheduleController : Controller
    {
        public IActionResult Index()
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString(Const.Token_Login)))
            {
                return RedirectToAction("Login", "Account");
            }
            ViewData["tokenLogin"] = HttpContext.Session.GetString(Const.Token_Login);
            ViewData["userRole"] = HttpContext.Session.GetString(Const.Token_UserRole);

            var list = GetList();
            ViewBag.Hours = SelectLoadHours();
            return View(list.OrderByDescending(x => x.ScheduleTime).ToList());
        }
        public IEnumerable<SelectListItem> SelectLoadHours()
        {
            var list = new List<SelectListItem>();

            for (int i = 0; i < 24; i++)
            {
                for (int j = 0; j < 31; j += 30)
                {
                    var time = $"{i:D2}:{j:D2}";
                    var item = new SelectListItem { Text = time, Value = time };
                    list.Add(item);
                }
            }
            return list;
        }
        private List<SchTaskModel> GetList()
        {
            string url = Const.API_URL + "api/schedule/getlist";
            var response = Helper.RequestHelper.GetRequestAPIMethodString(url);
            var scheduleList = JsonConvert.DeserializeObject<List<SchTaskModel>>(response);

            return scheduleList;

        }
    }
}
