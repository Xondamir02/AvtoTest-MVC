using Microsoft.AspNetCore.Mvc;
using Proj1.Models;
using Proj1.Sevices;

namespace Proj1.Controllers
{
    public class TicketsController : Controller
    {

        private readonly UserService _usersService;
        private readonly QuestionService _questionService;

        public TicketsController(UserService usersService, QuestionService questionService)
        {
            _usersService = usersService;
            _questionService = questionService;
        }

        public IActionResult Index()
        {
            var user = _usersService.GetCurrentUser(HttpContext);

            if (user == null)
                return RedirectToAction("SignIn", "Users");

            return View(user);
        }

        public IActionResult StartTicket(int ticketIndex)
        {
            var user = _usersService.GetCurrentUser(HttpContext);

            if (user == null)
                return RedirectToAction("SignIn", "Users");

            if (_questionService.TicketsCount <= ticketIndex)
                return View("NotFound");

            user.CurrentTicketIndex = ticketIndex;
            //user.CurrentTicket!.Date = DateTime.Now;
            user.CurrentTicketIndex = ticketIndex;
            user.CurrentTicket = user.CurrentTicketIndex == null ? null: _usersService._ticketRepository.GetTicket(user.CurrentTicketIndex.Value);
            _usersService._userRepository.UpdateUser(user);

            return RedirectToAction("Questions", new { id = user.CurrentTicket?.StartIndex });
        }

        public IActionResult Questions(int id, int? choiceIndex = null)
        {
            User? user = _usersService.GetCurrentUser(HttpContext);
            if (user == null)
                return RedirectToAction("SignIn", "Users");

            if (user.CurrentTicketIndex == null)
                return RedirectToAction("Index");

            if (id > user.CurrentTicketIndex * 10 + 10)
            {
                return RedirectToAction(nameof(Result));
            }

            var question = _questionService.Questions?.FirstOrDefault(x => x.Id == id);

            if (question == null)
                return View("NotFound");

            ViewBag.Question = question;
            ViewBag.IsAnswer = choiceIndex != null;

            if (choiceIndex != null)
            {
                var answer = new TicketQuestionAnswer()
                {
                    TicketId = user.CurrentTicketIndex.Value,
                    ChoiceIndex = choiceIndex.Value,
                    QuestionIndex = id,
                    CorrectIndex = question.Choices!.IndexOf(question.Choices.First(c => c.Answer))
                };

                //user.CurrentTicket!.Answers.Add(answer);
                _usersService._ticketRepository.AddTicketAnswer(answer);

                ViewBag.Answer = answer;
            }

            return View(user);
        }

        public IActionResult Result()
        {
            var user = _usersService.GetCurrentUser(HttpContext);

            if (user == null)
                return RedirectToAction("SignIn", "Users");

            return View(user);
        }
        public IActionResult CheckUser()
        {

            if (_usersService.IsLoggedIn(HttpContext))
            {
                var user = _usersService.GetCurrentUser(HttpContext);


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

