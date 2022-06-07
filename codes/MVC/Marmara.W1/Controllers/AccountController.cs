using Marmara.Common;
using Marmara.Common.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using static Marmara.Common.Helper;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System;

namespace Marmara.W1.Controllers
{
    public class AccountController : Controller
    {
        [HttpGet]
        public IActionResult Login()
        {
            if (!string.IsNullOrEmpty(HttpContext.Session.GetString(Const.Token_UserRole)))
            {
                HttpContext.Session.SetString(Const.Token_UserRole, "");
            }
            if (!string.IsNullOrEmpty(HttpContext.Session.GetString(Const.Token_Login)))
            {
                HttpContext.Session.SetString(Const.Token_Login, "");
                HttpContext.Session.SetString(Const.Token_Username, "");
            }
            return View();
        }
          
        [HttpPost]
        public IActionResult Login(LoginModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var tokenLogin = RequestHelper.GetTokenOnLoginUrl(model);
           
            if (string.IsNullOrEmpty(tokenLogin))
            {
                ModelState.AddModelError("", Message.AccountIncorrectEntry);
                return View(model);
            }

            HttpContext.Session.SetString(Const.Token_Login, tokenLogin);
            var stream = tokenLogin;
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(stream);
            var tokenS = jsonToken as JwtSecurityToken;

            if (model.Username == Const.admin)
            {
                HttpContext.Session.SetString(Const.Token_UserRole, Const.admin);
            }

            HttpContext.Session.SetString(Const.Token_Username, model.Username);
        
                return RedirectToAction("Index", "Home");
        }


        [HttpGet]
        public IActionResult Profile()
        {
            var userName = HttpContext.Session.GetString(Const.Token_Username);
            var appUserId =  RequestHelper.GetRequestForData(string.Format(@"{0}api/Authenticate/getUserId?username={1}", Const.API_URL, userName));
            var model = new SettingPasswordModel() { AppUserGuid = appUserId };
            ViewData["tokenLogin"] = HttpContext.Session.GetString(Const.Token_Login);
            ViewData["userRole"] = HttpContext.Session.GetString(Const.Token_UserRole);
            return View(model);
        }

        [HttpPost]
        public IActionResult Profile(SettingPasswordModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Message = Message.CheckChangePassword;
                return View();
            }
            var requestModel = JsonConvert.SerializeObject(model);
            string url = string.Format(@"{0}api/Authenticate/changePassword", Const.API_URL);
            var response = Helper.RequestHelper.PostRequestAPIMethod(url, requestModel);
            return Redirect("~/");
        }


        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }


    
    }
}
