using Microsoft.AspNetCore.Mvc;
using Proj1.Models;
using Proj1.Sevices;

namespace Proj1.Controllers;

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
        if (createUser.Photo == null || createUser.Photo == default)
        {
            ViewBag.NameError = "Name is null or empty";
            return View();
        }

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
        
        UserService.Register(createUser, HttpContext);

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
        var user = UserService._userRepository.GetUserByUsername(signInUserModel.Username!);

        if (user == null || user.Password != signInUserModel.Password)
            return RedirectToAction("SignIn");

        HttpContext.Response.Cookies.Append("user_Id", user.Id);

        return RedirectToAction("Profile");
    }

    public IActionResult Profile()
    {
        var user = UserService.GetCurrentUser(HttpContext);

        if (user == null)
        {
            return RedirectToAction("SignUp");
        }

        return View(user);
    }

    public IActionResult LogOut()
    {
        UserService.LogOut(HttpContext);

        return RedirectToAction("SignIn");
    }

    public IActionResult ChangeLanguage(string language)
    {
        QuestionService.Instance.LoadJson(language);

        return RedirectToAction("Index", "Home");
    }

    [HttpGet]
    public IActionResult ChangeName()
    {
        return View();
    }
    [HttpPost]
    public IActionResult ChangeName(ChangeUserModel changeUserModel)
    {
        changeUserModel.Id = HttpContext.Request.Cookies["user_Id"];
        var user = UserService._userRepository.GetUSerById(changeUserModel.Id);
        //var user = UserService.Users.FirstOrDefault(u => u.Id == changeUserModel.Id);
        //var user=UserService._userRepository.GetUSerById(user.Id);
        user.Name = changeUserModel.Name;

        HttpContext.Response.Cookies.Append("user_Id", user.Id);

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
        changeUserModel.Id = HttpContext.Request.Cookies["user_Id"];
        var user = UserService._userRepository.GetUSerById(changeUserModel.Id);

        user.Username = changeUserModel.Username;

        HttpContext.Response.Cookies.Append("user_Id", user.Id);

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
        changeUserModel.Id = HttpContext.Request.Cookies["user_Id"];
        var user = UserService._userRepository.GetUSerById(changeUserModel.Id);

        user.Password = changeUserModel.Password;

        HttpContext.Response.Cookies.Append("user_Id", user.Id);

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
        changeUserModel.Id = HttpContext.Request.Cookies["user_Id"];
        var user = UserService._userRepository.GetUSerById(changeUserModel.Id);

        user.PhotoPath = UserService.SavePhoto(changeUserModel.Photo);

        HttpContext.Response.Cookies.Append("user_Id", user.Id);

        return RedirectToAction("Profile");
    }
}
