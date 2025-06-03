using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ArtMart.Controllers
{
    public class BaseController : Controller
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            ViewBag.SiteName = "Art Mart";
            ViewBag.SupportEmail = "support@artmart.com";

            base.OnActionExecuting(context);
        }

    }
}
