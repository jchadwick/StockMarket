using System.Web.Mvc;

namespace Website.Controllers
{
    public class StockMarketController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Message = "Welcome to ASP.NET MVC!";

            return View();
        }
    }
}
