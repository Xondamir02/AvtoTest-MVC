using AutoTestBot.Models;
using Proj1.Sevices;

namespace Proj1.Models;

public class User
{
    public string? Id { get; set; }
    public string? Username { get; set; }
    public string? Password { get; set; }
    public string? Name { get; set; }
    public string? PhotoPath { get; set; }
    public int? CurrentTicketIndex { get; set; }
    public Ticket? CurrentTicket
    {
        get
        {
            if (CurrentTicketIndex == null)
            {
                return null;
            }
            return UserService._ticketRepository.GetTicket(CurrentTicketIndex.Value);
        }
    }

    public List<Ticket> Tickets => UserService._ticketRepository.GetTicketList(Id);

}

