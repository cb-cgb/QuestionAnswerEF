using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace EFQuestionAnswers.data
{
    public class QuestionRepository
    {

        private readonly String _conn;

        public QuestionRepository(String conn)
        {
            _conn = conn;
        }

        public IEnumerable<Question> GetQuestions()
        {
            using (var context = new QuestionAnswerContext(_conn))
            {
                return context.Questions .Include( q=> q.User)
                                         .Include(q => q.UserQuestionLikes)
                                               .ThenInclude(u => u.User)
                                         .Include(q => q.QuestionTags)
                                               .ThenInclude(t => t.Tag)
                                        .OrderByDescending(q => q.Date).ToList();
                                        
            }            
        }

        public Question GetQuestionById(int Id)
        {
            using (var context = new QuestionAnswerContext(_conn))
            {
                return context.Questions.Include(q => q.User)
                                        .Include(q => q.UserQuestionLikes)
                                        .Include(q => q.Answers)
                                          .ThenInclude(a => a.UserAnswerLikes)
                                        .Include(q => q.QuestionTags).ThenInclude(t => t.Tag)
                                        .FirstOrDefault(q => q.Id == Id);
            }
        }

        private Tag CheckTagExists(String tag)
        {
            using (var context = new QuestionAnswerContext(_conn))
            {
                return context.Tags.FirstOrDefault(t => t.Name == tag);
            }
        }

        public void AddQuestion(Question question, List<String> tagList)
        {
            using (var context = new QuestionAnswerContext(_conn))
            {
                question.Date =  DateTime.Now;
                                
                context.Questions.Add(question);
                context.SaveChanges();

                foreach (String t in tagList) 
                {                    
                    var tag = CheckTagExists(t);
                    
                    if (tag == null)
                    {
                        Tag newtag= new Tag{ Name=t};
                        AddTag(newtag);
                        
                       tag = newtag;//used to add to questiontags
                    }

                    context.QuestionTags.Add(new QuestionTags
                    {
                       QuestionId = question.Id,
                       TagId = tag.Id
                    });
                    context.SaveChanges();
                }                
            }
        }

        public void AddTag (Tag t)
        {
            using (var context = new QuestionAnswerContext(_conn))
            {
                context.Tags.Add(t);
                context.SaveChanges();
            }
        }
             
        public void AddLikeQuestion(int questionId, int userId)
        {
            using (var context = new QuestionAnswerContext(_conn))
            {
                var question = GetQuestionById(questionId);
                                
                context.UserQuestionLikes.Add(new UserQuestionLikes
                {
                    QuestionId = questionId,
                    UserId = userId            
                });              
                
                
                context.SaveChanges();

                //return (question.UserQuestionLikes.Count());
            }
        }

        public int GetQuestionLikes(int questionId)
        {
            using (var context = new QuestionAnswerContext(_conn))
            {
                var question = GetQuestionById(questionId);
                return question.UserQuestionLikes.Count();
            }
        }
        public int GetQuestionLikes(int questionId, int userId)
        {
            using (var context = new QuestionAnswerContext(_conn))
            {
                var question = GetQuestionById(questionId);
                return question.UserQuestionLikes.Select(u=>u.UserId ==userId).Count();
            }
        }

        public void AddAnswer(Answer answer)
        {
            using (var context = new QuestionAnswerContext(_conn))
            {
                answer.Date = DateTime.Now;
                context.Add(answer);
                context.SaveChanges();
            }
        }

        public int AddLikeAnswer(UserAnswerLikes la)
        {
            using (var context = new QuestionAnswerContext(_conn))
            {
                context.UserAnswerLikes.Add(new UserAnswerLikes
                {
                    AnswerId = la.AnswerId,
                    UserId = la.UserId
                }); ;
                context.SaveChanges();
                return (la.Answer.UserAnswerLikes.Count());


            }
        }

      


        public void AddUser(User u)
        {
            using (var context = new QuestionAnswerContext(_conn))
            {

                //first check if the email already exists for a user
                var user = GetUserByEmail(u.Email);

                if (user is null)
                {
                   u.PasswordHash = BCrypt.Net.BCrypt.HashPassword(u.PasswordHash);
                                    BCrypt.Net.BCrypt.HashPassword(u.PasswordHash);

                    context.Users.Add(u);
                    context.SaveChanges();
                }

            }
        }

        public User GetUserByEmail(String email)
            {
                using (var context = new QuestionAnswerContext(_conn))
                {
                  return context.Users.FirstOrDefault(u => u.Email ==email);
                }
            }

        public User Login (User u)
        {
            User user = GetUserByEmail(u.Email);

            if (user is null )
            {
                return null;
            }

            //var hash = BCrypt.Net.BCrypt.HashPassword(u.PasswordHash);
            bool validPass = BCrypt.Net.BCrypt.Verify(u.PasswordHash,user.PasswordHash);


            if (!validPass)  //pw didn't match pw stored in db
            {
                return null;
            }

            return user;
                
        }
    }
}
