using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ArtMart.Controllers
{
    [Area("Admin")]
    public class AdminBaseController : Controller
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            ViewBag.SiteName = "Art Mart";
            ViewBag.SupportEmail = "support@artmart.com";
            var role = context.HttpContext.Session.GetString("Role");

            if (string.IsNullOrEmpty(role) || (role != "Admin" && role != "Artist"))
            {
                context.Result = new RedirectToActionResult("AccessDenied", "Auth", new { area = "" });
                return;
            }
            base.OnActionExecuting(context);
        }

    }
}
