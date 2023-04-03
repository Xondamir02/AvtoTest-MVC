using Microsoft.Extensions.FileSystemGlobbing.Internal.PathSegments;

namespace AutoTestBot.Models
{
    class QuestionModel
    {
        public int Id { get; set; }
        public string Question { get; set; }
        public string Description { get; set; }
        public QuestionMedia Media { get; set; }
        public List<QuestionChoices> Choices { get; set; }
        public int CurrentTicketIndex { get; set; }
    }
}
