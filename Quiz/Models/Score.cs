using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Quiz.Models
{
    public class Score
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public User User { get; set; }

        [Required]
        public string Username { get; set; }

        public byte score { get; set; }
    }
}