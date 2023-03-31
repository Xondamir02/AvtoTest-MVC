using Microsoft.AspNetCore.Mvc;
using Proj1.Models;
using Proj1.Sevices;

namespace Proj1.Controllers
{
    public class UsersController : Controller
    {
        public IActionResult SignIn()
        {
            return View();
        }

        public IActionResult SignInPost(SignInUserModel signInUserModel)
        {
            var user=UserService.Users.FirstOrDefault(u=>u.Username==signInUserModel.Username
            && u.Password==signInUserModel.Password);

            if (user == null)
                return RedirectToAction("SignIn");
            HttpContext.Response.Cookies.Append("UserId",user.Id);
            return RedirectToAction("Profile");
        }
        
        public IActionResult SignUpPost(CreateUserModel createUser)
        {
            var user = new User()
            {
                Id = Guid.NewGuid().ToString(),
                Name = createUser.Name,
                Password = createUser.Password,
                Username = createUser.Username,
                PhotoPath = SavePhoto(createUser.Photo),
                TicketResults = new List<TicketResult>()
                
            };

            UserService.Users.Add(user);

            HttpContext.Response.Cookies.Append("UserId",user.Id);

            return RedirectToAction("Index", "Home");
        }
        public IActionResult SignUp()
        {
            return View();
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
        private string SavePhoto(IFormFile file)
        {
            if(!Directory.Exists("wwwroot/UserImages"))
                Directory.CreateDirectory("wwwroot/UserImages");

            var fileName=Guid.NewGuid().ToString()+".jpg";
            var ms = new MemoryStream();
            file.CopyTo(ms);
            System.IO.File.WriteAllBytes(Path.Combine("wwwroot","UserImages",fileName),ms.ToArray());
            return "/UserImages/"+fileName;
        }
        public IActionResult LogOut()
        {
            HttpContext.Response.Cookies.Delete("UserId");
            return RedirectToAction("SignIn");
        }
    }
}
