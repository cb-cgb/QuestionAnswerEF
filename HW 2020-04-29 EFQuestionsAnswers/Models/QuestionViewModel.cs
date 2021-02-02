using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EFQuestionAnswers.data;

namespace HW_2020_04_29_EFQuestionsAnswers.Models
{
    public class QuestionViewModel
    {
        public Question Question { get; set; }
        public int CurrentUserId { get; set; }
    }
}
