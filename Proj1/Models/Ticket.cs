using Proj1.Models;

namespace AutoTestBot.Models;

public class Ticket
{
    public int Id { get; set; }
    public string? UserId { get; set; }
    public int QuestionsCount { get; set; }
    public int StartIndex { get; set; }
    public int CurrentQuestionIndex { get; set; }

    public bool IsCompleted => QuestionsCount == Answers.Count;

    public List<TicketQuestionAnswer> Answers { get; set; } = new List<TicketQuestionAnswer>();

    public int CorrectCount => Answers.Count(answer => answer.IsCorrect);

    public DateTime Date { get; set; }

    public Ticket()
    {

    }

    public Ticket(int index, int questionsCount)
    {
        Id = index;
        QuestionsCount = questionsCount;
        StartIndex = index * questionsCount;
        CurrentQuestionIndex = StartIndex;
    }
}
