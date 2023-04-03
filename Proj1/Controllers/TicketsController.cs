using AutoTestBot.Models;
using Microsoft.AspNetCore.Mvc;

using Proj1.Sevices;
using Proj1.Models;

namespace Proj1.Controllers
{
    public class TicketsController : Controller
    {


        
            public IActionResult Index()
            {
                var user = UserService.GetCurrentUser(HttpContext);

                return View(user);
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
