using AutoTestBot.Models;

namespace Proj1.Models
{
    public class TicketQuestionAnswer
    {
        public int QuestionIndex { get; set; }
        public int ChoiceIndex { get; set; }
        public int CorrectIndex { get; set; }
        public bool IsCorrect => ChoiceIndex == CorrectIndex;
    }
}
