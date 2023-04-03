using AutoTestBot.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Proj1.Sevices;
using Proj1.Models;

namespace Proj1.Controllers
{
    public class TicketsController : Controller
    {

        private readonly List<QuestionModel> _questions;
        public TicketsController()
        {
            //var context = HttpContext.Request.Cookies["UserId"];

            //if (HttpContext.Request.Cookies.ContainsKey("UserId"))
            //{
            //    string userId = HttpContext.Request.Cookies["UserId"];
            //    User user = UserService.Users.FirstOrDefault(u => u.Id == userId);
            //    user.LanguageJsonName = HttpContext.Request.Cookies["Language"];

            //    string path = Path.Combine("JsonData", $"{user.LanguageJsonName}");
            //    var json = System.IO.File.ReadAllText(path);
            //    _questions = JsonConvert.DeserializeObject<List<QuestionModel>>(json);
            //}
            //else
            //{}
                string path = Path.Combine("JsonData", "uzlotin.json");
                var json = System.IO.File.ReadAllText(path);
                _questions = JsonConvert.DeserializeObject<List<QuestionModel>>(json);
            
        }
        public IActionResult Index()
        {

            ViewBag.TicketsCount = _questions.Count / 10;

            return View();
        }
        public IActionResult CheckUser()
        {
            if (HttpContext.Request.Cookies.ContainsKey("UserId"))
            {
                var userId = HttpContext.Request.Cookies["UserId"];
                var user = UserService.Users.FirstOrDefault(u => u.Id == userId);


                if (user == null)
                return View();

                return RedirectToAction("Index","Tickets");
            }

            return View();
        }

        //public IActionResult Oddiy(string til)
        //{
            
        //    HttpContext.Response.Cookies.Append("Language", til);

        //    return RedirectToAction("Index","Home");
        //}


    }
}
