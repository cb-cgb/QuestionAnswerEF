using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Microsoft.EntityFrameworkCore;


namespace EFQuestionAnswers.data
{
    public class QuestionAnswerContext : DbContext
    {

        private string _conn;
        public QuestionAnswerContext(String conn)
        {
            _conn = conn;
        }

        public DbSet<Question> Questions { get; set; }
        public DbSet<Answer> Answers { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserQuestionLikes> UserQuestionLikes { get; set; }
        public DbSet<UserAnswerLikes> UserAnswerLikes { get; set; }
        public DbSet<QuestionTags> QuestionTags { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_conn);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Taken from here:
            //https://www.learnentityframeworkcore.com/configuration/many-to-many-relationship-configuration

            //this foreach is needed to prevent a db cascade error when creating the foreign key
            foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            {
                relationship.DeleteBehavior = DeleteBehavior.Restrict;
            }

           /* modelBuilder.Entity<UserQuestionLikes>()
              .HasKey(uq => new { uq.QuestionId, uq.UserId });
            modelBuilder.Entity<UserQuestionLikes>()
                .HasOne(uq => uq.Question)
                .WithMany(q => q.UserQuestionLikes)
                .HasForeignKey(uq => uq.QuestionId);
            modelBuilder.Entity<UserQuestionLikes>()
                .HasOne(uq => uq.User)
                .WithMany(u => u.UserQuestionLikes)
                .HasForeignKey(uq => uq.UserId);

            modelBuilder.Entity<UserAnswerLikes>()
               .HasKey(ua => new { ua.AnswerId, ua.UserId });
            modelBuilder.Entity<UserAnswerLikes>()
                .HasOne(ua => ua.Answer)
                .WithMany(a => a.UserAnswerLikes)
                .HasForeignKey(ua => ua.AnswerId);
            modelBuilder.Entity<UserAnswerLikes>()
                .HasOne(ua => ua.User)
                .WithMany(u => u.UserAnswerLikes)
                .HasForeignKey(ua => ua.UserId);


            modelBuilder.Entity<QuestionTags>()
                .HasKey(qt => new { qt.QuestionId, qt.TagId });
            modelBuilder.Entity<QuestionTags>()
                .HasOne(qt => qt.Question)
                .WithMany(q => q.QuestionTags)
                .HasForeignKey(qt => qt.QuestionId);
            modelBuilder.Entity<QuestionTags>()
                .HasOne(qt => qt.Tag)
                .WithMany(t => t.QuestionTags)
                .HasForeignKey(qt => qt.TagId);*/

            
              //set up composite primary key
              modelBuilder.Entity<QuestionTags>()
                  .HasKey(qt => new { qt.QuestionId, qt.TagId });
              //set up foreign key from QuestionsTags to Questions
              modelBuilder.Entity<QuestionTags>()
                  .HasOne(qt => qt.Question)
                  .WithMany(q => q.QuestionTags)
                  .HasForeignKey(qt => qt.QuestionId);
              //set up foreign key from QuestionsTags to Tags
              modelBuilder.Entity<QuestionTags>()
                  .HasOne(qt => qt.Tag)
                  .WithMany(t => t.QuestionTags)
                  .HasForeignKey(qt => qt.TagId);


              //set up composite primary key
              modelBuilder.Entity<UserAnswerLikes>()
                 .HasKey(ua => new { ua.AnswerId, ua.UserId });
              //set up foreign key from UserAnswerLikes to Answers
              modelBuilder.Entity<UserAnswerLikes>()
                  .HasOne(ua => ua.Answer)
                  .WithMany(a => a.UserAnswerLikes)
                  .HasForeignKey(ua => ua.AnswerId);
              //set up foreign key from UserAnswerLikes to users
              modelBuilder.Entity<UserAnswerLikes>()
                  .HasOne(ua => ua.User)
                  .WithMany(u => u.UserAnswerLikes)
                  .HasForeignKey(ua => ua.UserId);


              //set up composite primary key
              modelBuilder.Entity<UserQuestionLikes>()
                 .HasKey(uq => new { uq.QuestionId, uq.UserId });
              //set up foreign key from userQuestionLikes to Questions
              modelBuilder.Entity<UserQuestionLikes>()
                  .HasOne(uq => uq.Question)
                  .WithMany(q => q.UserQuestionLikes)
                  .HasForeignKey(uq => uq.QuestionId);
              //set up foreign key from UserQuestionLikes to users
              modelBuilder.Entity<UserQuestionLikes>()
                  .HasOne(uq => uq.User)
                  .WithMany(u => u.UserQuestionLikes)
                  .HasForeignKey(uq => uq.UserId);

              
        }

       
    }
}
