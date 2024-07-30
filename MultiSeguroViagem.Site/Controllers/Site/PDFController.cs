using System.Net;
using System.Web.Mvc;

namespace MultiSeguroViagem.Site.Controllers.Site
{
    public class PdfController : Controller
    {
        // GET: PDF
        public ActionResult Index(string uri)
        {

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
            var response = request.GetResponse();
            var stream = response.GetResponseStream();

            return new FileStreamResult(stream, response.ContentType);
        }
    }
}