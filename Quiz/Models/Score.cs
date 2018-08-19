using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Quiz.Models
{
    public class Score
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public byte score { get; set; }
    }
}