using AutoTestBot.Models;
using Newtonsoft.Json;

namespace Proj1.Sevices
{
    public class QuestionService
    {
        private static QuestionService? _instance;

        public static QuestionService Instance => _instance ??= new QuestionService();

        public List<QuestionModel> Questions;

        public int TicketQuestionsCount => 10;
        public int TicketsCount => Questions.Count / TicketQuestionsCount;

        public QuestionService()
        {
            LoadJson("uz");

            Questions ??= new List<QuestionModel>();
        }

        public void LoadJson(string language)
        {
            var jsonPath = "uzlotin.json";

            switch (language)
            {
                case "uz": jsonPath = "uzlotin.json"; break;
                case "uzc": jsonPath = "uzkiril.json"; break;
                case "ru": jsonPath = "rus.json"; break;
            }

            var path = Path.Combine("JsonData", jsonPath);

            if (File.Exists(path))
            {
                var json = System.IO.File.ReadAllText(path);
                Questions = JsonConvert.DeserializeObject<List<QuestionModel>>(json)!;
            }
        }
    }
}