using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace EFQuestionAnswers.data
{
   public class QuestionAnswerContextFactory: IDesignTimeDbContextFactory<QuestionAnswerContext>

    {
        public QuestionAnswerContext CreateDbContext(string[] args)
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), $"..{Path.DirectorySeparatorChar}HW 2020-04-29 EFQuestionsAnswers"))
                .AddJsonFile("appsettings.json")
                .AddJsonFile("appsettings.local.json", optional: true, reloadOnChange: true).Build();

            return new QuestionAnswerContext(config.GetConnectionString("ConStr"));
        }
    }
    
    
}
