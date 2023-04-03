using Microsoft.AspNetCore.Mvc;

namespace Proj1.Controllers
{
    public class StatisticsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
