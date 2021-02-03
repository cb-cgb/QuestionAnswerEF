using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace EFQuestionAnswers.data
{



    public class Question
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public DateTime Date { get; set; }
        public List<UserQuestionLikes> UserQuestionLikes { get; set; }
        public List<QuestionTags> QuestionTags { get; set; }
        public List<Answer> Answers { get; set; }
    }

    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public List<UserQuestionLikes> UserQuestionLikes { get; set; }
        public List<UserAnswerLikes> UserAnswerLikes { get; set; }
 
    }
        public class UserQuestionLikes
    {
        public int UserId { get; set; }
        public int QuestionId { get; set; }
        public User User { get; set; }
        public Question Question { get; set; }
    }
    public class Answer
    {
        public int Id { get; set; }
        public String Text { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public DateTime Date { get; set; }
        public List<UserAnswerLikes> UserAnswerLikes { get; set; }
        public int QuestionId { get; set; }
        public Question Question { get; set; }
    }

    public class UserAnswerLikes
    {
        public int UserId { get; set; }
        public int AnswerId { get; set; }
        public User User { get; set; }
        public Answer Answer { get; set; }
    }

    public class Tag
    {
        public int Id { get; set; }
        public String Name { get; set; }
        public List <QuestionTags> QuestionTags { get; set; }
    }

    public class QuestionTags
    {
        public int QuestionId { get; set; }
        public int TagId { get; set; }
        public Question Question { get; set; }
        public Tag Tag { get; set; }
    }
}
