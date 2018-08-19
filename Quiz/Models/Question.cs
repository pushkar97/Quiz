using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Quiz.Models
{
    public class Question
    {
        public int Id { get; set; }
        public string question { get; set; }
        public string option1 { get; set; }
        public string option2 { get; set; }
    }
}