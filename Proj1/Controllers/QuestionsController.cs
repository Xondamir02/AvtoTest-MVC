using AutoTestBot.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Proj1.Models;
using Proj1.Sevices;

namespace Proj1.Controllers
{
    public class QuestionsController : Controller
    {
        private List<QuestionModel>? _questions;
        public QuestionsController()
        {

            var path = Path.Combine("JsonData", "uzlotin.json");
            var json = System.IO.File.ReadAllText(path);
            _questions = JsonConvert.DeserializeObject<List<QuestionModel>>(json);

        }

        private IActionResult Index()
        {
            ViewBag.Questions = _questions;

            return View();
        }

        public IActionResult GetQuestionById(int id, int? choiceIndex = null, int? ticketIndex = null)
        {
            if (ticketIndex != null)
            {
                HttpContext.Response.Cookies.Append("CurrentTicketIndex", ticketIndex.ToString());
            }
            if (HttpContext.Request.Cookies.ContainsKey("CurrentTicketIndex"))
            {
                var index = Convert.ToInt32(HttpContext.Request.Cookies["CurrentTicketIndex"]);
                if (id > index * 10 + 10)

                {
                    var correctCount = Convert.ToInt32(HttpContext.Request.Cookies["CorrectAnswersCount"]);
                    HttpContext.Response.Cookies.Delete("CorrectAnswersCount");
                    HttpContext.Response.Cookies.Delete("CurrentTicketIndex");

                    var user = UserService.GetCurrentUser(HttpContext);
                    if (user is not null)
                    {
                        user.TicketResults.Add(new TicketResult()
                        {
                            CorrectCount = correctCount,
                            QuestionCount = 10,
                            TicketIndex = index
                        });
                    }

                    return RedirectToAction(nameof(Result),new {ticketIndex=index,correctCount=correctCount});
                }

            }

            var question = _questions?.FirstOrDefault(x => x.Id == id);
            if (question == null)
            {
                ViewBag.QuestionId = id;
                ViewBag.IsSuccess = false;
            }
            else
            {
                ViewBag.Question = question;
                ViewBag.IsSuccess = true;

                ViewBag.IsAnswer = choiceIndex != null;
                if (choiceIndex != null)
                {
                    var answer = question.Choices[(int)choiceIndex].Answer;
                    ViewBag.Answer = answer;
                    ViewBag.ChoiceIndex = choiceIndex;
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
            return View();
        }
        public IActionResult Result(int ticketIndex,int correctCount)
        {
            ViewBag.TicketIndex = ticketIndex;
            ViewBag.CorrectCount = correctCount;
            return View();
        }

    }
}
