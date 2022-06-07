using Marmara.Common;
using Marmara.Common.Model;
using Marmara.W1.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Marmara.W1.Controllers
{
    public class RuleController : Controller
    {
        public IActionResult Index()
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString(Const.Token_Login)))
            {
                return RedirectToAction("Login", "Account");
            }
            ViewData["tokenLogin"] = HttpContext.Session.GetString(Const.Token_Login);
            ViewData["userRole"] = HttpContext.Session.GetString(Const.Token_UserRole);

            string url = Const.API_URL + "api/rule/getRules";
            var response = Helper.RequestHelper.GetRequestAPIMethodString(url);
            var ruleList = JsonConvert.DeserializeObject<List<RuleModel>>(response);
            var model = new RuleTaskModel();

            model.checkTemp = ruleList.FirstOrDefault(x => x.Name == Const.TEMPERATURE).IsActive;
            model.checkLux = ruleList.FirstOrDefault(x => x.Name == Const.LUX).IsActive;

            model.valueTemp = ruleList.FirstOrDefault(x => x.Name == Const.TEMPERATURE).Value;
            model.valueLux = ruleList.FirstOrDefault(x => x.Name == Const.LUX).Value;

            model.radioTempGreaterThen = !ruleList.FirstOrDefault(x => x.Name == Const.TEMPERATURE).IsGreaterThen;
            model.radioLuxGreaterThen = !ruleList.FirstOrDefault(x => x.Name == Const.LUX).IsGreaterThen;

            return View(model);
        }

        [HttpPost]
        public IActionResult Save(RuleTaskModel model)
        {
            int valueActiveTemp = model.checkTemp ? 1 : 0;
            int valueActiveLux = model.checkLux ? 1 : 0;

            int valueLuxGreaterThen= !model.radioLuxGreaterThen ? 1 : 0;

            var tempRule = new RuleModel()
            {
                Name = Const.TEMPERATURE,
                ValueActive = valueActiveTemp,
                Value = model.valueTemp
            };
            var luxRule = new RuleModel()
            {
                Name = Const.LUX,
                ValueActive = valueActiveLux,
                ValueGreaterThen= valueLuxGreaterThen,
                Value = model.valueLux
            };   
         
            var list = new List<RuleModel>();
            list.Add(tempRule);
            list.Add(luxRule);

            var requestModel = JsonConvert.SerializeObject(list);
            string url = Const.API_URL + "api/rule/save";
            var response = Helper.RequestHelper.PostRequestAPIMethod(url, requestModel);
            return RedirectToAction("Index"); 
        }
    }
}
