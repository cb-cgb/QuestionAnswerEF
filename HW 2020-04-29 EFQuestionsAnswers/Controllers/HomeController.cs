using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using HW_2020_04_29_EFQuestionsAnswers.Models;
using Microsoft.Extensions.Configuration;
using EFQuestionAnswers.data;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;    
using System.Security.Claims;

namespace HW_2020_04_29_EFQuestionsAnswers.Controllers
{
    public class HomeController : Controller
    {
       
        private readonly string _conn;
        public HomeController(IConfiguration configuration )
        {
            _conn = configuration.GetConnectionString("ConStr");
        }

        public IActionResult Index()
        {
            var db = new QuestionRepository(_conn);
            return View(db.GetQuestions());
        }

        public IActionResult Question(int questionId)
        {
            var db = new QuestionRepository(_conn);
            var vm = new QuestionViewModel();
            vm.Question = db.GetQuestionById(questionId);
            if (User.Identity.IsAuthenticated)
            {
                
                vm.CurrentUserId = db.GetUserByEmail(User.Identity.Name).Id;
            }

            return View (vm);
            
        }


        public IActionResult AddTag(Tag t)
        {
            var db = new QuestionRepository(_conn);
            db.AddTag(t);
            return Redirect("/home/Question");
        }

        [Authorize]
        public IActionResult NewQuestion()
        {
            return View();
        }

        [HttpPost]
        [Authorize]
        public IActionResult AddQuestion(Question q, String tags)
        {
            var tagList = new List<String>();
            tagList = tags.Split(',').ToList();

            var db = new QuestionRepository(_conn);
           // var User = db.GetUserByEmail(User.Identity.Name);
            q.UserId = db.GetUserByEmail(User.Identity.Name).Id;
            db.AddQuestion(q, tagList);
            return Redirect("/Home/Index");


        }
        [Authorize]
        [HttpPost]
        public void AddQuestionLike(int questionId)
        {
            var db = new QuestionRepository(_conn);
            var user = db.GetUserByEmail(User.Identity.Name);
            db.AddLikeQuestion(questionId,user.Id);
           

        }

        public IActionResult GetQuestionLikes (int questionId)
        {

            var db = new QuestionRepository(_conn);
            var countLikes = db.GetQuestionLikes(questionId);
            return Json(countLikes); 
            //return Json(new { countLikes = db.GetQuestionLikes(questionId) });
        }

        public IActionResult GetQuestionLikesForCurrentUser(int questionId, int userId)
        {

            var db = new QuestionRepository(_conn);
            var countLikes = db.GetQuestionLikes(questionId);
            return Json(countLikes);
        }

        [HttpPost]
        [Authorize]
        public IActionResult AddAnswer(Answer a)
        {
            var db = new QuestionRepository(_conn);
            db.AddAnswer(a);
            return Redirect($"/home/Question?questionId={a.QuestionId}");
        }

        [Authorize] [HttpPost]
        public IActionResult AddAnswerLike(UserAnswerLikes u)
        {
            var db = new QuestionRepository(_conn);
            var countLikes=db.AddLikeAnswer(u);
            //var countLikes = u.Answer.UserAnswerLikes.Count();
            return Json(countLikes);

        }

    }
}
