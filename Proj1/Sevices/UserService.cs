using Proj1.Models;

namespace Proj1.Sevices
{
    public static class UserService
    {
        public static List<User> Users = new List<User>();

        public static User? GetCurrentUser(HttpContext context)
        {
            if (context.Request.Cookies.ContainsKey("UserId"))
            {
                var userId = context.Request.Cookies["UserId"];
                var user = Users.FirstOrDefault(u => u.Id == userId);

                return user;
            }

            return null;
        }

        public static bool IsLoggedIn(HttpContext context)
        {
            if (!context.Request.Cookies.ContainsKey("UserId")) return false;

            var userId = context.Request.Cookies["UserId"];
            var user = Users.FirstOrDefault(u => u.Id == userId);

            return user != null;
        }
    }
}