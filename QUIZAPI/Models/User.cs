using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QuizAPI.Models
{
    public class User
    {
        public int Id { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        public string Salt { get; set; }

        public string Firstname { get; set; }

        public string Middlename { get; set; }

        public string Lastname { get; set; }

        public int? GroupId { get; set; }
    }
}