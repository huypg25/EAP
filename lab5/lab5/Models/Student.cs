using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace lab5.Models
{
    public class Student
    {
        public string StudentId { get; set; }
        public int ClassId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public string BirthDay { get; set; }
        public bool Gender { get; set; }
        public string Phone { get; set; }
        public string Picture { get; set; }
    }
}