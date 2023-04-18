using Microsoft.AspNetCore.Mvc;
using Proj1.Models;
using Proj1.Sevices;

namespace Proj1.Controllers;

public class UsersController : Controller
{


    private readonly UserService _usersService;
    private readonly QuestionService _questionService;

    public UsersController(UserService usersService, QuestionService questionService)
    {
        _usersService = usersService;
        _questionService = questionService;
    }




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
        
        _usersService.Register(createUser, HttpContext);

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
        var user = _usersService._userRepository.GetUserByUsername(signInUserModel.Username!);

        if (user == null || user.Password != signInUserModel.Password)
            return RedirectToAction("SignIn");

        HttpContext.Response.Cookies.Append("user_Id", user.Id);

        return RedirectToAction("Profile");
    }

    public IActionResult Profile()
    {
        var user = _usersService.GetCurrentUser(HttpContext);

        if (user == null)
        {
            return RedirectToAction("SignUp");
        }

        return View(user);
    }

    public IActionResult LogOut()
    {
        _usersService.LogOut(HttpContext);

        return RedirectToAction("SignIn");
    }

    public IActionResult ChangeLanguage(string language)
    {
        _questionService.LoadJson(language);

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
        var user = _usersService._userRepository.GetUSerById(changeUserModel.Id);


        //var user = UserService.Users.FirstOrDefault(u => u.Id == changeUserModel.Id);
        //var user=UserService._userRepository.GetUSerById(user.Id);
        //user.Name = changeUserModel.Name;
        
        _usersService._userRepository.UpdateName(changeUserModel,user);
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
        var user = _usersService._userRepository.GetUSerById(changeUserModel.Id);

        _usersService._userRepository.UpdateUsername(changeUserModel, user);

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
        var user = _usersService._userRepository.GetUSerById(changeUserModel.Id);

       _usersService._userRepository.UpdateUserPassword(changeUserModel,user);

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
        var user = _usersService._userRepository.GetUSerById(changeUserModel.Id);

        

        string photoPath = _usersService.SavePhoto(changeUserModel.Photo);

        _usersService._userRepository.UpdateUserPhoto(photoPath,user);

        HttpContext.Response.Cookies.Append("user_Id", user.Id);

        return RedirectToAction("Profile");
    }
}
