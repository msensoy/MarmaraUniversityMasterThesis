using Marmara.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
namespace Marmara.W1.Controllers
{

    public class SwaggerController : Controller
    {
        public IActionResult Index()
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString(Const.Token_Login)))
            {
                return RedirectToAction("Login", "Account");
            }
            ViewData["tokenLogin"] = HttpContext.Session.GetString(Const.Token_Login);
            ViewData["userRole"] = HttpContext.Session.GetString(Const.Token_UserRole);
            string swaggerUrl = Const.API_URL + "swagger/index.html";
            return Redirect(swaggerUrl);
        }
    }
}
