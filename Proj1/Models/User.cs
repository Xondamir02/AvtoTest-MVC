namespace Proj1.Models
{
    public class User
    {
        public string Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string? PhotoPath { get; set; }

        public List<TicketResult> TicketResults { get;set; }
    }

}
