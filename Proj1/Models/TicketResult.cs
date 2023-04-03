using AutoTestBot.Models;

namespace Proj1.Models
{
    public class UserTickets :Ticket
    {

        public List<QuestionAnswer> Answers { get; set; } = new();
        public DateTime DateTime { get; set; }

    }
    public class QuestionAnswer
    {
        public int QuestionIndex { get; set;}
        public int ChoiceIndex { get; set;}
        public int CorrectIndex { get; set;}
    }
}
