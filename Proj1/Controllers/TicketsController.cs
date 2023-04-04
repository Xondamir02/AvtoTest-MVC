using Microsoft.AspNetCore.Mvc;
using Proj1.Models;
using Proj1.Sevices;

namespace Proj1.Controllers
{
    public class TicketsController : Controller
    {
        public IActionResult Index()
        {
            var user = UserService.GetCurrentUser(HttpContext);

            if (user == null)
                return RedirectToAction("SignIn", "Users");

            return View(user);
        }

        public IActionResult StartTicket(int ticketIndex)
        {
            var user = UserService.GetCurrentUser(HttpContext);

            if (user == null)
                return RedirectToAction("SignIn", "Users");

            if (QuestionService.Instance.TicketsCount <= ticketIndex)
                return View("NotFound");

            user.CurrentTicketIndex = ticketIndex;
            user.CurrentTicket!.Date = DateTime.Now;

            return RedirectToAction("Questions", new { id = user.CurrentTicket?.StartIndex });
        }

        public IActionResult Questions(int id, int? choiceIndex = null)
        {
            User? user = UserService.GetCurrentUser(HttpContext);
            if (user == null)
                return RedirectToAction("SignIn", "Users");

            if (user.CurrentTicketIndex == null)
                return RedirectToAction("Index");

            if (id > user.CurrentTicketIndex * 10 + 10)
            {
                return RedirectToAction(nameof(Result));
            }

            var question = QuestionService.Instance.Questions?.FirstOrDefault(x => x.Id == id);

            if (question == null)
                return View("NotFound");

            ViewBag.Question = question;
            ViewBag.IsAnswer = choiceIndex != null;

            if (choiceIndex != null)
            {
                var answer = new TicketQuestionAnswer()
                {
                    ChoiceIndex = choiceIndex.Value,
                    QuestionIndex = id,
                    CorrectIndex = question.Choices.IndexOf(question.Choices.First(c => c.Answer))
                };

                user.CurrentTicket!.Answers.Add(answer);

                ViewBag.Answer = answer;
            }

            return View(user);
        }

        public IActionResult Result()
        {
            var user = UserService.GetCurrentUser(HttpContext);

            if (user == null)
                return RedirectToAction("SignIn", "Users");

            return View(user);
        }
        public IActionResult CheckUser()
        {

            if (UserService.IsLoggedIn(HttpContext))
            {
                var user = UserService.GetCurrentUser(HttpContext);


                if (user == null)
                    return View();

                return RedirectToAction("Index", "Tickets");
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

