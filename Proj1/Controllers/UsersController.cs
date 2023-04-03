using Microsoft.AspNetCore.Mvc;
using Proj1.Models;
using Proj1.Sevices;

namespace Proj1.Controllers
{
    public class UsersController : Controller
    {

        [HttpGet]
        public IActionResult SignUp()
        {
            return View();
        }



        [HttpPost]
        public IActionResult SignUp(CreateUserModel createUser)
        {
            if (string.IsNullOrEmpty(createUser.Username))
            {
                ViewBag.NameError = "Name is null or empty";
                return View();
            }
            if (string.IsNullOrEmpty(createUser.Password))
            {
                ViewBag.NameError = "Password is null or empty";
                return View();
            }

            var user = new User()
            {
                Id = Guid.NewGuid().ToString(),
                Name = createUser.Name,
                Password = createUser.Password,
                Username = createUser.Username,
                PhotoPath = SavePhoto(createUser.Photo),
                UserTickets = new List<UserTickets>()

            };

            var questions = QuestionService.Instance.Questions;
            var ticketsCount = questions.Count / 10;

            for (int i = 0; i < ticketsCount; i++)
            {
                var startIndex = i * 10 + 1;
                user.UserTickets.Add(new UserTickets()
                {
                    Index = i,
                    CurrentQuestionIndex = startIndex,
                    StartIndex = startIndex,
                    QuestionsCount = 10
                });
            }

            UserService.Users.Add(user);

            HttpContext.Response.Cookies.Append("UserId", user.Id);

            return RedirectToAction("Index", "Home");
        }




        

        [HttpGet]
        public IActionResult SignIn()
        {
            return View();
        }


        [HttpPost]
        public IActionResult SignIn(SignInUserModel signInUserModel)
        {
            var user = UserService.Users.FirstOrDefault(u => u.Username == signInUserModel.Username
            && u.Password == signInUserModel.Password);

            if (user == null)
                return RedirectToAction("SignIn");

            HttpContext.Response.Cookies.Append("UserId", user.Id);

            return RedirectToAction("Index", "Home");
        }


        public IActionResult Profile()
        {
            if (HttpContext.Request.Cookies.ContainsKey("UserId"))
            {
                var userId = HttpContext.Request.Cookies["UserId"];
                var user = UserService.Users.FirstOrDefault(u => u.Id == userId);

                if (user == null)
                    return RedirectToAction("SignIn");

                return View(user);
            }

            return RedirectToAction("SignUp");
        }

        public IActionResult LogOut()
        {
            HttpContext.Response.Cookies.Delete("UserId");
            return RedirectToAction("SignIn");
        }

        private string SavePhoto(IFormFile file)
        {
            if (!Directory.Exists("wwwroot/UserImages"))
                Directory.CreateDirectory("wwwroot/UserImages");

            var fileName = Guid.NewGuid().ToString() + ".jpg";
            var ms = new MemoryStream();
            file.CopyTo(ms);
            System.IO.File.WriteAllBytes(Path.Combine("wwwroot", "UserImages", fileName), ms.ToArray());

            return "/UserImages/" + fileName;
        }

        [HttpGet]
        public IActionResult ChangeName()
        {
            return View();
        }
        [HttpPost]
        public IActionResult ChangeName(ChangeUserModel changeUserModel)
        {
            changeUserModel.Id = HttpContext.Request.Cookies["UserId"];

            var user = UserService.Users.FirstOrDefault(u => u.Id == changeUserModel.Id);

            user.Name = changeUserModel.Name;

            HttpContext.Response.Cookies.Append("UserId", user.Id);

            return RedirectToAction("Profile");

        }

        [HttpGet]
        public IActionResult ChangeUserName()
        {
            return View();
        }
        [HttpPost]
        public IActionResult ChangeUserName(ChangeUserModel changeUserModel)
        {
            changeUserModel.Id = HttpContext.Request.Cookies["UserId"];

            var user = UserService.Users.FirstOrDefault(u => u.Id == changeUserModel.Id);

            user.Username = changeUserModel.Username;

            HttpContext.Response.Cookies.Append("UserId", user.Id);

            return RedirectToAction("Profile");
        }
        [HttpGet]
        public IActionResult ChangePassword()
        {
            return View();
        }
        [HttpPost]
        public IActionResult ChangePassword(ChangeUserModel changeUserModel)
        {
            changeUserModel.Id = HttpContext.Request.Cookies["UserId"];

            var user = UserService.Users.FirstOrDefault(u => u.Id == changeUserModel.Id);

            user.Password = changeUserModel.Password;

            HttpContext.Response.Cookies.Append("UserId", user.Id);

            return RedirectToAction("Profile");
        }

        [HttpGet]
        public IActionResult ChangePhoto()
        {
            return View();
        }
        [HttpPost]
        public IActionResult ChangePhoto(ChangeUserModel changeUserModel)
        {
            changeUserModel.Id = HttpContext.Request.Cookies["UserId"];

            var user = UserService.Users.FirstOrDefault(u => u.Id == changeUserModel.Id);

            user.PhotoPath = SavePhoto(changeUserModel.Photo);

            HttpContext.Response.Cookies.Append("UserId", user.Id);

            return RedirectToAction("Profile");
        }
    }
}

   