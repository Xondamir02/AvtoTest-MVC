using AutoTestBot.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Proj1.Models;
using Proj1.Sevices;

namespace Proj1.Controllers
{


    public class QuestionsController : Controller
    {
        public IActionResult Index()
        {
            ViewBag.Questions = QuestionService.Instance.Questions;

            return View();
        }

        public IActionResult GetQuestionById(int id, int? ticketIndex = null, int? choiceIndex = null)
        {
            if (!UserService.IsLoggedIn(HttpContext))
                return RedirectToAction("SignIn", "Users");

            Models.User user = UserService.GetCurrentUser(HttpContext);

            if (ticketIndex != null)
            {
                HttpContext.Response.Cookies.Append("CurrentTicketIndex", ticketIndex.ToString());
                HttpContext.Response.Cookies.Delete("CorrectAnswersCount");
            }
            else if (HttpContext.Request.Cookies.ContainsKey("CurrentTicketIndex"))
            {
                ticketIndex = Convert.ToInt32(HttpContext.Request.Cookies["CurrentTicketIndex"]);
            }

            if (id > ticketIndex * 10 + 10)
            {
                var correctCount = Convert.ToInt32(HttpContext.Request.Cookies["CorrectAnswersCount"]);

                HttpContext.Response.Cookies.Delete("CurrentTicketIndex");
                HttpContext.Response.Cookies.Delete("CorrectAnswersCount");

                if (user is not null)
                {
                    var userTicket = user.UserTickets.FirstOrDefault(t => t.Index == ticketIndex);
                    userTicket.CorrectCount = correctCount;
                }

                return RedirectToAction(nameof(Result), "Questions",
                    new
                    {
                        ticketIndex = ticketIndex,
                        correctCount = correctCount
                    });
            }

            var question = QuestionService.Instance.Questions?.FirstOrDefault(x => x.Id == id);

            if (question == null)
            {
                ViewBag.QuestionId = id;
                ViewBag.IsSuccess = false;
            }
            else
            {
                ViewBag.Question = question;
                ViewBag.IsSuccess = true;
                ViewBag.Ticket = user.UserTickets.FirstOrDefault(t => t.Index == ticketIndex);
                ViewBag.IsAnswer = choiceIndex != null;

                if (choiceIndex != null)
                {
                    var answer = question.Choices[(int)choiceIndex].Answer;
                    ViewBag.IsCorrectAnswer = answer;
                    ViewBag.ChoiceIndex = choiceIndex;

                    var userTicket = user.UserTickets.FirstOrDefault(t => t.Index == ticketIndex);
                    
                    userTicket.Answers.Add(new QuestionAnswer()
                    {
                        ChoiceIndex = choiceIndex.Value,
                        QuestionIndex = id,
                        CorrectIndex = question.Choices.IndexOf(question.Choices.First(c => c.Answer))
                    });

                    if (answer)
                    {
                        if (HttpContext.Request.Cookies.ContainsKey("CorrectAnswersCount"))
                        {
                            var index = Convert.ToInt32(HttpContext.Request.Cookies["CorrectAnswersCount"]);

                            HttpContext.Response.Cookies.Append("CorrectAnswersCount", $"{index + 1}");
                        }
                        else
                        {
                            HttpContext.Response.Cookies.Append("CorrectAnswersCount", "1");
                        }
                    }
                }
            }

            return View(user);
        }

        public IActionResult Result(int ticketIndex, int correctCount)
        {
            ViewBag.TicketIndex = ticketIndex;
            ViewBag.CorrectCount = correctCount;

            return View();
        }
    }
}

