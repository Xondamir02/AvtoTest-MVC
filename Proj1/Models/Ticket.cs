namespace AutoTestBot.Models
{
    class Ticket
    {
        public int Index { get; set; }
        public int CorrectCount { get; set; }
        public int QuestionsCount { get; set; }
        public int StartIndex { get; set; }
        public int CurrentQuestionIndex { get; set; }

        public bool IsCompleted
        {
            get
            {
                return StartIndex + QuestionsCount <= CurrentQuestionIndex;
            }
        }

        public Ticket()
        {

        }

        public Ticket(int index, int questionsCount)
        {
            Index = index;
            QuestionsCount = questionsCount;
            StartIndex = index * questionsCount;
            CurrentQuestionIndex = StartIndex;
            CorrectCount = 0;
        }

        public void SetDefault()
        {
            CurrentQuestionIndex = StartIndex;
            CorrectCount = 0;
        }
    }
}
