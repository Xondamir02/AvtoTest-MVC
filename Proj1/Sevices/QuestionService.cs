using AutoTestBot.Models;
using Newtonsoft.Json;

namespace Proj1.Sevices
{
    public class QuestionService
    {
        private static QuestionService? _instance;
        public static QuestionService Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new QuestionService();
                }

                return _instance;
            }
        }

        public readonly List<QuestionModel>? Questions;

        public QuestionService()
        {
            var path = Path.Combine("JsonData", "uzlotin.json");
            var json = System.IO.File.ReadAllText(path);

            Questions = JsonConvert.DeserializeObject<List<QuestionModel>>(json);
        }
    }
}