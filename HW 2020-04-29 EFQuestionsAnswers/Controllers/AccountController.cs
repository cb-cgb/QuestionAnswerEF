using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using HW_2020_04_29_EFQuestionsAnswers.Models;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using EFQuestionAnswers.data;
using Microsoft.Extensions.Configuration;

namespace HW_2020_04_29_EFQuestionsAnswers.Controllers
{
    public class AccountController : Controller
    {
        private string _conn;
        public AccountController(IConfiguration configuration)
        {
            _conn = configuration.GetConnectionString("ConStr");
        }

        public IActionResult Signup ()
        {
            return View();

        }

        [HttpPost]
        public IActionResult Signup(User u)
        {
            var db = new QuestionRepository(_conn);
            var user = db.GetUserByEmail(u.Email);
            
            if(user != null) //email already exists
            {
                TempData["Message"]="Email already used by another user. Please log in with this email or register with a different one.";
                return Redirect("/Account/Login");
            }

            db.AddUser(u);
            TempData["Success-Message"] = $"Thank you for signing up! You are registered with user: {u.Email}. Please log into proceed.";
            
            return Redirect("/Account/Login");
        }

        public IActionResult Login(string returnURL)
        {
            ViewBag.Message = returnURL;
            return View();
        }

        [HttpPost]
        public IActionResult Login(User u, string returnUrl)
        {
            
            var db = new QuestionRepository(_conn);
            var user = db.Login(u);

            if (user == null)
            {
                TempData["Message"] = "Invalid user or password credentials. Please log in again.";
                return Redirect("/Account/Login");
            }

            //credentials are correct, log the user in  . this creates the "ASPNetCore" cookie. 
            var claims = new List<Claim>
            {
                new Claim("user", u.Email)
            };
            HttpContext.SignInAsync(new ClaimsPrincipal(
                new ClaimsIdentity(claims, "Cookies", "user", "role"))).Wait();

            if (!String.IsNullOrEmpty(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return Redirect("/Home/Index");
        }

        public IActionResult Logout (User u)
        {

            if (User.Identity.IsAuthenticated)
            {
                HttpContext.SignOutAsync().Wait(); //logs out the user
            }

            return Redirect("/Account/Login");
        }
            
    }
}
