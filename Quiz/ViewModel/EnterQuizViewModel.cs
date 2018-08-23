using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Quiz.Models;

namespace Quiz.ViewModel
{
    public class EnterQuizViewModel
    {
        public string Answer { get; set; }
        public Question Question { get; set; }
    }
}