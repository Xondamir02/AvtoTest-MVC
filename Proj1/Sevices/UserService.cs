using AutoTestBot.Models;
using Proj1.Models;
using Proj1.Repositories;

namespace Proj1.Sevices;

public static class UserService
{
    //public static List<User> Users = new List<User>();

    private const string UserIdCookieKey = "user_id";
    public static readonly TicketRepository _ticketRepository = new TicketRepository();
    public static readonly UserRepository _userRepository = new UserRepository();
    public static void Register(CreateUserModel createUser, HttpContext httpContext)
    {
        var user = new User
        {
            Id = Guid.NewGuid().ToString(),
            Name = createUser.Name!,
            Password = createUser.Password!,
            Username = createUser.Username!,
            PhotoPath = SavePhoto(createUser.Photo!),
            //Tickets = new List<Ticket>()
        };

        CreateUserTickets(user);

        _userRepository.AddUser(user);

        httpContext.Response.Cookies.Append(UserIdCookieKey, user.Id);
    }

    public static bool LogIn(SignInUserModel signInUserModel, HttpContext httpContext)
    {
        var user = _userRepository.GetUserByUsername(signInUserModel.Username);

        if (user == null || user.Password != signInUserModel.Password)
            return false;

        httpContext.Response.Cookies.Append(UserIdCookieKey, user.Id);

        return true;
    }

    public static User? GetCurrentUser(HttpContext context)
    {
        if (context.Request.Cookies.ContainsKey(UserIdCookieKey))
        {
            var userId = context.Request.Cookies[UserIdCookieKey];
            var user = _userRepository.GetUSerById(userId);

            return user;
        }

        return null;
    }

    public static bool IsLoggedIn(HttpContext context)
    {
        if (!context.Request.Cookies.ContainsKey(UserIdCookieKey)) return false;

        var userId = context.Request.Cookies[UserIdCookieKey];
        var user = _userRepository.GetUSerById(userId);

        return user != null;
    }


    public static void LogOut(HttpContext httpContext)
    {
        httpContext.Response.Cookies.Delete(UserIdCookieKey);
    }

    private static void CreateUserTickets(User user)
    {
        for (var i = 0; i < QuestionService.Instance.TicketsCount; i++)
        {
            var startIndex = i * QuestionService.Instance.TicketQuestionsCount + 1;
            //user.Tickets.Add(new Ticket()
            //{
            //    Id = i,
            //    CurrentQuestionIndex = startIndex,
            //    StartIndex = startIndex,
            //    QuestionsCount = QuestionService.Instance.TicketQuestionsCount
            //});
            _ticketRepository.AddTicket(new Ticket()
            {
                Id = i,
                UserId = user.Id,
                CurrentQuestionIndex = startIndex,
                StartIndex = startIndex,
                QuestionsCount = QuestionService.Instance.TicketQuestionsCount
            });
        }
    }

    public static string SavePhoto(IFormFile file)
    {
        if (!Directory.Exists("wwwroot/UserImages"))
            Directory.CreateDirectory("wwwroot/UserImages");

        var fileName = Guid.NewGuid() + ".jpg";
        var ms = new MemoryStream();
        file.CopyTo(ms);
        System.IO.File.WriteAllBytes(Path.Combine("wwwroot", "UserImages", fileName), ms.ToArray());

        return "/UserImages/" + fileName;
    }


}