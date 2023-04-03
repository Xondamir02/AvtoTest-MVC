using Microsoft.AspNetCore.Mvc;

namespace Proj1.Controllers
{
    public class HttpMethodsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
