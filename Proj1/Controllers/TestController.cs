using Microsoft.AspNetCore.Mvc;

namespace Proj1.Controllers
{
    public class TestController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index(int a)
        {
            return View();
        }

        public IActionResult Add()
        {
            //logic

            return RedirectToAction("Index");
        }
    }
}
