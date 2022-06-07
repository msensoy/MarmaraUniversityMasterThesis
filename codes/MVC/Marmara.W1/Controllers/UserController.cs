using Marmara.Common;
using Marmara.Common.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using static Marmara.Common.Helper;
namespace Marmara.W1.Controllers
{
    public class UserController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            var model = new List<ControlUserModel>();
            model = JsonConvert.DeserializeObject<List<ControlUserModel>>(RequestHelper.GetRequestForData(string.Format(@"{0}api/Authenticate/GetUserList", Const.API_URL)));
            ViewData["tokenLogin"] = HttpContext.Session.GetString(Const.Token_Login);
            ViewData["userRole"] = HttpContext.Session.GetString(Const.Token_UserRole);
            return View(model);
        }

        [HttpGet]
        public JsonResult DeleteUser(int id)
        {
            var data = RequestHelper.GetRequestForData(string.Format(@"{0}api/Authenticate/deleteUser?id={1}", Const.API_URL,id));
            return Json(Convert.ToInt32(data));
        }


        [HttpPost]
        public JsonResult RegisterNewUser(RegisterModel model)
        {
            var requestModel = JsonConvert.SerializeObject(model);
            string url = string.Format(@"{0}api/Authenticate/Register", Const.API_URL);
            var response = Helper.RequestHelper.PostRequestAPIMethod(url, requestModel);
            return Json(response);
        }  
        
        


    }
}
