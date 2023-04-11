using AutoTestBot.Models;

namespace Proj1.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string? PhotoPath { get; set; }
        public string LanguageJsonName { get; set; }
        public bool IsSellected { get; set; }
        public int StartQiuestionId { get; set; }

        public int? CurrentTicketIndex { get; set; }
        public Ticket? CurrentTicket => Tickets.FirstOrDefault(t => t.Index == CurrentTicketIndex);
        public List<Ticket> Tickets { get; set; } = null!;
    }

}
