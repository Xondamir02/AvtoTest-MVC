namespace Proj1.Models
{
    public class TicketQuestionAnswer
    {
        public int Id { get; set; }
        public int TicketId { get; set; }
        public int QuestionIndex { get; set; }
        public int ChoiceIndex { get; set; }
        public int CorrectIndex { get; set; }
        public bool IsCorrect => ChoiceIndex == CorrectIndex;
    }
}
