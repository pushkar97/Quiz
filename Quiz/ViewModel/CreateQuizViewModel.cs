using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Quiz.Models;

namespace Quiz.ViewModel
{
    public class CreateQuizViewModel
    {
        public Question question { get; set; }
        public string answer { get; set; }
        public int UserId { get; set; }
    }
}