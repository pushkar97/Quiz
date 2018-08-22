﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Quiz.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required]
        public string Username { get; set; }
    }
}