using System.Web.Mvc;

namespace MultiSeguroViagem.Site.Controllers.Site
{
    public class QuatroZeroQuatroController : Controller
    {
        [HandleError]
        public ActionResult Index()
        {
            Response.StatusCode = 404;
            Response.TrySkipIisCustomErrors = true;

            return View("404");
        }
    }
}