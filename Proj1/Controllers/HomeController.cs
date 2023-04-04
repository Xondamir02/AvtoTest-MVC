using Microsoft.AspNetCore.Mvc;
using Proj1.Models;
using System.Diagnostics;


namespace Proj1.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }


        public IActionResult Privacy()
        {
            return View();
        }
        //public IActionResult Language(Language language)
        //{


        //    language.Id = HttpContext.Request.Cookies["UserId"];

        //    var user = UserService.Users.FirstOrDefault(u => u.Id == language.Id);
        //    user.LanguageJsonName = language.LanguageJsonName;

        //    return RedirectToAction("Index", "Home");


        //}           

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}